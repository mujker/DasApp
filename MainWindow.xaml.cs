using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using DasApp.Log4Net;
using DasApp.Models;
using DasApp.Socket;
using Oracle.ManagedDataAccess.Client;
using ServiceStack.Redis;
using SuperSocket.ClientEngine;

namespace DasApp
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string JkdSql = "SELECT JKD_ID, JKD_NAME, RMI_ID FROM yxjk_jkd where ISRUN = 1";
        public static string CryptKey = "hxsoft++";
        private IDbConnection _dbc;
        private bool _taskFlag = true;

        public List<YXJK_JKD> SoureJkds = new List<YXJK_JKD>();

        public MainWindow()
        {
            InitializeComponent();
            InitiJkd();
            StartMethod();
//            MultiTaskMethod();
            LogHelper.WriteLog("启动");
        }

        private void InitiJkd()
        {
            try
            {
                _dbc = new OracleConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
                var jkds = _dbc.Query<YXJK_JKD>(JkdSql);
                var yxjkJkds = jkds as IList<YXJK_JKD> ?? jkds.ToList();
                if (!yxjkJkds.Any())
                {
                    MessageBox.Show("无换热站");
                    return;
                }
                SoureJkds = yxjkJkds.ToList();
//                SoureJkds.Clear();
//                for (int i = 0; i < 100; i++)
//                {
//                    SoureJkds.Add(new YXJK_JKD() {JKD_ID = "tyzx"+i, JKD_NAME = "体验中心"+i});
//                }
                Grid.ItemsSource = SoureJkds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Source, ex);
            }
        }

        private void StartMethod()
        {
            Parallel.ForEach(SoureJkds, item =>
            {
                Task.Factory.StartNew(async delegate
                {
                    var client = new EasyClient();

                    /***
                     * 初始化socket连接, 接受返回数据处理
                     * HxReceiveFilter为自定义的协议
                     * ***/
                    client.Initialize(new HxReceiveFilter(), request =>
                    {
                        try
                        {
                            item.CURR_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            item.JKD_VALUE = request.Key;
                            using (var redis = new RedisClient(Settings.RedisIp, Settings.RedisPort, Settings.RedisPw))
                            {
                                try
                                {
                                    item.REDIS_SAVE = redis.Set(item.JKD_ID, item.JKD_VALUE).ToString();
//                                var s = redis.Get<string>(item.JKD_ID);
//                                Console.WriteLine(s);
                                }
                                catch (Exception e)
                                {
                                    item.REDIS_SAVE = bool.FalseString;
                                    LogHelper.WriteLog(e.Source, e);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(ex.Source, ex);
                        }
                    });
                    // Connect to the server
                    await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(Settings.RmiIp), Settings.RmiPort));

                    while (_taskFlag)
                        try
                        {
                            if (client.IsConnected)
                            {
                                //获取发送字符串
                                var enStr = DataPacketCodec.Encode($"rij,{item.JKD_ID}", CryptKey) + "#";
                                // Send data to the server
                                client.Send(Encoding.UTF8.GetBytes(enStr));
                            }
                            else
                            {
                                await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(Settings.RmiIp),
                                    Settings.RmiPort));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(ex.Source, ex);
                            // reconnet
                            await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(Settings.RmiIp),
                                Settings.RmiPort));
                        }
                        finally
                        {
                            await Task.Delay(3000);
                        }
                    await client.Close();
                });
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _taskFlag = false;
            LogHelper.WriteLog("停止");
            Environment.Exit(0);
        }
    }
}
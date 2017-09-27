using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using DasApp.Models;
using DasApp.Socket;
using Oracle.ManagedDataAccess.Client;
using MessageBox = System.Windows.MessageBox;

namespace DasApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDbConnection _dbc;
        private const string JkdSql = "SELECT JKD_ID, JKD_NAME, RMI_ID FROM yxjk_jkd where ISRUN = 1";

        public List<YXJK_JKD> SoureJkds = new List<YXJK_JKD>();
        private bool _taskFlag = true;

        public MainWindow()
        {
            InitializeComponent();
            InitiJkd();
            StartMethod();
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
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void StartMethod()
        {
            var groupBy = SoureJkds.GroupBy(item => new {item.RMI_ID}).Select(item => item.Key).ToList();
            foreach (var item in groupBy)
            {
                if (string.IsNullOrEmpty(item.RMI_ID))
                {
                    continue;
                }
                SocketWrapper sw = new SocketWrapper()
                {
                    IP = Settings.RmiIp,
                    Port = Settings.RmiPort
                };
                Parallel.ForEach(SoureJkds.Where(j => item.RMI_ID.Equals(j.RMI_ID)), jkd =>
                {
                    jkd._sw = sw;
                    Task.Factory.StartNew(delegate
                    {
                        while (_taskFlag)
                        {
                            jkd.SocketMethod();
                            Thread.Sleep(2000);
                        }
                    }, TaskCreationOptions.LongRunning);
                });
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _taskFlag = false;
            Environment.Exit(0);
        }
    }
}
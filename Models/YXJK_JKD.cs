using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DasApp.Log4Net;
using DasApp.Redis;
using DasApp.Socket;

namespace DasApp.Models
{
    public class YXJK_JKD : INotifyPropertyChanged
    {
        private string _JKD_ID;
        private string _JKD_NAME;
        private string _JKD_VALUE;
        private string _RMI_ID;
        private string _CURR_TIME;
        private string _REDIS_SAVE;

        private int sleepTime = 2000;
        public static string CryptKey = "hxsoft++";
        public SocketWrapper Sw;

        public void SocketMethod()
        {
            try
            {
                if (!Sw.IsOpen())
                {
                    Sw.Connect();
                }
                var strSendData = $"rij,{this.JKD_ID}";
                var ensendData = DataPacketCodec.Encode(strSendData, CryptKey);
                string desStr = Sw.Receive($"{ensendData}#");
                CURR_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (string.IsNullOrEmpty(desStr) || desStr.IndexOf("#") == 0)
                {
                    JKD_VALUE = "#";
                    SetRedisValue();
                    return;
                }
                if (desStr.IndexOf("#") > -1)
                {
                    desStr = desStr.Substring(0, desStr.IndexOf("#"));
                    JKD_VALUE = desStr + "#";
                }
                else
                {
                    JKD_VALUE = "#";
                }
                SetRedisValue();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Source, ex);
            }
        }

        public void SetRedisValue()
        {
            //            using (var client = ConnectionMultiplexer.Connect(Settings.ConOption))
            //            {
            //                REDIS_SAVE = client.GetDatabase().StringSet(JKD_ID, JKD_VALUE).ToString();
            //            }
            REDIS_SAVE = RedisManager.SetRedisValue(JKD_ID, JKD_VALUE).ToString();
        }

        public string CURR_TIME
        {
            get { return _CURR_TIME; }

            set
            {
                _CURR_TIME = value;
                OnPropertyChanged();
            }
        }

        public string JKD_ID
        {
            get { return _JKD_ID; }

            set
            {
                _JKD_ID = value;
                OnPropertyChanged();
            }
        }

        public string JKD_NAME
        {
            get { return _JKD_NAME; }

            set
            {
                _JKD_NAME = value;
                OnPropertyChanged();
            }
        }

        public string RMI_ID
        {
            get { return _RMI_ID; }

            set
            {
                _RMI_ID = value;
                OnPropertyChanged();
            }
        }


        public string REDIS_SAVE
        {
            get { return _REDIS_SAVE; }

            set
            {
                _REDIS_SAVE = value;
                OnPropertyChanged();
            }
        }

        public string JKD_VALUE
        {
            get { return _JKD_VALUE; }

            set
            {
                _JKD_VALUE = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
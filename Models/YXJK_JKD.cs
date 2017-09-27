using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DasApp.Socket;
using ServiceStack.Redis;

namespace DasApp.Models
{
    public class YXJK_JKD : INotifyPropertyChanged
    {
        private string _JKD_ID;
        private string _JKD_NAME;
        private string _JKD_VALUE;
        private string _RMI_ID;
        private string _CurrTime;

        private int sleepTime = 2000;
        private const string CryptKey = "hxsoft++";
        public SocketWrapper _sw;

        public void SocketMethod()
        {
            try
            {
                if (!_sw.IsOpen())
                {
                    _sw.Connect();
                }
                var strSendData = $"rij,{this.JKD_ID}";
                var ensendData = DataPacketCodec.Encode(strSendData, CryptKey);
                string desStr = _sw.Receive($"{ensendData}#");
                CurrTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                Console.WriteLine(ex.Message);
            }
        }

        public void SetRedisValue()
        {
            using (RedisClient redis = new RedisClient(Settings.RedisIp, Settings.RedisPort)
            {
                Password = Settings.RedisPw
            })
            {
                redis.Set(JKD_ID, JKD_VALUE);
            }
        }

        public string CurrTime
        {
            get { return _CurrTime; }

            set
            {
                _CurrTime = value;
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

        public string JKD_VALUE
        {
            get { return _JKD_VALUE; }

            set
            {
                _JKD_VALUE = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
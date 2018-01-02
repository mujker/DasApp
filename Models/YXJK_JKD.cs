using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DasApp.Models
{
    public class YXJK_JKD : INotifyPropertyChanged
    {
        private string _CURR_TIME;
        private string _JKD_ID;
        private string _JKD_NAME;
        private string _JKD_VALUE;
        private string _REDIS_SAVE;
        private string _RMI_ID;


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
﻿using System.ComponentModel;
using System.Timers;

namespace IISManager.ViewModels
{
    public class UserMessage : INotifyPropertyChanged
    {
        private readonly Timer timer;
        private string message;
        private UserMessageType userMessageType;

        public UserMessage()
        {
            timer = new Timer();
            timer.Interval = 10000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Message"));
                }

                if (value != null)
                {
                    timer.Stop();
                    timer.Start();
                }
            }
        }

        public UserMessageType Type
        {
            get { return userMessageType; }
            set
            {
                if (this.userMessageType != value)
                {
                    this.userMessageType = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Type"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        public void SetInfo(string message)
        {
            Message = message;
            Type = UserMessageType.Info;
        }

        public void SetWarn(string message)
        {
            Message = message;
            Type = UserMessageType.Warn;
        }

        public void SetError(string message)
        {
            Message = message;
            Type = UserMessageType.Error;
        }

        public void Clear(string message)
        {
            Message = null;
            Type = UserMessageType.Info;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Message = null;
        }
    }
}

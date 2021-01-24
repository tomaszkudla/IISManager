using System;
using System.Windows.Threading;

namespace IISManagerUI
{
    public sealed class RefreshingTimer
    {
        private static readonly RefreshingTimer instance = new RefreshingTimer();
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool isPaused;

        static RefreshingTimer()
        {
        }

        private RefreshingTimer()
        {
            SetupTimer();
        }

        public static RefreshingTimer Instance
        {
            get
            {
                return instance;
            }
        }

        public void PauseForNextTick()
        {
            isPaused = true;
        }

        public EventHandler Tick { get; set; }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                Tick?.Invoke(sender, e);
            }
        }

        private void SetupTimer()
        {
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(Configuration.RefreshingInterval);
            timer.Start();
        }
    }
}

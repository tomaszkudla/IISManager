using System;
using System.Windows;

namespace IISManagerUI
{
    public static class Utils
    {
        public static void SafeExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

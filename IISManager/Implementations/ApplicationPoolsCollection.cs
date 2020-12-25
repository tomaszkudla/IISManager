using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IISManager.Implementations
{
    public class ApplicationPoolsCollection : ObservableCollection<ApplicationPool>
    {
        public void Refresh()
        {
            for (var i = 0; i < this.Count(); i++)
            {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            foreach (var item in this)
            {
                item.WorkerProcesses.Refresh();
            }
        }
    }
}

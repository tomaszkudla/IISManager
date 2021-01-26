using IISManager.Utils;
using System.Collections.Generic;
using System.ComponentModel;

namespace IISManager.ViewModels
{
    public class ApplicationPoolsList : INotifyPropertyChanged
    {
        private List<ApplicationPool> value = new List<ApplicationPool>();
        private HashSet<string> selectedApplicationPools = new HashSet<string>();
        private string filter;
        private SortingType sorting;

        public HashSet<string> SelectedApplicationPools
        {
            get { return selectedApplicationPools; }
            set
            {
                selectedApplicationPools = value;
                for (int i = 0; i < this.value.Count; i++)
                {
                    var currentValue = this.value[i];
                    currentValue.IsSelected = selectedApplicationPools.Contains(currentValue.Name);
                }

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedApplicationPools"));
            }
        }

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                Value = this.value.FilterAppPools(filter).OrderAppPoolsBy(sorting);
                PropertyChanged(this, new PropertyChangedEventArgs("Filter"));
            }
        }

        public SortingType Sorting
        {
            get { return sorting; }
            set
            {
                sorting = value;
            }
        }

        public List<ApplicationPool> Value
        {
            get { return value.FilterAppPools(filter).OrderAppPoolsBy(sorting); }
            set
            {
                var valueFilteredAndSorted = value.FilterAppPools(filter).OrderAppPoolsBy(sorting);
                if (this.value.Count == valueFilteredAndSorted.Count)
                {
                    for (int i = 0; i < valueFilteredAndSorted.Count; i++)
                    {
                        var currentValue = this.value[i];
                        var newValue = valueFilteredAndSorted[i];

                        currentValue.Name = newValue.Name;
                        currentValue.IsSelected = SelectedApplicationPools.Contains(newValue.Name);
                        currentValue.State = newValue.State;
                        currentValue.WorkerProcesses.Value = newValue.WorkerProcesses.Value;
                        currentValue.Applications.Value = newValue.Applications.Value;
                    }
                }
                else
                {
                    this.value = valueFilteredAndSorted;
                    for (int i = 0; i < valueFilteredAndSorted.Count; i++)
                    {
                        this.value[i].IsSelected = SelectedApplicationPools.Contains(valueFilteredAndSorted[i].Name);
                    }

                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

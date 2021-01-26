using System.Collections.Generic;
using System.ComponentModel;

namespace IISManager.ViewModels
{
    public class ApplicationsList : INotifyPropertyChanged
    {
        private List<Application> value;

        public ApplicationsList(List<Application> applications)
        {

            value = applications;    
        }

        public List<Application> Value
        {
            get { return value; }
            set
            {
                if (this.value.Count == value.Count)
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        var currentValue = this.value[i];
                        var newValue = value[i];

                        currentValue.ApplicationPoolName = newValue.ApplicationPoolName;
                        currentValue.Path = newValue.Path;
                        currentValue.DirPath = newValue.DirPath;
                        currentValue.WebPath = newValue.WebPath;
                    }
                }
                else
                {
                    this.value = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IISManager.Implementations
{
    public class ApplicationPoolsList : INotifyPropertyChanged
	{
		private List<ApplicationPool> value;
		private HashSet<string> selectedApplicationPools = new HashSet<string>();

		public ApplicationPoolsList()
		{
			value = new List<ApplicationPool>();
		}

		public ApplicationPoolsList(List<ApplicationPool> value)
		{
			this.value = value;
		}

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
			}
        }

		public List<ApplicationPool> Value
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

						currentValue.Name = newValue.Name;
						currentValue.IsSelected = SelectedApplicationPools.Contains(newValue.Name);
						currentValue.State = newValue.State;
						currentValue.WorkerProcesses.Value = newValue.WorkerProcesses.Value;
					}
				}
				else
                {
					this.value = value;
                    for (int i = 0; i < value.Count; i++)
                    {
						this.value[i].IsSelected = SelectedApplicationPools.Contains(value[i].Name);
					}

					PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
			}
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}
}

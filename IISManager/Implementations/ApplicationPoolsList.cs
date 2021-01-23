using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IISManager.Implementations
{
    public class ApplicationPoolsList : INotifyPropertyChanged
	{
		private List<ApplicationPool> value;

		public ApplicationPoolsList()
		{
			value = new List<ApplicationPool>();
		}

		public ApplicationPoolsList(List<ApplicationPool> value)
		{
			this.value = value;
		}

		public HashSet<string> SelectedApplicationPools { get; } = new HashSet<string>();

		public List<ApplicationPool> Value
		{
			get { return value; }
			set
			{
				if (this.value.Count == value.Count)
				{
                    for (int i = 0; i < value.Count; i++)
                    {
						this.value[i].Name = value[i].Name;
						this.value[i].IsSelected = SelectedApplicationPools.Contains(value[i].Name);
						this.value[i].State = value[i].State;
						this.value[i].WorkerProcesses.Value = value[i].WorkerProcesses.Value;
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

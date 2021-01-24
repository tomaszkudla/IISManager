using System.Collections.Generic;
using System.ComponentModel;

namespace IISManager.ViewModels
{
    public class WorkerProcessesList : INotifyPropertyChanged
	{
		private List<WorkerProcess> value;

		public WorkerProcessesList()
		{
			value = new List<WorkerProcess>();
		}

		public WorkerProcessesList(List<WorkerProcess> value)
		{
			this.value = value;
		}

		public List<WorkerProcess> Value
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

						currentValue.Id = newValue.Id;
						currentValue.State = newValue.State;
						currentValue.CpuUsage = newValue.CpuUsage;
						currentValue.MemoryUsage = newValue.MemoryUsage;
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

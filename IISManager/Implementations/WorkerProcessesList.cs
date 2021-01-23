using System.Collections.Generic;
using System.ComponentModel;

namespace IISManager.Implementations
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
						this.value[i].Id = value[i].Id;
						this.value[i].State = value[i].State;
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

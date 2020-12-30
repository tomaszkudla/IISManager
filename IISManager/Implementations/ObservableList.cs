using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IISManager.Implementations
{
	public class ObservableList<T> : INotifyPropertyChanged
	{
		private List<T> value;

		public ObservableList()
		{
			value = new List<T>();
		}

		public ObservableList(List<T> value)
		{
			this.value = value;
		}

		public List<T> Value
		{
			get { return value; }
			set
			{
				if (!Enumerable.SequenceEqual(this.value, value))
				{
					this.value = value;
					PropertyChanged(this, new PropertyChangedEventArgs("Value"));
				}
			}
		}

		public static implicit operator List<T>(ObservableList<T> val)
		{
			return val.value;
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
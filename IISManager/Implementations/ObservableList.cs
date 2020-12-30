using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace IISManager.Implementations
{
	public class ObservableList<T> : INotifyPropertyChanged, IEnumerable
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
				this.value = value;
				PropertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		public static implicit operator List<T>(ObservableList<T> val)
		{
			return val.value;
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public IEnumerator GetEnumerator()
        {
			return value.GetEnumerator();
        }
    }
}
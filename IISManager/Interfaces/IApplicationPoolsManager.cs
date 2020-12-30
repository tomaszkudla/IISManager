using IISManager.Implementations;
using System.Collections.ObjectModel;

namespace IISManager.Interfaces
{
    public interface IApplicationPoolsManager
    {
        ObservableCollection<ApplicationPool> ApplicationPools { get; }
        Observable<bool> AllSelected { get; }
        void Refresh();
        void Select(string name);
        void Unselect(string name);
        void SelectAll();
        void UnselectAll();
        void StartSelected();
        void StopSelected();
        void RecycleSelected();
    }
}

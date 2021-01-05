using IISManager.Implementations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IISManager.Interfaces
{
    public interface IApplicationPoolsManager
    {
        ObservableList<ApplicationPool> ApplicationPools { get; }
        Observable<bool> AllSelected { get; }
        Observable<SortingType> Sorting { get; }
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

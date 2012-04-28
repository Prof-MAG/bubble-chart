using System.Collections.ObjectModel;
using System.Linq;

namespace BubbleChart.ViewModels
{
    public class MainPageModel : ViewModel
    {
        private int _reportingYear;

        public MainPageModel()
        {
            Data = new ObservableCollection<DataItemModel>();
            DataFiltered = new ObservableCollection<DataItemModel>();
            RefreshDataFiltered();
        }

        private void RefreshDataFiltered()
        {
            DataFiltered.Clear();
            foreach (var dataItem in Data.Where(item => item.ReportingYear == ReportingYear))
                DataFiltered.Add(dataItem);
        }

        public ObservableCollection<DataItemModel> Data { get; private set; }
        public ObservableCollection<DataItemModel> DataFiltered { get; private set; }

        public int ReportingYear
        {
            get { return _reportingYear; }
            set
            {
                if(SetValue(ref _reportingYear, value, "ReportingYear"))
                {
                    RefreshDataFiltered();
                }
            }
        }
    }
}

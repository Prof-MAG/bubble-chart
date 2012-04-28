using System.Collections.ObjectModel;
using System.Linq;

namespace BubbleChart.ViewModels
{
    public class MainPageModel : ViewModel
    {
        private int _reportingYear;

        public MainPageModel()
        {
            Data = new ObservableCollection<DataItemModel>()
            {
                new DataItemModel(2000, "Donetsk", 12450, 22435, 123),
                new DataItemModel(2000, "Kyiv", 12354, 22432, 124),
                new DataItemModel(2000, "Dnepr", 12353, 22491, 125),
                new DataItemModel(2000, "Lviv", 12315, 22603, 120),

                new DataItemModel(2001, "Donetsk", 12355, 22431, 123),
                new DataItemModel(2001, "Kyiv", 12350, 22433, 125),
                new DataItemModel(2001, "Dnepr", 12358, 22494, 121),
                new DataItemModel(2001, "Lviv", 12311, 22610, 127)
            };
            DataFiltered = new ObservableCollection<DataItemModel>();
            ReportingYear = 2000;
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

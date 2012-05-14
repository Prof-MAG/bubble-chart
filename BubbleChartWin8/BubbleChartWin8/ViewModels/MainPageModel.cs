using System.Collections.ObjectModel;
using System.Linq;

namespace BubbleChartWin8.ViewModels
{
    public class MainPageModel : ViewModel
    {
        private int _reportingYear;

        public MainPageModel()
        {
            Data = new ObservableCollection<DataItemModel>
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
            var dataForPeriod = Data.Where(item => item.ReportingYear == ReportingYear).ToList();
            // remove regions that are not present in the current period
            var redundantItems = DataFiltered.Where(item => dataForPeriod.All(periodItem => periodItem.Region != item.Region)).ToList();
            foreach(var dataItem in redundantItems)
                DataFiltered.Remove(dataItem);
            // update or add new regions
            foreach (var periodItem in Data.Where(item => item.ReportingYear == ReportingYear))
            {
                var filteredItem = DataFiltered.FirstOrDefault(item => item.Region == periodItem.Region);
                if (filteredItem == null)
                {
                    filteredItem = new DataItemModel(ReportingYear, periodItem.Region);
                    DataFiltered.Add(filteredItem);
                }
                filteredItem.MiddleAge = periodItem.MiddleAge;
                filteredItem.Population = periodItem.Population;
                filteredItem.Profit = periodItem.Profit;
            }
        }

        public ObservableCollection<DataItemModel> Data { get; private set; }
        public ObservableCollection<DataItemModel> DataFiltered { get; private set; }

        public int ReportingYear
        {
            get { return _reportingYear; }
            set
            {
                if (SetValue(ref _reportingYear, value, "ReportingYear"))
                {
                    RefreshDataFiltered();
                }
            }
        }
    }
}

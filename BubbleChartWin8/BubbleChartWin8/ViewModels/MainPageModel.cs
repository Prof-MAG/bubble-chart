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
                           new DataItemModel(2005, "Донецк", 1028.2, 1008, 330.3),
                           new DataItemModel(2005, "Мариуполь", 493.6, 1307, 178.5),
                           new DataItemModel(2005, "Макеевка", 385.9, 872, 83.3),
                           new DataItemModel(2005, "Горловка", 291.4, 798, 66.1),
                           new DataItemModel(2005, "Краматорск", 183.6, 962, 55.6),
                           new DataItemModel(2005, "Словянск", 125.2, 727, 33.1),
                           new DataItemModel(2005, "Енакиево", 103, 814, 38.5),
                           new DataItemModel(2005, "Константиновка", 93.2, 627, 17),
                           new DataItemModel(2005, "Артемовск", 83.5, 873, 31.8),
                           new DataItemModel(2005, "Красноармейск", 68.1, 1485, 30.8),
                           new DataItemModel(2006, "Донецк", 993.5, 1275, 339.8),
                           new DataItemModel(2006, "Мариуполь", 480, 1539, 178.6),
                           new DataItemModel(2006, "Макеевка", 372.8, 1034, 84),
                           new DataItemModel(2006, "Горловка", 275.5, 1019, 65.7),
                           new DataItemModel(2006, "Краматорск", 173.3, 1280, 55.6),
                           new DataItemModel(2006, "Словянск", 121.4, 917, 32.9),
                           new DataItemModel(2006, "Енакиево", 94.4, 1006, 37),
                           new DataItemModel(2006, "Константиновка", 86.8, 799, 17.6),
                           new DataItemModel(2006, "Артемовск", 80.2, 1108, 32.2),
                           new DataItemModel(2006, "Красноармейск", 66.9, 1854, 30.9),
                           new DataItemModel(2007, "Донецк", 984.05, 1666, 349.3),
                           new DataItemModel(2007, "Мариуполь", 475.95, 1837, 177.6),
                           new DataItemModel(2007, "Макеевка", 368.25, 1459, 85),
                           new DataItemModel(2007, "Горловка", 270.85, 1350, 67.5),
                           new DataItemModel(2007, "Краматорск", 171.2, 1591, 56.6),
                           new DataItemModel(2007, "Словянск", 120.8, 1208, 33),
                           new DataItemModel(2007, "Енакиево", 91.6, 1369, 37.3),
                           new DataItemModel(2007, "Константиновка", 84.1, 1051, 17),
                           new DataItemModel(2007, "Артемовск", 79.75, 1376, 32.8),
                           new DataItemModel(2007, "Красноармейск", 66.4, 2191, 30.5),
                           new DataItemModel(2008, "Донецк", 974.6, 2206, 353.6),
                           new DataItemModel(2008, "Мариуполь", 471.9, 2180, 173.3),
                           new DataItemModel(2008, "Макеевка", 363.7, 1989, 86.3),
                           new DataItemModel(2008, "Горловка", 266.2, 1768, 68),
                           new DataItemModel(2008, "Краматорск", 169.1, 1967, 85.5),
                           new DataItemModel(2008, "Словянск", 120.2, 1597, 33),
                           new DataItemModel(2008, "Енакиево", 88.8, 1809, 36),
                           new DataItemModel(2008, "Константиновка", 81.4, 1433, 16.8),
                           new DataItemModel(2008, "Артемовск", 79.3, 1785, 31.6),
                           new DataItemModel(2008, "Красноармейск", 65.9, 3039, 29.8),
                           new DataItemModel(2009, "Донецк", 968.3, 2340, 317),
                           new DataItemModel(2009, "Мариуполь", 469.3, 2122, 154.8),
                           new DataItemModel(2009, "Макеевка", 361, 2196, 80.4),
                           new DataItemModel(2009, "Горловка", 263.7, 1762, 61.8),
                           new DataItemModel(2009, "Краматорск", 167.8, 1877, 51.9),
                           new DataItemModel(2009, "Словянск", 119.5, 1715, 30.2),
                           new DataItemModel(2009, "Енакиево", 87.3, 1999, 33.3),
                           new DataItemModel(2009, "Константиновка", 80.4, 1498, 15),
                           new DataItemModel(2009, "Артемовск", 78.8, 2024, 27.5),
                           new DataItemModel(2009, "Красноармейск", 65.8, 3096, 28.7),
                           new DataItemModel(2010, "Донецк", 962, 2730, 341),
                           new DataItemModel(2010, "Мариуполь", 466.6, 2597, 155.7),
                           new DataItemModel(2010, "Макеевка", 358.2, 2554, 80.6),
                           new DataItemModel(2010, "Горловка", 261.1, 2274, 58.4),
                           new DataItemModel(2010, "Краматорск", 166.3, 2348, 48.8),
                           new DataItemModel(2010, "Словянск", 118.6, 2076, 30.2),
                           new DataItemModel(2010, "Енакиево", 85.7, 2441, 32.5),
                           new DataItemModel(2010, "Константиновка", 79.2, 1868, 15.7),
                           new DataItemModel(2010, "Артемовск", 78.2, 2427, 26.9),
                           new DataItemModel(2010, "Красноармейск", 65.4, 3738, 28.9)
                       };
            DataFiltered = new ObservableCollection<DataItemModel>();
            ReportingYear = 2005;
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
                filteredItem.WorkingPopulation = periodItem.WorkingPopulation;
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

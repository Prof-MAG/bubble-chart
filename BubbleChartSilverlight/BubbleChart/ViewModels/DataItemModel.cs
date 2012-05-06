namespace BubbleChart.ViewModels
{
    public class DataItemModel : ViewModel
    {
        private double _middleAge;
        private double _population;
        private double _profit;
        private string _region;
        private int _reportingYear;

        public DataItemModel(int reportingYear, string region, double population, double profit, double middleAge)
        {
            ReportingYear = reportingYear;
            Region = region;
            Population = population;
            Profit = profit;
            MiddleAge = middleAge;
        }

        public DataItemModel(int reportingYear, string region)
        {
            ReportingYear = reportingYear;
            Region = region;
        }

        public double MiddleAge
        {
            get { return _middleAge; }
            set
            {
                _middleAge = value;
                RaisePropertyChanged("MiddleAge");
            }
        }

        public double Population
        {
            get { return _population; }
            set
            {
                _population = value;
                RaisePropertyChanged("Population");
            }
        }

        public double Profit
        {
            get { return _profit; }
            set
            {
                _profit = value;
                RaisePropertyChanged("Profit");
            }
        }

        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                RaisePropertyChanged("Region");
            }
        }

        public int ReportingYear
        {
            get { return _reportingYear; }
            set
            {
                _reportingYear = value;
                RaisePropertyChanged("ReportingYear");
            }
        }
    }
}
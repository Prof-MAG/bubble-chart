namespace BubbleChartWin8.ViewModels
{
    public class DataItemModel : ViewModel
    {
        private double _workingPopulation;
        private double _population;
        private double _profit;
        private string _region;
        private int _reportingYear;

        public DataItemModel(int reportingYear, string region, double population, double profit, double workingPopulation)
        {
            ReportingYear = reportingYear;
            Region = region;
            Population = population;
            Profit = profit;
            WorkingPopulation = workingPopulation;
        }

        public DataItemModel(int reportingYear, string region)
        {
            ReportingYear = reportingYear;
            Region = region;
        }

        public double WorkingPopulation
        {
            get { return _workingPopulation; }
            set
            {
                _workingPopulation = value;
                RaisePropertyChanged("WorkingPopulation");
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

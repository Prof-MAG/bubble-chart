namespace BubbleChart.ViewModels
{
    public class DataItemModel
    {
        public DataItemModel(int reportingYear, string region, double population, double profit, double middleAge)
        {
            ReportingYear = reportingYear;
            Region = region;
            Population = population;
            Profit = profit;
            MiddleAge = middleAge;
        }

        public int ReportingYear { get; set; }
        public string Region { get; set; }
        public double Population { get; set; }
        public double Profit { get; set; }
        public double MiddleAge { get; set; }
    }
}
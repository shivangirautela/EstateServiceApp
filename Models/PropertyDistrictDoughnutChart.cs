using System.Collections.Generic;

namespace EstateServiceApp.Models
{

    public class PropertyDistrictDoughnutChart
    {
        public PropertyDistrictDoughnutChart()
        {
            labels = new List<string>();
            datasets = new List<DoughnutChartChildVM>();
        }
        public List<string> labels { get; set; }
        public List<DoughnutChartChildVM> datasets { get; set; }
    }

    public class DoughnutChartChildVM
    {
        public DoughnutChartChildVM()
        {
            backgroundColor = new List<string>();
            data = new List<int>();
        }
        public List<string> backgroundColor { get; set; }
        public List<int> data { get; set; }
        public int hoverOffset { get; set; }
        public string label { get; set; }
    }

}
using System.Collections.Generic;

namespace EstateServiceApp.Models
{
    public class PropertyCityLineChart
    {
        public PropertyCityLineChart()
        {
            labels = new List<string>();
            datasets = new List<LineChartChildVM>();
        }
        public List<string> labels { get; set; }
        public List<LineChartChildVM> datasets { get; set; }
    }

    public class LineChartChildVM
    {
        public LineChartChildVM()
        {
            data = new List<int>();
        }
        public string label { get; set; }
        public List<int> data { get; set; }
        public string borderColor { get; set; }
        public bool fill { get; set; } = false;
       
    }
}
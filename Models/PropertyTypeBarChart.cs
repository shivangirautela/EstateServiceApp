using System.Collections.Generic;

namespace EstateServiceApp.Models
{
    public class PropertyTypeBarChart
    {
        public PropertyTypeBarChart()
        {
            labels = new List<string>();
            datasets = new List<BarChartChildVM>();
        }
        public List<string> labels { get; set; }
        public List<BarChartChildVM> datasets { get; set; }
    }

    public class BarChartChildVM
    {
        public BarChartChildVM()
        {
            backgroundColor = new List<string>();
            data = new List<int>();
        }
        public List<string> backgroundColor { get; set; }
        public List<int> data { get; set; }
        public int borderWidth { get; set; }
        public List<string> borderColor { get; set; }
        public string label { get; set; }
    }
}
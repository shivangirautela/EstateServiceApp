using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Models
{
    public class AdminStatisticsViewModel
    {
        public PropertyStatusPieChart PropertyStatusPieChartData { get; set; }
        public PropertyTypeBarChart PropertyTypeBarChartData { get; set; }
        public PropertyCityLineChart PropertyCityLineChartData { get; set; }
        public PropertyDistrictDoughnutChart PropertyDistrictDoughnutChartData { get; set; }
    }
}

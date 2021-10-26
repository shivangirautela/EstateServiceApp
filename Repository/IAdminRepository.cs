using EstateServiceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateServiceApp.Repository
{
    public interface IAdminRepository
    {
        Task<List<PropertyModel>> GetAllProperties();
        Task<List<UserActivity>> GetAllUserActivities();
        Task<AdminStatisticsViewModel> PieChartData();
        int GetTotalPhotosCount();
        decimal GetMinPropertyPrice();
        decimal GetMaxPropertyPrice();
        decimal GetTotalBedRooms();
        decimal GetTotalBathRooms();
        decimal PropertiesHavingPool();
        decimal PropertiesHavingParking();
        bool DeleteActivity(int id);
    }
}
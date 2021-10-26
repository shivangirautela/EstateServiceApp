using EstateServiceApp.Data;
using EstateServiceApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly PropertyDbContext _context;
        private readonly IPropertyRepository _propertyRepository;

        public AdminRepository(PropertyDbContext propertyDbContext, IPropertyRepository propertyRepository)
        {
            _context = propertyDbContext;
            _propertyRepository = propertyRepository;
        }

        public async Task<List<UserActivity>> GetAllUserActivities()
        {
            return await _context.UserActivites.ToListAsync();
        }
        public async Task<List<PropertyModel>> GetAllProperties()
        {
            return await _propertyRepository.GetProperties();
        }

        public int GetTotalPhotosCount()
        {
            var count =  _context.Properties.Sum(x => x.TotalPhotos);
            return count;       
        }
        public decimal GetMinPropertyPrice()
        {
            return _context.Properties.Min(x => x.Price); 
        }
        public decimal GetMaxPropertyPrice()
        {
            return _context.Properties.Max(x => x.Price);
        }
        public decimal GetTotalBedRooms()
        {
            return _context.Properties.Sum(x => x.BedRooms);
        }
        public decimal GetTotalBathRooms()
        {
            return _context.Properties.Sum(x => x.BathRooms);
        }
        public decimal PropertiesHavingPool()
        {
            return _context.Properties.Where(x => x.HasPool == true).ToList().Count;
        }
        public decimal PropertiesHavingParking()
        {
            return _context.Properties.Where(x => x.HasParking == true).ToList().Count;
        }
        public bool DeleteActivity(int id)
        {
            var activity = _context.UserActivites.Where(x => x.Id == id).FirstOrDefault();
            _context.UserActivites.Remove(activity);
            return _context.SaveChanges() > 0 ? true : false;
        }
        public async Task<AdminStatisticsViewModel> PieChartData()
        {
            var rent = (ICollection<Property>)await _context.Properties.Where(x => x.PropertyStatus == PropertyStatusEnum.AvailableForRent).ToListAsync();
            var rentCount = rent.Count;
            var sale = (ICollection<Property>)await _context.Properties.Where(x => x.PropertyStatus == PropertyStatusEnum.AvailableForSale).ToListAsync();
            var saleCount = sale.Count;
            var rented = (ICollection<Property>)await _context.Properties.Where(x => x.PropertyStatus == PropertyStatusEnum.Rented).ToListAsync();
            var rentedCount = rented.Count;
            var sold = (ICollection<Property>)await _context.Properties.Where(x => x.PropertyStatus == PropertyStatusEnum.Sold).ToListAsync();
            var soldCount = sold.Count;
            var construction = (ICollection<Property>)await _context.Properties.Where(x => x.PropertyStatus == PropertyStatusEnum.UnderConstruction).ToListAsync();
            var constructionCount = construction.Count;
            var renovation = (ICollection<Property>)await _context.Properties.Where(x => x.PropertyStatus == PropertyStatusEnum.UnderRenovation).ToListAsync();
            var renovationCount = renovation.Count;

            var value1 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.Duplex).ToListAsync();
            var count1 = value1.Count;
            var value2 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.StudioApartment).ToListAsync();
            var count2 = value2.Count;
            var value3 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.ShopShowroom).ToListAsync();
            var count3 = value3.Count;
            var value4 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.Office).ToListAsync();
            var count4 = value4.Count;
            var value5 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.IndependentHouse).ToListAsync();
            var count5 = value5.Count;
            var value6 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.Flat).ToListAsync();
            var count6 = value6.Count;
            var value7 = (ICollection<Property>)await _context.Properties.Where(x => x.Type == PropertyType.Villa).ToListAsync();
            var count7 = value7.Count;
            
            var distinctCities = await _context.Properties.Select(x => x.City.ToUpper()).Distinct().ToListAsync();
            var countCities = new List<int>();
            var nameCities = new List<string>();
            foreach(var city in distinctCities)
            {
                nameCities.Add(city);
                var value = (ICollection<Property>)await _context.Properties.Where(x => x.City.ToLower() == city.ToLower()).ToListAsync();
                countCities.Add(value.Count);
            }

            var distinctDistricts = await _context.Properties.Select(x => x.District.ToUpper()).Distinct().ToListAsync();
            var countDistricts = new List<int>();
            var nameDistricts = new List<string>();
            var colors = new List<string>();
            Random generator = new Random();
            
            foreach (var district in distinctDistricts)
            {
                nameDistricts.Add(district);
                var value = (ICollection<Property>)await _context.Properties.Where(x => x.District.ToLower() == district.ToLower()).ToListAsync();
                countDistricts.Add(value.Count);
                colors.Add("#" + generator.Next(0, 1000000).ToString("D6"));
            }

            var statistics = new AdminStatisticsViewModel()
            {
                PropertyStatusPieChartData = new PropertyStatusPieChart()
                {
                    datasets = new List<PieChartChildVM>()
                    {
                        new PieChartChildVM()
                        {
                            data = new List<int>() { rentCount, saleCount, rentedCount, soldCount, constructionCount, renovationCount },
                            backgroundColor = new List<string>() { "#00ff21", "#ff0000", "#fef91a", "#0094ff", "#f57e2a", "#a20bf9" }
                        },
                    },
                    labels = new List<string>() { "Available For Rent", "Available For Sale", "Rented", "Sold", "Under Construction", "Under Renovation" }
                },

                PropertyTypeBarChartData = new PropertyTypeBarChart()
                {
                    datasets = new List<BarChartChildVM>()
                    {
                        new BarChartChildVM()
                        {
                            data = new List<int>() { count1, count2, count3, count4, count5, count6, count7 },
                            backgroundColor = new List<string>() { "#00ff21", "#ff0000", "#fef91a", "#0094ff", "#f57e2a", "#a20bf9", "#ff0000" },
                            borderColor = new List<string>() { "#a20bf9", "#00ff21", "#ff0000", "#fef91a", "#0094ff", "#f57e2a", "#a20bf9" },
                            borderWidth = 3,
                            label = "Number of properties"
                        }
                    },
                    labels = new List<string>() { "Duplex", "Studio Apartment", "Shop Showroom", "Office", "Independent House", "Flat", "Villa" }
                },
                PropertyCityLineChartData = new PropertyCityLineChart()
                {
                    datasets = new List<LineChartChildVM>()
                    {
                        new LineChartChildVM()
                        {
                            data = countCities,
                            borderColor = "rgb(75, 192, 192)",
                            label = "Properties in City"
                        }
                    },
                    labels = nameCities
                },
                PropertyDistrictDoughnutChartData = new PropertyDistrictDoughnutChart()
                {
                    datasets = new List<DoughnutChartChildVM>()
                    {
                        new DoughnutChartChildVM()
                        {
                            hoverOffset = 20,
                            backgroundColor = colors,
                            data = countDistricts,
                            label = "districts available"
                        }
                    },
                    labels = nameDistricts
                }
            };
            return statistics;
        }
    }
}

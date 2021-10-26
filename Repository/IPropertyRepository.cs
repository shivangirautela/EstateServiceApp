using EstateServiceApp.Data;
using EstateServiceApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateServiceApp.Repository
{
    /// <summary>
    /// This interface helps in declaring functions for property repository
    /// </summary>
    public interface IPropertyRepository
    {
        Task<int> AddNewProperty(PropertyModel model);
        Task<List<PropertyModel>> GetProperties();
        Task<PropertyModel> GetPropertyById(int id);
        IList<SelectListItem> GetFeatures();
        string GetFeatureText(int id);
        Task<List<PropertyModel>> GetTop(int id, PropertyStatusEnum propertyStatus, string address, int minBudget, int maxBudget);
        string SaveAllBrochures(int id);
        void AddPdfPathToDb(string pdfPath, int id);
        string GetBrochurePathById(int id);
     }
}
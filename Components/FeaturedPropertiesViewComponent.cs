using EstateServiceApp.Data;
using EstateServiceApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Components
{
    /// <summary>
    /// This view component was created to analyse the basic properties all over the application
    /// </summary>
    public class FeaturedPropertiesViewComponent:ViewComponent
    {
        private readonly IPropertyRepository _propertyRepository;
        public FeaturedPropertiesViewComponent(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        /// <summary>
        /// This function is used to search for properties based on specified details by customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <param name="minBudget"></param>
        /// <param name="maxBudget"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int id,PropertyStatusEnum type,string address, int minBudget, int maxBudget)
        {
            var properties = await _propertyRepository.GetTop(id, type, address, minBudget, maxBudget);
            return View(properties);
        }
    }
}

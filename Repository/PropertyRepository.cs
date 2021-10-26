using EstateServiceApp.Data;
using EstateServiceApp.Models;
using EstateServiceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using Microsoft.AspNetCore.Hosting;

namespace EstateServiceApp.Repository
{
    /// <summary>
    /// This repository helps in managing property db CRUD operations
    /// </summary>
    public class PropertyRepository : IPropertyRepository
    {
        private readonly PropertyDbContext _context = null;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// This constructor provides dependency injection of services related to property db context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <param name="webHostEnvironment"></param>
        public PropertyRepository(PropertyDbContext context, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// This method gets name of facilities offered
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFeatureText(int id)
        {
            return _context.Features.Where(x => x.Id == id).FirstOrDefault().Name;
        }

        /// <summary>
        /// This method gets all features using select list items
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetFeatures()
        {
            return _context.Features.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        }
        /// <summary>
        /// This method gets the path of brochure from database by property id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetBrochurePathById(int id)
        {
            return _context.Properties.Where(x => x.Id == id).FirstOrDefault().PdfPath;
        }

        /// <summary>
        /// This method saves the brochure details of each property into database by their id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string SaveAllBrochures(int id)
        {
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            //Set the BlinkBinaries folder path
            blinkConverterSettings.BlinkPath = @"C:\Users\shivangi Rautela\.nuget\packages\syncfusion.htmltopdfconverter.blink.net.core.windows\19.3.0.43\lib\BlinkBinariesWindows";

            //Assign Blink converter settings to HTML converter
            htmlConverter.ConverterSettings = blinkConverterSettings;

            PdfDocument document = htmlConverter.Convert("https://localhost:44361/Property/GetProperty/" + id);
            string path = Path.Combine(_webHostEnvironment.WebRootPath +"\\brochures", Guid.NewGuid().ToString() + "_" + id + ".pdf");
            FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
            

            //Save and close the PDF document 
            document.Save(fileStream);
            fileStream.Close();
            document.Close(true);
            return path;

        }
        /// <summary>
        /// This method adds the pdf path to the database
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <param name="id"></param>
        public void AddPdfPathToDb(string pdfPath, int id)
        {
            Property property = _context.Properties.Where(x => x.Id == id).FirstOrDefault();
            property.PdfPath = pdfPath;
            _context.SaveChanges();
        }
        /// <summary>
        /// This method adds new property posted by clients
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> AddNewProperty(PropertyModel model)
        {
            var newProperty = new Property()
            {
                Type = model.Type,
                Address = model.Address,
                AvailableFrom = model.AvailableFrom,
                PostedOn = DateTime.Today,
                Price = model.Price,
                Title = model.Title,
                CoverImageUrl = model.CoverImageUrl,
                PropertyOwnerId = _userService.GetUserId(),
                PropertyStatus = model.PropertyStatus,
                Description = model.Description,
                BedRooms = model.BedRooms,
                BathRooms = model.BathRooms,
                HasParking = false,
                HasPool = false,
                PropertyArea = model.PropertyArea,
                District=model.District,
                City=model.City
                
            };
            if (model.Features != null)
            {
                var f = model.Features.Where(x => x.Selected).Select(y => y.Value);
                foreach (var i in f)
                {
                    var fg = GetFeatureText(int.Parse(i));
                    if (fg == "HasPool")
                    {
                        newProperty.HasPool = true;
                    }
                    if (fg == "HasParking")
                    {
                        newProperty.HasParking = true;
                    }
                }
            }
            newProperty.propertyGallery = new List<PropertyGallery>();
            foreach (var file in model.Gallery)
            {
                newProperty.propertyGallery.Add(new PropertyGallery()
                {
                    Name = file.Name,
                    Url = file.Url
                });
            }
            newProperty.TotalPhotos = newProperty.propertyGallery.Count;
            await _context.Properties.AddAsync(newProperty);
            await _context.SaveChangesAsync();

            return newProperty.Id;
        }
        /// <summary>
        /// This method gets all properties visible to our customers
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyModel>> GetProperties()
        {
            var properties = await _context.Properties.ToListAsync();
            var allProperties = new List<PropertyModel>();

            if (properties?.Any() == true)
            {
                foreach (var item in properties)
                {
                    var date = item.PostedOn.Value.Date;

                    allProperties.Add(new PropertyModel()
                    {
                        Address = item.Address,
                        AvailableFrom = item.AvailableFrom,
                        Price = item.Price,
                        Title = item.Title,
                        Type = item.Type,
                        Id = item.Id,
                        CoverImageUrl = item.CoverImageUrl,
                        PostedOn = date,
                        TotalPhotos = item.TotalPhotos,
                        PropertyOwnerId = item.PropertyOwnerId,
                        PropertyStatus = item.PropertyStatus,
                        Description = item.Description,
                        BedRooms = item.BedRooms,
                        BathRooms = item.BathRooms,
                        PropertyArea = item.PropertyArea,
                        HasParking = item.HasParking,
                        HasPool = item.HasPool,
                        District = item.District,
                        City = item.City,                        
                        OwnerPhoneNumber= _context.Users.Where(x => x.Id == item.PropertyOwnerId).FirstOrDefault().PhoneNumber,
                        PropertyPostedBy = _context.Users.Where(x => x.Id == item.PropertyOwnerId).FirstOrDefault().FirstName
                    });

                }
            }
            return allProperties;
        }
        /// <summary>
        /// This method gets specific property details in a new page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PropertyModel> GetPropertyById(int id)
        {
            return await _context.Properties
               .Where(x => x.Id == id)
               .Select(property => new PropertyModel()
               {
                   Address = property.Address,
                   AvailableFrom = property.AvailableFrom,
                   Price = property.Price,
                   Title = property.Title,
                   Type = property.Type,
                   Id = property.Id,
                   PostedOn = (DateTime)property.PostedOn.Value.Date,
                   CoverImageUrl = property.CoverImageUrl,
                   TotalPhotos = property.TotalPhotos,
                   PropertyOwnerId = property.PropertyOwnerId,
                   PropertyStatus = property.PropertyStatus,
                   Description = property.Description,
                   BedRooms = property.BedRooms,
                   BathRooms = property.BathRooms,
                   HasParking = property.HasParking,
                   HasPool = property.HasPool,
                   PropertyArea = property.PropertyArea,
                   PdfPath = property.PdfPath,
                   District=property.District,
                   City=property.City,
                   OwnerPhoneNumber = _context.Users.Where(x => x.Id == property.PropertyOwnerId).FirstOrDefault().PhoneNumber,
                   PropertyPostedBy = _context.Users.Where(x => x.Id == property.PropertyOwnerId).FirstOrDefault().FirstName,
                   Gallery = property.propertyGallery.Select(g => new GalleryModel()
                   {
                       Id = g.Id,
                       Name = g.Name,
                       Url = g.Url
                   }).ToList()
               }).FirstOrDefaultAsync();
        }
        /// <summary>
        /// This method gives searched properties by customers based on given filters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyStatus"></param>
        /// <param name="address"></param>
        /// <param name="minBudget"></param>
        /// <param name="maxBudget"></param>
        /// <returns></returns>
        public async Task<List<PropertyModel>> GetTop(int id, PropertyStatusEnum propertyStatus, string address, int minBudget, int maxBudget)
        {
            return await _context.Properties.Where(x => x.Id == id || x.PropertyStatus == propertyStatus
            || x.Address.Contains(address) || (x.Price >= minBudget && x.Price <= maxBudget))
                .Select(property => new PropertyModel()
                {
                    Address = property.Address,
                    AvailableFrom = property.AvailableFrom,
                    Price = property.Price,
                    Title = property.Title,
                    Type = property.Type,
                    Id = property.Id,
                    PostedOn = (DateTime)property.PostedOn.Value.Date,
                    CoverImageUrl = property.CoverImageUrl,
                    TotalPhotos = property.TotalPhotos,
                    PropertyOwnerId = property.PropertyOwnerId,
                    PropertyStatus = property.PropertyStatus,
                    Description = property.Description,
                    BedRooms = property.BedRooms,
                    BathRooms = property.BathRooms,
                    HasParking = property.HasParking,
                    HasPool = property.HasPool,
                    PropertyArea = property.PropertyArea,
                    District = property.District,
                    City = property.City,
                    OwnerPhoneNumber = _context.Users.Where(x => x.Id == property.PropertyOwnerId).FirstOrDefault().PhoneNumber,
                    PropertyPostedBy = _context.Users.Where(x => x.Id == property.PropertyOwnerId).FirstOrDefault().FirstName
                }).ToListAsync();
        }
    }
}

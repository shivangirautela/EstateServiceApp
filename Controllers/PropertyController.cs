using EstateServiceApp.Models;
using EstateServiceApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using EstateServiceApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EstateServiceApp.Controllers
{
    public class PropertyController:BaseController<PropertyModel>
    {
        private readonly IPropertyRepository _propertyRepository=null;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public PropertyController(IPropertyRepository propertyRepository,IWebHostEnvironment webHostEnvironment, IHubContext<NotificationHub> notificationHubContext)
        {
            _propertyRepository = propertyRepository;
            _hostingEnvironment = webHostEnvironment;
            _notificationHubContext = notificationHubContext;
        }
        /// <summary>
        /// This get method is used for adding new property to the database 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="propertyId"></param>
        /// <param name="images"></param>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult AddNewProperty(bool isSuccess =false,int propertyId=0,List<string> images=null,string pdfPath=null)
        {
            var categoryList = _propertyRepository.GetFeatures();
            ViewBag.F = categoryList;
            var allFeatures = new PropertyModel()
            {
                Features = categoryList
            };
            ViewBag.IsSuccess = isSuccess;
            ViewBag.PropertyId = propertyId;
            ViewBag.Images = images;
            if (propertyId != 0 && pdfPath == null)
            {
                var pdfPathdb = _propertyRepository.SaveAllBrochures(propertyId);
                if (pdfPathdb != null)
                {
                    _propertyRepository.AddPdfPathToDb(pdfPathdb, propertyId);
                    return RedirectToAction(nameof(AddNewProperty), new { isSuccess = true, propertyId = propertyId, images = images, pdfPath = pdfPathdb });
                }
            }       
            return View(allFeatures);
        }
        /// <summary>
        /// This method is used to upload images of file types into specified folder
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> UploadImage(string folder, IFormFile file)
        {
            folder += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_hostingEnvironment.WebRootPath, folder);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folder;
        }
        /// <summary>
        /// This post method helps in saving property details to database 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddNewProperty(PropertyModel model)
        {
            if (ModelState.IsValid)
            {
                List<string> images = new List<string>();
                if (model.CoverPhoto != null)
                {
                    string folder = "properties/cover/";
                    model.CoverImageUrl = await UploadImage(folder, model.CoverPhoto);
                    images.Add(model.CoverImageUrl);
                }
                if (model.GalleryFiles != null)
                {
                    string folder = "properties/gallery/";
                    model.Gallery = new List<GalleryModel>();

                    foreach (var file in model.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = file.FileName,
                            Url = await UploadImage(folder,file)
                        };
                        images.Add(gallery.Url);
                        model.Gallery.Add(gallery);
                    }
                }
                
                int id = await _propertyRepository.AddNewProperty(model);
                if (id > 0)
                    return RedirectToAction(nameof(AddNewProperty), new { isSuccess = true, propertyId = id, images = images});

            }
            ViewBag.IsSuccess = false;
            ViewBag.PropertyId = 0;
 
            return View();
        }
        /// <summary>
        /// This post method is used to download the brochure about each property 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DownloadBrochure(int id)
        {
            var path = _propertyRepository.GetBrochurePathById(id);
            var memory = new MemoryStream(); 
            using (var stream =new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.Read))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(path));
        }
        /// <summary>
        /// This method is used to view all properties posted by users
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAllProperties(int page =1)
        {
            var data = await _propertyRepository.GetProperties();
            var paginatedResult = PaginatedResult(data, page, 5);

            return View(paginatedResult);
        }
        /// <summary>
        ///This method is used to view property details by it's id 
        /// </summary>
        /// <returns></returns>]
        public async Task<IActionResult> GetProperty(int id)
        {
            return View(await _propertyRepository.GetPropertyById(id));
        }

    }
}

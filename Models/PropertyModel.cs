using EstateServiceApp.Data;
using EstateServiceApp.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Models
{
    public class PropertyModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required(ErrorMessage = "Type of property is required.")]
        [Display(Name = "What type of property is it ?")]
        public PropertyType Type { get; set; }

        [Required]
        [Display(Name = "When is your property available from ?")]
        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Display(Name ="Choose one cover photo for your property")]
        [NotMappedAttribute]
        [Required]
        public IFormFile CoverPhoto { get; set; }

        public string CoverImageUrl { get; set; }

        [Display(Name = "Choose (at least minimum 5) gallery images for your property")]  
        [NotMappedAttribute]
        [GalleryValidationAttribute]
        public IFormFileCollection GalleryFiles { get; set; }

        public List<GalleryModel> Gallery { get; set; }

        [Required(ErrorMessage ="Status of property is missing.")]
        [Display(Name = "Is the property available ?")]
        public PropertyStatusEnum PropertyStatus { get; set; }
        public string PropertyOwnerId { get; set; }
        public string PropertyPostedBy { get; set; }
        public string OwnerPhoneNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime PostedOn { get; set; }

        public int TotalPhotos { get; set; }
        public string Description { get; set; }
        [Required]
        public int BedRooms { get; set; }
        [Required]
        public int BathRooms { get; set; }
        [Required]
        public double PropertyArea { get; set; }

        [Display(Name = "Please Select The Facilities Offered.")]
        public IList<SelectListItem> Features { get; set; }
        public bool HasParking { get; set; }
        public bool HasPool { get; set; }
        public string PdfPath { get; set; }

        [Required]
        public string District { get; set; }
        [Required]
        public string City { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Data
{
    /// <summary>
    /// This class contains all entities related to property
    /// </summary>
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public PropertyType Type { get; set; }
        public DateTime AvailableFrom { get; set; }      
        public string Address { get; set; }
        public decimal Price { get; set; }
        public DateTime? PostedOn { get; set; }
        public string CoverImageUrl { get; set; }
        public ICollection<PropertyGallery> propertyGallery { get; set; }
        public PropertyStatusEnum PropertyStatus { get; set; }
        public int TotalPhotos { get; set; }
        public string Description { get; set; }
        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public double PropertyArea { get; set; }
        public bool HasParking { get; set; }
        public bool HasPool { get; set; }
        public string PropertyOwnerId { get; set; }
        public string PdfPath { get; set; }
        public string District { get; set; }
        public string City { get; set; }


    }
}

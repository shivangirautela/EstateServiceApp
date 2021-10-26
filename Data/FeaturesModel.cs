using System.ComponentModel.DataAnnotations;

namespace EstateServiceApp.Data
{
    public class FeaturesModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int propertyId { get; set; }
    }
}
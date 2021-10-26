using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Data
{
    public class PropertyGallery
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Property Property { get; set; }
    }
}

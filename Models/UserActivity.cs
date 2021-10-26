using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Models
{
    public class UserActivity
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public int PropertyId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        
        public DateTime? ActivityDate { get; set; } = DateTime.Now;
    }
}

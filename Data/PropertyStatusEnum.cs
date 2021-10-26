using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Data
{
    public enum PropertyStatusEnum
    {
        [Display(Name = "Select Property Status")]
        SelectStatus,

        Sold,
        [Display(Name = "Available For Sale")]
        AvailableForSale,
        Rented,
        [Display(Name = "Available For Rent")]
        AvailableForRent,
        [Display(Name = "Under Construction")]
        UnderConstruction,
        [Display(Name = "Under Renovation")]
        UnderRenovation
    }
}

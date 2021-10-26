using System.ComponentModel.DataAnnotations;

namespace EstateServiceApp.Data
{
    public enum PropertyType
    {
        Villa,

        Duplex,

        [Display(Name = "Studio Apartment")]
        StudioApartment,

        [Display(Name = "Independent House")]
        IndependentHouse,

        Flat,

        [Display(Name = "Shop / Showroom")]
        ShopShowroom,

        Office,

        SelectType
    }
}
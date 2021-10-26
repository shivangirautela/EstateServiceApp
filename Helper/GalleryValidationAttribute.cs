using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Helper
{
    public class GalleryValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                IFormFileCollection files = (IFormFileCollection)value;
                if(files.Count >= 5)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("At least 5 images have to be uploaded for better responses.");
                }
            }
            return new ValidationResult("No Images selected yet");
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyImageHandler
{
    public class ImageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (((IFormFile)value).IsValidImage(out _))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);

        }
    }
}

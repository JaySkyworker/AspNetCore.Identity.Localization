using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityLocalization.Services
{
    public interface ILocalizedValidationAttributeAdapter
    {
        string GetErrorMessageResourceName(ValidationAttribute attribute);

        bool CanHandle(ValidationAttribute attribute);
    }
}

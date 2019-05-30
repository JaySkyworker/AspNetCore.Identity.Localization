using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityLocalization.Services
{
    public abstract class BaseLocalizedValidationAttributeAdapter<TValidationAttribute> : ILocalizedValidationAttributeAdapter
        where TValidationAttribute : ValidationAttribute
    {
        private static string[] InternalDefaultSupportedAttributes = new string[]
        {
            nameof(CompareAttribute),
            nameof(EmailAddressAttribute),
            nameof(RequiredAttribute),
            nameof(CreditCardAttribute),
            nameof(FileExtensionsAttribute),
            nameof(MaxLengthAttribute),
            nameof(MinLengthAttribute),
            nameof(PhoneAttribute),
            nameof(RangeAttribute),
            nameof(RegularExpressionAttribute),
            nameof(UrlAttribute)
        };

        public virtual bool CanHandle(ValidationAttribute attribute)
        {
            return attribute is TValidationAttribute;
        }

        public virtual string GetErrorMessageResourceName(ValidationAttribute attribute)
        {
            return InternalGetErrorMessageResourceName(attribute as TValidationAttribute);
        }

        protected abstract string InternalGetErrorMessageResourceName(TValidationAttribute attr);
    }
}

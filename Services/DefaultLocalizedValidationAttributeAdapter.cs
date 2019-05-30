using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityLocalization.Services
{
    public class DefaultLocalizedValidationAttributeAdapter : ILocalizedValidationAttributeAdapter
    {
        public IList<string> SupportedAttributes { get; } =
        new string[]
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

    public DefaultLocalizedValidationAttributeAdapter()
        {
            //if (options == null) { throw new ArgumentNullException(nameof(options)); }
            //SupportedAttributes = options.SupportedAttributes;
        }

        public bool CanHandle(ValidationAttribute attribute)
        {
            return SupportedAttributes.Contains(attribute.GetType().Name);
        }

        public string GetErrorMessageResourceName(ValidationAttribute attribute)
        {
            return $"{attribute.GetType().Name}_ErrorMessage";
        }
    }
}

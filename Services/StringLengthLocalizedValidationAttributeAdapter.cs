using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityLocalization.Services
{
    public class StringLengthLocalizedValidationAttributeAdapter : BaseLocalizedValidationAttributeAdapter<StringLengthAttribute>
    {
        public const string DefaultResourceName = "StringLengthAttribute_ErrorMessage";
        public const string ResourceNameIncludingMinimum = "StringLengthAttribute_ErrorMessageIncludingMinimum";

        protected override string InternalGetErrorMessageResourceName(StringLengthAttribute attr)
        {
            if (attr.MinimumLength > 0)
            {
                return ResourceNameIncludingMinimum;
            }
            else
            {
                return DefaultResourceName;
            }
        }
    }
}

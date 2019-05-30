using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityLocalization.Services
{
    public interface ILocalizedValidationMetadataProvider : IValidationMetadataProvider
    {
        IList<ILocalizedValidationAttributeAdapter> Adapters { get; }
    }
}

using AspNetCoreIdentityLocalization.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

public class LocalizedValidationMetadataProvider<TErrorMessageResource> : ILocalizedValidationMetadataProvider
{
    public Type ErrorMessageResourceType { get; }
    public IList<ILocalizedValidationAttributeAdapter> Adapters { get; }

    public LocalizedValidationMetadataProvider(params ILocalizedValidationAttributeAdapter[] adapters)
    {
        Adapters = new List<ILocalizedValidationAttributeAdapter>(adapters ?? Enumerable.Empty<ILocalizedValidationAttributeAdapter>());
        ErrorMessageResourceType = typeof(TErrorMessageResource);
    }

    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        foreach (var attr in context.Attributes)
        {
            if (!(attr is ValidationAttribute validationAttr)) continue;

            // Do nothing if custom error message or localization options are specified
            if (!(string.IsNullOrWhiteSpace(validationAttr.ErrorMessage) || attr is DataTypeAttribute)) continue;
            if (!string.IsNullOrWhiteSpace(validationAttr.ErrorMessageResourceName) && validationAttr.ErrorMessageResourceType != null) continue;

            foreach (var adapter in Adapters)
            {
                if (adapter.CanHandle(validationAttr))
                {
                    validationAttr.ErrorMessageResourceType = ErrorMessageResourceType;
                    validationAttr.ErrorMessageResourceName = adapter.GetErrorMessageResourceName(validationAttr);
                    validationAttr.ErrorMessage = null;
                    break;
                }
            }

        }
    }
}

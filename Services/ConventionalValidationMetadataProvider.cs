using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspNetCoreIdentityLocalization.Services{
    public class ConventionalValidationMetadataProvider : IValidationMetadataProvider {
        private readonly ResourceManager _resourceManager;
        private readonly Type _resourceType;

        public ConventionalValidationMetadataProvider() : this(typeof(Resources.DefaultValidationMessages)) {
        }

        public ConventionalValidationMetadataProvider(Type resourceType) {
            this._resourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
            this._resourceManager = new ResourceManager(resourceType);
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Add Required attribute to value types to simplify localization
            if (context.Key.ModelType.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(context.Key.ModelType.GetTypeInfo()) == null && !context.ValidationMetadata.ValidatorMetadata.OfType<RequiredAttribute>().Any()) {
                context.ValidationMetadata.ValidatorMetadata.Add(new RequiredAttribute());
            }

            foreach (var attribute in context.ValidationMetadata.ValidatorMetadata) {
                var validationAttribute = attribute as ValidationAttribute;
                if (validationAttribute == null) continue; // Not a validation attribute

                // Do nothing if custom error message or localization options are specified
                if (!(string.IsNullOrWhiteSpace(validationAttribute.ErrorMessage) || attribute is DataTypeAttribute)) continue;
                if (!string.IsNullOrWhiteSpace(validationAttribute.ErrorMessageResourceName) && validationAttribute.ErrorMessageResourceType != null) continue;

                // Get attribute name without the "Attribute" suffix
                var attributeName = validationAttribute.GetType().Name.Replace("Attribute", "");

                var keyName = this._resourceManager.GetConventionalKeyName(context.Key, attributeName, null);

                // Link to resource if exists
                if (keyName != null){
                    validationAttribute.ErrorMessageResourceType = this._resourceType;
                    validationAttribute.ErrorMessageResourceName = keyName;
                    validationAttribute.ErrorMessage = null;
                } else {
                    //validationAttribute.ErrorMessage = $"Missing resource key '{attributeName}'.";
                }

            }
        }
    }
}

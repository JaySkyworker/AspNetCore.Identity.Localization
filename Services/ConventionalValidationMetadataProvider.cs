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

                var keyName = this.GetResourceKeyName(context.Key, attributeName);

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

        private string GetResourceKeyName(ModelMetadataIdentity metadataIdentity, string attributeName)
        {
            if (string.IsNullOrEmpty(metadataIdentity.Name)) return null;

            string fullPropertyName;
            if (!string.IsNullOrEmpty(metadataIdentity.ContainerType?.FullName))
            {
                fullPropertyName = metadataIdentity.ContainerType.FullName + "." + metadataIdentity.Name;
            }
            else
            {
                fullPropertyName = metadataIdentity.Name;
            }

            // Search by name from more specific to less specific
            var resourceKeyName = fullPropertyName.Replace('.', '_').Replace('+', '_') + "_" + attributeName ;
            var namePartsCount = resourceKeyName.Length - resourceKeyName.Replace("_", string.Empty).Length + 1;
            for (var i = 0; i < namePartsCount; i++)
            {
                // Get the resource key to lookup
                if (i > 0) resourceKeyName = resourceKeyName.Substring(resourceKeyName.IndexOf("_") + 1);

                // Check if given value exists in resource
                var keyExists = !(this._resourceManager.GetString(resourceKeyName) == null);
                if (keyExists) return resourceKeyName;
            }

            // Not found
            return null;
        }

    }
}

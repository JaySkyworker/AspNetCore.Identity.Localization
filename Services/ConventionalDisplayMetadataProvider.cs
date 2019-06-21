using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspNetCoreIdentityLocalization.Services{
    public class ConventionalDisplayMetadataProvider : IDisplayMetadataProvider {
        private readonly ResourceManager _resourceManager;
        private readonly Type _resourceType;

        public ConventionalDisplayMetadataProvider(Type resourceType) {
            this._resourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
            this._resourceManager = new ResourceManager(resourceType);
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.UpdateDisplayName(context);

        }

        private void UpdateDisplayName(DisplayMetadataProviderContext context) {
            // Special cases
            if (string.IsNullOrWhiteSpace(context.Key.Name)) return;
            if (!string.IsNullOrWhiteSpace(context.DisplayMetadata.SimpleDisplayProperty)) return;
            if (context.Attributes.OfType<DisplayNameAttribute>().Any(x => !string.IsNullOrWhiteSpace(x.DisplayName))) return;
            if (context.Attributes.OfType<DisplayAttribute>().Any(x => !string.IsNullOrWhiteSpace(x.Name))) return;

            // Try get resource key name
            var keyName = this.GetResourceKeyName(context.Key, null);
            if (keyName != null) context.DisplayMetadata.DisplayName = () => this._resourceManager.GetString(keyName);
        }

        private string GetResourceKeyName(ModelMetadataIdentity metadataIdentity, string resourceKeySuffix) {
            if (string.IsNullOrEmpty(metadataIdentity.Name)) return null;

            if (string.IsNullOrWhiteSpace(resourceKeySuffix))
            {
                resourceKeySuffix = string.Empty;
            }
            else
            {
                    resourceKeySuffix = "_" + resourceKeySuffix;
            }

            string fullPropertyName;
            if (!string.IsNullOrEmpty(metadataIdentity.ContainerType?.FullName)) {
                fullPropertyName = metadataIdentity.ContainerType.FullName + "." + metadataIdentity.Name;
            } else {
                fullPropertyName = metadataIdentity.Name;
            }

            // Search by name from more specific to less specific
            var resourceKeyName = fullPropertyName.Replace('.', '_').Replace('+', '_');
            var namePartsCount = resourceKeyName.Length - resourceKeyName.Replace("_", string.Empty).Length + 1;
            resourceKeyName += resourceKeySuffix;
            for (var i = 0; i < namePartsCount; i++)
            {
                // Get the resource key to lookup
                if (i > 0) resourceKeyName = resourceKeyName.Substring(resourceKeyName.IndexOf("_") + 1);

                // Check if given value exists in resource
                var keyExists = !(this._resourceManager.GetString(resourceKeyName) == null);
                if (keyExists) return resourceKeyName; //{ break; };
            }

            // Not found
            return null;
        }

    }
}

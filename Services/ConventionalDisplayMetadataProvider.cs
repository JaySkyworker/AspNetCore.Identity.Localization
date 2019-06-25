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
            var keyName = this._resourceManager.GetConventionalKeyName(context.Key, null, null);
            if (keyName != null) context.DisplayMetadata.DisplayName = () => this._resourceManager.GetString(keyName);
        }
    }
}

using System;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspNetCoreIdentityLocalization.Services{
    internal static class ResourceManagerExtensions
    {
        public static string GetConventionalKeyName(this ResourceManager resourceManager, ModelMetadataIdentity metadataIdentity, string attributeName, string resourceKeySuffix)
        {
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));

            if (string.IsNullOrEmpty(metadataIdentity.Name)) return null;

            attributeName = string.IsNullOrWhiteSpace(attributeName) ? string.Empty : "_" + attributeName;

            resourceKeySuffix = string.IsNullOrWhiteSpace(resourceKeySuffix) ? string.Empty : "_" + resourceKeySuffix;

            var fullPropertyName = string.IsNullOrEmpty(metadataIdentity.ContainerType?.FullName) ? metadataIdentity.Name 
                : metadataIdentity.ContainerType.FullName + "." + metadataIdentity.Name ;

            var fullKeyName = fullPropertyName.Replace('.', '_').Replace('+', '_') + attributeName;
            var namePartsCount = fullKeyName.Length - fullKeyName.Replace("_", string.Empty).Length + 1;
            fullKeyName += resourceKeySuffix;
            for (var i = 0; i < namePartsCount; i++)
            {
                if (i > 0) {
                    fullKeyName = fullKeyName.Substring(fullKeyName.IndexOf("_") + 1);
                }

                // return KeyName when found
                if (resourceManager.GetString(fullKeyName) != null) {
                    return fullKeyName;
                }

            }

            // Not found
            return null;
        }
    }
}

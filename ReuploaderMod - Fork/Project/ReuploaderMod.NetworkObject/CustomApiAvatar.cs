using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using VRC.Core;

namespace ReuploaderMod.NetworkObject
{
    public class CustomApiAvatar
    {
        private string string_0;

        private int int_0;

        private string string_1;

        private CustomAssetVersion customAssetVersion_0;

        private string string_2;

        private string string_3;

        private DateTime dateTime_0;

        private string string_4;

        private bool bool_0;

        private string string_5;

        private string string_6;

        private string string_7;

        private string string_8;

        private List<string> list_0;

        private string string_9;

        private string string_10;

        private DateTime dateTime_1;

        private int int_1;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get
            {
                return string_0;
            }

            set
            {
                string_0 = value;
            }
        }

        [JsonProperty(PropertyName = "apiVersion")]
        public int ApiVersion
        {
            get
            {
                return int_0;
            }

            set
            {
                int_0 = value;
            }
        }

        [JsonProperty(PropertyName = "assetUrl")]
        public string AssetUrl
        {
            get
            {
                return string_1;
            }

            set
            {
                string_1 = value;
            }
        }

        [JsonProperty(PropertyName = "assetVersion")]
        public CustomAssetVersion AssetVersion
        {
            get
            {
                return customAssetVersion_0;
            }

            set
            {
                customAssetVersion_0 = value;
            }
        }

        [JsonProperty(PropertyName = "authorId")]
        public string AuthorId
        {
            get
            {
                return string_2;
            }

            set
            {
                string_2 = value;
            }
        }

        [JsonProperty(PropertyName = "authorName")]
        public string AuthorName
        {
            get
            {
                return string_3;
            }

            set
            {
                string_3 = value;
            }
        }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime Created
        {
            get
            {
                return dateTime_0;
            }

            set
            {
                dateTime_0 = value;
            }
        }

        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get
            {
                return string_4;
            }

            set
            {
                string_4 = value;
            }
        }

        [JsonProperty(PropertyName = "featured")]
        public bool Featured
        {
            get
            {
                return bool_0;
            }

            set
            {
                bool_0 = value;
            }
        }

        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl
        {
            get
            {
                return string_5;
            }

            set
            {
                string_5 = value;
            }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get
            {
                return string_6;
            }

            set
            {
                string_6 = value;
            }
        }

        [JsonProperty(PropertyName = "platform")]
        public string Platform
        {
            get
            {
                return string_7;
            }

            set
            {
                string_7 = value;
            }
        }

        [JsonProperty(PropertyName = "releaseStatus")]
        public string ReleaseStatus
        {
            get
            {
                return string_8;
            }

            set
            {
                string_8 = value;
            }
        }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags
        {
            get
            {
                return list_0;
            }

            set
            {
                list_0 = value;
            }
        }

        [JsonProperty(PropertyName = "thumbnailImageUrl")]
        public string ThumbnailImageUrl
        {
            get
            {
                return string_9;
            }

            set
            {
                string_9 = value;
            }
        }

        [JsonProperty(PropertyName = "unityPackageUrl")]
        public string UnityPackageUrl
        {
            get
            {
                return string_10;
            }

            set
            {
                string_10 = value;
            }
        }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime Updated
        {
            get
            {
                return dateTime_1;
            }

            set
            {
                dateTime_1 = value;
            }
        }

        [JsonProperty(PropertyName = "version")]
        public int Version
        {
            get
            {
                return int_1;
            }

            set
            {
                int_1 = value;
            }
        }

        public CustomApiAvatar()
        {
        }

        public CustomApiAvatar(ApiAvatar apiAvatar_0)
        {
            Id = apiAvatar_0.id ?? string.Empty;
            ApiVersion = apiAvatar_0.apiVersion;
            AssetUrl = apiAvatar_0.assetUrl ?? string.Empty;
            AssetVersion = new CustomAssetVersion(apiAvatar_0.assetVersion);
            AuthorId = apiAvatar_0.authorId ?? string.Empty;
            AuthorName = apiAvatar_0.authorName ?? string.Empty;
            Created = apiAvatar_0.created_at.ToManagedDateTime();
            Description = apiAvatar_0.description ?? string.Empty;
            Featured = apiAvatar_0.featured;
            ImageUrl = apiAvatar_0.imageUrl ?? string.Empty;
            Name = apiAvatar_0.name ?? string.Empty;
            Platform = apiAvatar_0.platform ?? string.Empty;
            ReleaseStatus = apiAvatar_0.releaseStatus ?? string.Empty;
            Tags = apiAvatar_0.tags.ToArray().ToList();
            ThumbnailImageUrl = apiAvatar_0.thumbnailImageUrl ?? string.Empty;
            UnityPackageUrl = apiAvatar_0.unityPackageUrl ?? string.Empty;
            Updated = apiAvatar_0.updated_at.ToManagedDateTime();
            Version = apiAvatar_0.version;
        }

        public override bool Equals(object obj)
        {
            CustomApiAvatar customApiAvatar = obj as CustomApiAvatar;
            if (customApiAvatar != null && customApiAvatar.AssetUrl == AssetUrl)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return AssetUrl.GetHashCode();
        }
    }
}
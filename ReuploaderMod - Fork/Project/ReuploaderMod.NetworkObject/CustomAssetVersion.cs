using VRC.Core;

namespace ReuploaderMod.NetworkObject
{
    public class CustomAssetVersion
    {
        private int int_0;

        private string string_0;

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

        public string UnityVersion
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

        public CustomAssetVersion()
        {
        }

        public CustomAssetVersion(AssetVersion assetVersion_0)
        {
            ApiVersion = assetVersion_0.ApiVersion;
            UnityVersion = assetVersion_0.UnityVersion;
        }
    }
}
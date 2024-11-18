using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Templar
{
    public static class Assets
    {
        public static void Initialize()
        {
            MainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Loader.pluginInfo.Location), "templar"));
            SecondaryAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Loader.pluginInfo.Location), "skillbundle"));
            PopulateAssets();
        }

        internal static void PopulateAssets()
        {
            bool flag = !MainAssetBundle;
            if (flag)
            {
                Debug.LogError("There is no Main AssetBundle to load assets from.");
            }
            else
            {
                clayBombModel = MainAssetBundle.LoadAsset<GameObject>("ClayBombModel");
                clayMissileModel = MainAssetBundle.LoadAsset<GameObject>("ClayMissileModel");
                templarIcon = MainAssetBundle.LoadAsset<Texture>("TemplarBody");
                templarIcon.filterMode = FilterMode.Point;
                templarIconOld = MainAssetBundle.LoadAsset<Texture>("TemplarBodyOld");
                templarIconOld.filterMode = FilterMode.Point;
                templarSkinTex = MainAssetBundle.LoadAsset<Texture>("texTemplarSkin");
                bool flag2 = !SecondaryAssetBundle;
                if (flag2)
                {
                    Debug.LogError("There is no Secondary AssetBundle to load assets from.");
                }
                else
                {
                    iconP = MainAssetBundle.LoadAsset<Sprite>("TarNewIcon");
                    icon1 = MainAssetBundle.LoadAsset<Sprite>("MinigunNewIcon");
                    icon1b = MainAssetBundle.LoadAsset<Sprite>("RifleNewIcon");
                    icon1c = MainAssetBundle.LoadAsset<Sprite>("RailgunNewIcon");
                    icon2 = MainAssetBundle.LoadAsset<Sprite>("ClayBombNewIcon");
                    icon2b = MainAssetBundle.LoadAsset<Sprite>("ShotgunNewIcon");
                    icon3 = MainAssetBundle.LoadAsset<Sprite>("TarBurstNewIcon");
                    icon3b = MainAssetBundle.LoadAsset<Sprite>("SidestepIcon");
                    icon4 = MainAssetBundle.LoadAsset<Sprite>("BazookaNewIcon");
                    icon4b = MainAssetBundle.LoadAsset<Sprite>("SwapNewIcon");
                    icon1d = SecondaryAssetBundle.LoadAsset<Sprite>("FlamethrowerIcon");
                }
            }
        }

        public static AssetBundle MainAssetBundle = null;

        private static string[] assetNames = new string[0];

        public static AssetBundle SecondaryAssetBundle = null;

        public static Texture templarIcon;

        public static Texture templarIconOld;

        public static Sprite iconP;

        public static Sprite icon1;

        public static Sprite icon1b;

        public static Sprite icon1c;

        public static Sprite icon1d;

        public static Sprite icon2;

        public static Sprite icon2b;

        public static Sprite icon3;

        public static Sprite icon3b;

        public static Sprite icon4;

        public static Sprite icon4b;

        public static Texture templarSkinTex;

        public static GameObject clayBombModel;

        public static GameObject clayMissileModel;
    }
}

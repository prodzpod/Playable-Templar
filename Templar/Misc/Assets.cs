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
			bool flag = Assets.MainAssetBundle == null;
			if (flag)
			{
				using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Templar.templar"))
				{
					Assets.MainAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream);
				}
			}
			Assets.assetNames = Assets.MainAssetBundle.GetAllAssetNames();
			bool flag2 = Assets.SecondaryAssetBundle == null;
			if (flag2)
			{
				using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Templar.skillbundle"))
				{
					Assets.SecondaryAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream2);
				}
			}
			Assets.PopulateAssets();
		}

		internal static void PopulateAssets()
		{
			bool flag = !Assets.MainAssetBundle;
			if (flag)
			{
				Debug.LogError("There is no Main AssetBundle to load assets from.");
			}
			else
			{
				Assets.clayBombModel = Assets.MainAssetBundle.LoadAsset<GameObject>("ClayBombModel");
				Assets.clayMissileModel = Assets.MainAssetBundle.LoadAsset<GameObject>("ClayMissileModel");
				Assets.templarIcon = Assets.MainAssetBundle.LoadAsset<Texture>("TemplarBody");
				Assets.templarIcon.filterMode = FilterMode.Point;
				Assets.templarIconOld = Assets.MainAssetBundle.LoadAsset<Texture>("TemplarBodyOld");
				Assets.templarIconOld.filterMode = FilterMode.Point;
				Assets.templarSkinTex = Assets.MainAssetBundle.LoadAsset<Texture>("texTemplarSkin");
				bool flag2 = !Assets.SecondaryAssetBundle;
				if (flag2)
				{
					Debug.LogError("There is no Secondary AssetBundle to load assets from.");
				}
				else
				{
					Assets.iconP = Assets.MainAssetBundle.LoadAsset<Sprite>("TarNewIcon");
					Assets.icon1 = Assets.MainAssetBundle.LoadAsset<Sprite>("MinigunNewIcon");
					Assets.icon1b = Assets.MainAssetBundle.LoadAsset<Sprite>("RifleNewIcon");
					Assets.icon1c = Assets.MainAssetBundle.LoadAsset<Sprite>("RailgunNewIcon");
					Assets.icon2 = Assets.MainAssetBundle.LoadAsset<Sprite>("ClayBombNewIcon");
					Assets.icon2b = Assets.MainAssetBundle.LoadAsset<Sprite>("ShotgunNewIcon");
					Assets.icon3 = Assets.MainAssetBundle.LoadAsset<Sprite>("TarBurstNewIcon");
					Assets.icon3b = Assets.MainAssetBundle.LoadAsset<Sprite>("SidestepIcon");
					Assets.icon4 = Assets.MainAssetBundle.LoadAsset<Sprite>("BazookaNewIcon");
					Assets.icon4b = Assets.MainAssetBundle.LoadAsset<Sprite>("SwapNewIcon");
					Assets.icon1d = Assets.SecondaryAssetBundle.LoadAsset<Sprite>("FlamethrowerIcon");
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

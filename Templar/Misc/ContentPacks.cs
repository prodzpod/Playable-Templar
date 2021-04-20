using RoR2.ContentManagement;
using System;
using System.Collections;

namespace Templar
{
	public class ContentPacks : IContentPackProvider
	{
		public string identifier
		{
			get
			{
				return "Lemurian.LemurianContent";
			}
		}

		public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
		{
			ContentPacks.contentPack.buffDefs.Add(Buffs.buffDefs.ToArray());
			ContentPacks.contentPack.survivorDefs.Add(Loader.survivorDefs.ToArray());
			ContentPacks.contentPack.bodyPrefabs.Add(Loader.bodyPrefabs.ToArray());
			ContentPacks.contentPack.masterPrefabs.Add(Loader.masterPrefabs.ToArray());
			ContentPacks.contentPack.projectilePrefabs.Add(Loader.projectilePrefabs.ToArray());
			args.ReportProgress(1f);
			yield break;
		}

		public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
		{
			ContentPack.Copy(ContentPacks.contentPack, args.output);
			yield break;
		}

		public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
		{
			args.ReportProgress(1f);
			yield break;
		}

		internal static ContentPack contentPack = new ContentPack();
	}
}

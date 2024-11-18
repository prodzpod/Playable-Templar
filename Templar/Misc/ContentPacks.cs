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
            contentPack.buffDefs.Add(Buffs.buffDefs.ToArray());

            contentPack.skillDefs.Add(Loader.skillDefs.ToArray());
            contentPack.skillFamilies.Add(Loader.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(Loader.survivorDefs.ToArray());
            contentPack.bodyPrefabs.Add(Loader.bodyPrefabs.ToArray());
            contentPack.masterPrefabs.Add(Loader.masterPrefabs.ToArray());
            contentPack.projectilePrefabs.Add(Loader.projectilePrefabs.ToArray());
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(contentPack, args.output);
            yield break;
        }

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }

        internal static ContentPack contentPack = new();
    }
}

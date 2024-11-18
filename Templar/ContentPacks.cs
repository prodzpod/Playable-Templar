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

            contentPack.skillDefs.Add(Main.skillDefs.ToArray());
            contentPack.skillFamilies.Add(Main.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(Main.survivorDefs.ToArray());
            contentPack.bodyPrefabs.Add(Main.bodyPrefabs.ToArray());
            contentPack.masterPrefabs.Add(Main.masterPrefabs.ToArray());
            contentPack.projectilePrefabs.Add(Main.projectilePrefabs.ToArray());
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

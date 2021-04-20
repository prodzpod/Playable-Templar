using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Templar
{
    public static class TemplarSkins
    {
        private static SkinDef.GameObjectActivation[] getActivations(GameObject[] allObjects, params GameObject[] activatedObjects)
        {
            List<SkinDef.GameObjectActivation> GameObjectActivations = new List<SkinDef.GameObjectActivation>();

            for (int i = 0; i < allObjects.Length; i++)
            {

                bool activate = activatedObjects.Contains(allObjects[i]);

                GameObjectActivations.Add(new SkinDef.GameObjectActivation
                {
                    gameObject = allObjects[i],
                    shouldActivate = activate
                });
            }

            return GameObjectActivations.ToArray();
        }

        public static void RegisterSkins()
        {
            GameObject bodyPrefab = Templar.myCharacter;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();
            SkinnedMeshRenderer mainRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer");

            #region Default Skin
            LanguageAPI.Add("Templar_DEFAULT_SKIN", "Default");
            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
            skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f), new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f));
            skinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[0];
            skinDefInfo.Name = "Templar_DEFAULT_SKIN";
            skinDefInfo.NameToken = "Templar_DEFAULT_SKIN";
            skinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            skinDefInfo.RootObject = model;
            CharacterModel.RendererInfo[] rendererInfos = skinDefInfo.RendererInfos;
            CharacterModel.RendererInfo[] array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);
            Material material = array[0].defaultMaterial;
            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                array[0].defaultMaterial = material;
            }
            skinDefInfo.RendererInfos = array;
            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
            #endregion

            var skinDefs = new List<SkinDef>()
                {
                    defaultSkin
                };

            skinController.skins = skinDefs.ToArray();
        }
    }
}
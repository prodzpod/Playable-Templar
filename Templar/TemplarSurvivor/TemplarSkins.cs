using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Linq;
using Templar.TemplarSurvivor;

namespace Templar
{
    public static class TemplarSkins
    {
        private static SkinDef.GameObjectActivation[] getActivations(GameObject[] allObjects, params GameObject[] activatedObjects)
        {
            List<SkinDef.GameObjectActivation> GameObjectActivations = [];

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
            SkinDefInfo skinDefInfo = default;
            skinDefInfo.BaseSkins = [];
            skinDefInfo.MinionSkinReplacements = [];
            skinDefInfo.ProjectileGhostReplacements = [];
            skinDefInfo.Icon = Skins.CreateSkinIcon(new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f), new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f));
            skinDefInfo.MeshReplacements = [];
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
                material = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                array[0].defaultMaterial = material;
            }
            skinDefInfo.RendererInfos = array;
            SkinDef defaultSkin = Skins.CreateNewSkinDef(skinDefInfo);
            #endregion

            var skinDefs = new List<SkinDef>()
                {
                    defaultSkin
                };

            skinController.skins = skinDefs.ToArray();
            TemplarSkins2.Patch();
        }
    }
}
using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Linq;
using Templar.TemplarSurvivor;
using System.ComponentModel;

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
            var smrs = bodyPrefab.GetComponentsInChildren<SkinnedMeshRenderer>();
            var mrs = bodyPrefab.GetComponentsInChildren<MeshRenderer>();

            LanguageAPI.Add("Templar_DEFAULT_SKIN", "Default");
            LanguageAPI.Add("TEMPLARBODY_ALT_SKIN_NAME", "Vagabond");
            SkinDefInfo skinDefInfo = default;
            skinDefInfo.BaseSkins = [];
            skinDefInfo.MinionSkinReplacements = [];
            skinDefInfo.ProjectileGhostReplacements = [];
            skinDefInfo.Icon = Skins.CreateSkinIcon(new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f), new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f));
            skinDefInfo.MeshReplacements = [];
            skinDefInfo.GameObjectActivations =
            [
                new SkinDef.GameObjectActivation
                {
                    gameObject = smrs.FirstOrDefault(x => x && x.gameObject.name == "ClayBruiserChestArmorMesh").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = smrs.FirstOrDefault(x => x && x.gameObject.name == "ClayBruiserHeadMesh").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = mrs.FirstOrDefault(x => x && x.gameObject.name == "VagabondHead").gameObject,
                    shouldActivate = false
                }
            ];
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
            SkinDef skinDef = Skins.CreateNewSkinDef(skinDefInfo);

            SkinDefInfo skinDefInfo2 = default;
            skinDefInfo2.BaseSkins = [];
            skinDefInfo2.MinionSkinReplacements = [];
            skinDefInfo2.ProjectileGhostReplacements = [];
            skinDefInfo2.Icon = Skins.CreateSkinIcon(new Color(0.66f, 0.41f, 0.29f), new Color(0f, 0f, 0f), new Color(0.46f, 0.25f, 0.02f), new Color(0.47f, 0.16f, 0.16f));
            skinDefInfo2.MeshReplacements = [];
            skinDefInfo2.GameObjectActivations =
            [
                new SkinDef.GameObjectActivation
                {
                    gameObject = smrs.FirstOrDefault(x => x && x.gameObject.name == "ClayBruiserChestArmorMesh").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = smrs.FirstOrDefault(x => x && x.gameObject.name == "ClayBruiserHeadMesh").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = mrs.FirstOrDefault(x => x && x.gameObject.name == "VagabondHead").gameObject,
                    shouldActivate = true
                }
            ];
            skinDefInfo2.Name = "TEMPLARBODY_ALT_SKIN_NAME";
            skinDefInfo2.NameToken = "TEMPLARBODY_ALT_SKIN_NAME";
            skinDefInfo2.RendererInfos = [..array];
            skinDefInfo2.RootObject = model;
            SkinDef skinDef2 = Skins.CreateNewSkinDef(skinDefInfo2);
            skinController.skins = [skinDef, skinDef2];
            TemplarSkins2.Patch();
        }
    }
}
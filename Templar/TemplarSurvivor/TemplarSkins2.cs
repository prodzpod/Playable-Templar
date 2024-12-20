﻿using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Templar.TemplarSurvivor
{
    public class TemplarSkins2
    {
        public static GameObject bodyPrefab;
        public static GameObject gameObject;
        public static SkinDef defaultSkin;
        public static bool FORCEBAKE = false;
        public static void Patch()
        {
            bodyPrefab = Templar.TemplarPrefab;
            gameObject = bodyPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;
            // gameObject.transform.Find("ClayBruiserArmature").Find("ROOT").Find("base").Find("stomach").Find("chest").Find("neck").Find("head").Find("VagabondHead").gameObject.SetActive(false);
            ModelSkinController modelSkinController = gameObject.GetComponent<ModelSkinController>();
            List<SkinDef> templarSkins = [.. modelSkinController.skins];
            if (Templar.EnableTemplarSkin.Value)
            {
                templarSkins.Add(AddTemplarSkin("skinTemplarYellowAlt", "Yellow", rgb(183, 96, 20), rgb(85, 37, 16)));
                templarSkins.Add(AddTemplarSkin("skinTemplarRedAlt", "Red", rgb(228, 0, 28), rgb(92, 9, 17)));
                templarSkins.Add(AddTemplarSkin("skinTemplarGreenAlt", "Green", rgb(90, 153, 115), rgb(51, 56, 41)));
                templarSkins.Add(AddTemplarSkin("skinTemplarGoldAlt", "Gold", rgb(252, 238, 157), rgb(145, 80, 17)));
                templarSkins.Add(AddTemplarSkin("skinTemplarAlt", "Black", rgb(19, 6, 6), rgb(10, 1, 2)));
                templarSkins.Add(AddTemplarSkin("skinTemplarBulwarksHauntAlt", "White", rgb(224, 178, 180), rgb(146, 113, 119)));
                templarSkins.Add(AddTemplarSkin("skinTemplarVoidAlt", "Void", rgb(255, 2, 253), rgb(85, 37, 16)));
                templarSkins.Add(AddTemplarSkin("TemplarNeo", "Lunar", rgb(214, 239, 245), rgb(128, 117, 128)));
                templarSkins.Add(AddTemplarSkin("skinTemplarInfernoAlt", "Inferno", rgb(249, 243, 186), rgb(144, 33, 22)));
            }
            modelSkinController.skins = templarSkins.ToArray();
            if (!Templar.EnableEngiVoidSkin.Value) return;
            SkinDef skinDef = AddSkin("EngiBody", "skinEngiVoidAlt", "TEMPLARSKINS_ENGIVOID_NAME", "matEngi", "matEngiVoid", rgb(255, 0, 255), rgb(50, 11, 46));
            SkinDef.MinionSkinReplacement[] minionSkinReplacementArray = new SkinDef.MinionSkinReplacement[2];
            SkinDef.MinionSkinReplacement minionSkinReplacement = new()
            {
                minionBodyPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody"),
                minionSkin = AddSkin("EngiTurretBody", "skinEngiTurretVoidAlt", "TEMPLARSKINS_ENGIVOID_NAME", "matEngiTurret", "matEngiTurretVoid", rgb(255, 0, 255), rgb(50, 11, 46))
            };
            minionSkinReplacementArray[0] = minionSkinReplacement; minionSkinReplacement = new SkinDef.MinionSkinReplacement
            {
                minionBodyPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody"),
                minionSkin = AddSkin("EngiWalkerTurretBody", "skinEngiWalkerTurretVoidAlt", "TEMPLARSKINS_ENGIVOID_NAME", "matEngiTurret", "matEngiTurretVoid", rgb(255, 0, 255), rgb(50, 11, 46))
            };
            minionSkinReplacementArray[1] = minionSkinReplacement; skinDef.minionSkinReplacements = minionSkinReplacementArray;
            ModelSkinController skinController = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<ModelSkinController>();
            for (var i = 0; i < skinController.skins.Length; i++) if (skinController.skins[i].name == skinDef.name) skinController.skins[i].minionSkinReplacements = minionSkinReplacementArray;
        }

        public static Color rgb(byte r, byte g, byte b, byte a = 255)
        {
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public static SkinDef AddTemplarSkin(string skinName, string materialName, Color c1 = new(), Color c2 = new())
        {
            List<CharacterModel.RendererInfo> infos = new(defaultSkin.rendererInfos);
            for (var i = 0; i < infos.Count; i++)
            {
                CharacterModel.RendererInfo info = infos[i];
                if (info.defaultMaterial == null) continue;
                if (info.defaultMaterial.name.Contains("matClayBruiser")) info.defaultMaterial = Assets.TemplarSkins.LoadAsset<Material>("Assets/templarSkin/matClayBruiser" + materialName + ".mat");
                else if (info.defaultMaterial.name.Contains("matTrimSheetClayBruiser")) info.defaultMaterial = Assets.TemplarSkins.LoadAsset<Material>("Assets/templarSkin/matTrimSheetClayBruiser" + materialName + ".mat");
                infos[i] = info;
            }
            Main.Log.LogDebug("Adding Skin " + skinName + " to Templar");
            return Skins.CreateNewSkinDef(new SkinDefInfo()
            {
                BaseSkins = [defaultSkin],
                Name = skinName,
                NameToken = "TEMPLARSKINS_TEMPLAR" + materialName.ToUpper() + "_NAME",
                UnlockableDef = null,
                RootObject = gameObject,
                RendererInfos = infos.ToArray(),
                Icon = Skins.CreateSkinIcon(c1, c2, c1, c2)
            });
        }

        public static SkinDef AddSkin(string bodyName, string skinName, string skinNameToken, string materialNameFrom, string materialNameTo, Color c1 = new(), Color c2 = new(), Color c3 = new(), Color c4 = new())
        {
            GameObject bodyPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName);
            GameObject gameObject = bodyPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;
            ModelSkinController skinController = gameObject.GetComponent<ModelSkinController>();
            Main.Log.LogDebug("Adding Skin " + skinName + " to " + bodyName);
            SkinDef def = Skins.CreateNewSkinDef(new SkinDefInfo()
            {
                BaseSkins = [skinController.skins[0]],
                Name = skinName,
                NameToken = skinNameToken,
                UnlockableDef = null,
                RootObject = gameObject,
                RendererInfos = InfoWithMaterial(skinController.skins[0].rendererInfos, materialNameFrom, materialNameTo),
                Icon = Skins.CreateSkinIcon(c1, c2, c3, c4)
            });
            List<SkinDef> skins = new(skinController.skins)
            {
                def
            };
            skinController.skins = skins.ToArray();
            return def;
        }

        public static CharacterModel.RendererInfo[] InfoWithMaterial(CharacterModel.RendererInfo[] infoRaw, string materialNameFrom, string materialNameTo)
        {
            List<CharacterModel.RendererInfo> infos = new(infoRaw);
            for (var i = 0; i < infos.Count; i++)
            {
                CharacterModel.RendererInfo info = infos[i];
                if (info.defaultMaterial == null) continue;
                if (info.defaultMaterial.name.Contains(materialNameFrom)) info.defaultMaterial = Assets.TemplarSkins.LoadAsset<Material>("Assets/templarSkin/" + materialNameTo + ".mat");
                infos[i] = info;
            }
            return infos.ToArray();
        }
    }
}

using MonoMod.Cil;
using R2API;
using System;
using System.Collections.Generic;
using static RoR2.UI.CharacterSelectController;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using EntityStates;
using RoR2.UI;
using Mono.Cecil.Cil;
using System.Linq;

namespace Templar.TemplarSurvivor
{
    public class MalignantOrigins
    {
        public static GenericSkill passive;
        public static SkillDef radiant;
        public static SkillDef malignance;
        public static bool isMalignanceOn = false;
        public static List<CharacterBody> malignantCharacters = [];
        public const float Multiplier = 2.5f;
        public static int level;
        public static void Patch()
        {
            // radiant
            radiant = ScriptableObject.CreateInstance<SkillDef>();
            radiant.activationState = new SerializableEntityStateType(typeof(Idle));
            radiant.activationStateMachineName = "Body";
            radiant.baseRechargeInterval = 0f;
            radiant.skillNameToken = "SKILL_TEMPLARPASSIVE_RADIANT_NAME";
            radiant.skillDescriptionToken = "SKILL_TEMPLARPASSIVE_RADIANT_DESC";
            radiant.icon = Assets.TemplarSkins.LoadAsset<Sprite>("Assets/texRadiantOrigins.png");
            ContentAddition.AddSkillDef(radiant);
            // malignance
            malignance = ScriptableObject.CreateInstance<MalignanceSkill>();
            malignance.activationState = new SerializableEntityStateType(typeof(Idle));
            malignance.activationStateMachineName = "Body";
            malignance.baseRechargeInterval = 0f;
            malignance.skillNameToken = "SKILL_TEMPLARPASSIVE_MALIGNANT_NAME";
            malignance.skillDescriptionToken = "SKILL_TEMPLARPASSIVE_MALIGNANT_DESC";
            malignance.icon = Assets.iconP;
            ContentAddition.AddSkillDef(malignance);
            // add skillfamily
            passive = Templar.myCharacter.AddComponent<GenericSkill>();
            passive._skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            passive.skillName = "TemplarPassive";
            passive.skillFamily.variants = new SkillFamily.Variant[2];
            passive.skillFamily.variants[0] = new SkillFamily.Variant()
            {
                skillDef = radiant,
                viewableNode = new ViewablesCatalog.Node(radiant.skillNameToken, false)
            };
            passive.skillFamily.variants[1] = new SkillFamily.Variant()
            {
                skillDef = malignance,
                viewableNode = new ViewablesCatalog.Node(malignance.skillNameToken, false)
            };
            passive.skillFamily.defaultVariantIndex = 1;
            ContentAddition.AddSkillFamily(passive.skillFamily);
            SkillLocator loc = Templar.myCharacter.GetComponent<SkillLocator>();
            loc.passiveSkill.enabled = false;
            loc.passiveSkill.keywordToken = null;
            // this IS in r2api <3333333
            Stage.onStageStartGlobal += _ => malignantCharacters.Clear();
        }

        public class MalignanceSkill : SkillDef
        {
            public static bool vfxOn = false;
            public override BaseSkillInstanceData OnAssigned(GenericSkill skillSlot)
            {
                if (!isMalignanceOn)
                {
                    level = 1;
                    RecalculateStatsAPI.GetStatCoefficients += MalignantOriginsAPIUpdate;
                    On.RoR2.UI.ExpBar.Update += MalignantOriginsUIUpdate;
                    On.RoR2.Run.RecalculateDifficultyCoefficentInternal += MalignantOriginsEventUpdate;
                    On.RoR2.CharacterBody.RecalculateStats += MalignantOriginsDebugUpdate;
                    On.RoR2.CharacterBody.OnLevelUp += MalignantOriginsVFXUpdate;
                    Run.onRunDestroyGlobal += Unhook;
                    isMalignanceOn = true;
                }
                return base.OnAssigned(skillSlot); // should be null
            }

            public static void Unhook(Run _ = null)
            {
                RecalculateStatsAPI.GetStatCoefficients -= MalignantOriginsAPIUpdate;
                On.RoR2.UI.ExpBar.Update -= MalignantOriginsUIUpdate;
                On.RoR2.Run.RecalculateDifficultyCoefficentInternal -= MalignantOriginsEventUpdate;
                On.RoR2.CharacterBody.RecalculateStats -= MalignantOriginsDebugUpdate;
                On.RoR2.CharacterBody.OnLevelUp -= MalignantOriginsVFXUpdate;
                Run.onRunDestroyGlobal -= Unhook;
                isMalignanceOn = false;
            }

            public static void MalignantOriginsAPIUpdate(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
            {
                if (body?.skillLocator != null && Array.Exists(body.skillLocator.allSkills, x => x.skillDef == malignance) && body?.inventory != null)
                {
                    if (!malignantCharacters.Contains(body)) malignantCharacters.Add(body);
                    float oldLevel = TeamManager.instance.GetTeamLevel(body.teamComponent.teamIndex);
                    if (body.inventory.GetItemCount(RoR2Content.Items.UseAmbientLevel) > 0) oldLevel = Math.Max(oldLevel, Run.instance.ambientLevelFloor);
                    oldLevel += body.inventory.GetItemCount(RoR2Content.Items.LevelBonus);
                    args.levelFlatAdd += level - oldLevel;
                }
            }
            public static void MalignantOriginsUIUpdate(On.RoR2.UI.ExpBar.orig_Update orig, ExpBar self)
            {
                orig(self);
                CharacterBody body = LocalUserManager.GetFirstLocalUser()?.cachedBody;
                if (body?.skillLocator != null && Array.Exists(body.skillLocator.allSkills, x => x.skillDef == malignance) && body?.inventory != null)
                    self.fillRectTransform.anchorMax = new Vector2((Run.instance.ambientLevel - 1) % Multiplier / Multiplier, 1f);
            }
            public static void MalignantOriginsEventUpdate(On.RoR2.Run.orig_RecalculateDifficultyCoefficentInternal orig, Run self)
            {
                orig(self);
                foreach (var body in malignantCharacters) if (body != null && body.isActiveAndEnabled)
                    {
                        int newLevel = Mathf.FloorToInt((self.ambientLevel - 1) / Multiplier) + 1;
                        if (newLevel != level) body.MarkAllStatsDirty();
                        if (newLevel > level)
                        {
                            vfxOn = true;
                            body.OnLevelUp();
                            vfxOn = false;
                        }
                        if (newLevel != level) level = newLevel;
                    }
            }

            public static void MalignantOriginsVFXUpdate(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self) { if (self == null || vfxOn || !malignantCharacters.Contains(self)) orig(self); }

            public static void MalignantOriginsDebugUpdate(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
            {
                orig(self);
                if (self != null && malignantCharacters.Contains(self)) self.level = level;
            }
        }
    }
}

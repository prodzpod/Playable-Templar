﻿using MonoMod.Cil;
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
            // this REALLY should be some kind of r2api thing
            if (!Main.Mods("com.TeamSandswept.Sandswept") && !Main.Mods("prodzpod.MinerSkillReturns") && !Main.Mods("xyz.yekoc.PassiveAgression"))
            {
                On.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot += (orig, owner, bodyI, slotI, slot) =>
                {
                    LoadoutPanelController.Row row = (LoadoutPanelController.Row)orig(owner, bodyI, slotI, slot);
                    if (slot.skillFamily == passive.skillFamily)
                    {
                        Transform label = row.rowPanelTransform.Find("SlotLabel") ?? row.rowPanelTransform.Find("LabelContainer").Find("SlotLabel");
                        if (label) label.GetComponent<LanguageTextMeshController>().token = "Passive";
                    }
                    return row;
                };
                IL.RoR2.UI.CharacterSelectController.BuildSkillStripDisplayData += (il) =>
                {
                    ILCursor c = new ILCursor(il);
                    int skillIndex = -1;
                    if (c.TryGotoNext(x => x.MatchLdloc(out skillIndex), x => x.MatchLdfld(typeof(GenericSkill).GetField("hideInCharacterSelect")), x => x.MatchBrtrue(out _)) && skillIndex != (-1) && c.TryGotoNext(MoveType.After, x => x.MatchLdfld(typeof(SkillFamily.Variant).GetField("skillDef")), x => x.MatchStloc(out _)))
                    {
                        if (c.TryGotoNext(x => x.MatchCallOrCallvirt(typeof(List<StripDisplayData>).GetMethod("Add"))))
                        {
                            c.Remove();
                            c.Emit(OpCodes.Ldloc, skillIndex);
                            c.EmitDelegate<Action<List<StripDisplayData>, StripDisplayData, GenericSkill>>((list, disp, ski) =>
                            {
                                if (SkillCatalog.GetSkillFamilyName(ski.skillFamily.catalogIndex).ToUpper().Contains("PASSIVE")) list.Insert(0, disp);
                                else list.Add(disp);
                            });
                        }
                    }
                };
                IL.RoR2.UI.LoadoutPanelController.Rebuild += (il) =>
                {
                    ILCursor c = new ILCursor(il);
                    if (c.TryGotoNext(MoveType.After, x => x.MatchLdloc(0), x => x.MatchCallOrCallvirt(out _), x => x.MatchCallOrCallvirt(out _), x => x.MatchStloc(1)))
                    {
                        c.Emit(OpCodes.Ldloc_1);
                        c.Emit(OpCodes.Ldarg_0);
                        c.EmitDelegate<Action<List<GenericSkill>, LoadoutPanelController>>((list, self) =>
                        {
                            foreach (var slot in list.Where((slot) => { return slot != list.First() && slot.skillFamily == passive.skillFamily; }))
                                self.rows.Add(LoadoutPanelController.Row.FromSkillSlot(self, self.currentDisplayData.bodyIndex, list.FindIndex((skill) => skill == slot), slot));
                            list.RemoveAll((slot) => { return slot != list.First() && slot.skillFamily == passive.skillFamily; });
                        });
                    }
                };
            }
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

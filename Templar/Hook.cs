using R2API.Utils;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public static class Hook
    {
        internal static void HookSetup()
        {
            On.RoR2.CharacterBody.RecalculateStats += delegate (On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
            {
                orig(self);
                bool flag = self && self.HasBuff(Buffs.TemplArmorBuff);
                if (flag)
                {
                    self.SetPropertyValue("armor", self.armor + 50f);
                }
                bool flag2 = self && self.HasBuff(Buffs.TemplarstationaryArmorBuff);
                if (flag2)
                {
                    self.SetPropertyValue("armor", self.armor + 100f);
                }
                bool flag3 = self && self.HasBuff(Buffs.TemplarOverdriveBuff);
                if (flag3)
                {
                    self.SetPropertyValue("regen", self.regen * 12f);
                    self.SetPropertyValue("attackSpeed", self.attackSpeed * 1.5f);
                }
                bool flag4 = self && self.HasBuff(Buffs.TemplarigniteDebuff);
                if (flag4)
                {
                    self.SetPropertyValue("armor", self.armor - 45f);
                    self.SetPropertyValue("moveSpeed", self.moveSpeed * 0.8f);
                }
            };
            On.RoR2.HealthComponent.TakeDamage += delegate (On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
            {
                orig(self, di);
                bool flag = di.inflictor != null && self != null && di.attacker != null && di.attacker.GetComponent<CharacterBody>() != null && di.attacker.GetComponent<CharacterBody>().baseNameToken == "Templar_Survivor" && di.damageType.damageType.HasFlag(DamageType.BypassOneShotProtection) && self.GetComponent<CharacterBody>().HasBuff(RoR2.RoR2Content.Buffs.ClayGoo) && !self.GetComponent<CharacterBody>().HasBuff(Buffs.TemplarigniteDebuff);
                if (flag)
                {
                    self.GetComponent<CharacterBody>().AddTimedBuff(Buffs.TemplarigniteDebuff, 12f);
                    bool flag2 = self.GetComponent<CharacterBody>().modelLocator;
                    bool flag3 = flag2;
                    if (flag3)
                    {
                        Transform modelTransform = self.GetComponent<CharacterBody>().modelLocator.modelTransform;
                        bool flag4 = modelTransform.GetComponent<CharacterModel>();
                        bool flag5 = flag4;
                        if (flag5)
                        {
                            TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                            temporaryOverlay.duration = 16f;
                            temporaryOverlay.animateShaderAlpha = true;
                            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                            temporaryOverlay.destroyComponentOnEnd = true;
                            temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matDoppelganger");
                            temporaryOverlay.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
                        }
                        BlastAttack blastAttack = new()
                        {
                            attacker = di.inflictor,
                            inflictor = di.inflictor,
                            teamIndex = TeamIndex.Player,
                            baseForce = 0f,
                            position = self.transform.position,
                            radius = 12f,
                            falloffModel = RoR2.BlastAttack.FalloffModel.None,
                            crit = di.crit,
                            baseDamage = di.damage * 0.2f,
                            procCoefficient = di.procCoefficient
                        };
                        blastAttack.damageType |= DamageType.Stun1s;
                        blastAttack.Fire();
                        BlastAttack blastAttack2 = new()
                        {
                            attacker = di.inflictor,
                            inflictor = di.inflictor,
                            teamIndex = TeamIndex.Player,
                            baseForce = 0f,
                            position = self.transform.position,
                            radius = 16f,
                            falloffModel = RoR2.BlastAttack.FalloffModel.None,
                            crit = false,
                            baseDamage = 0f,
                            procCoefficient = 0f,
                            damageType = DamageType.BypassOneShotProtection
                        };
                        blastAttack2.Fire();
                        RoR2.EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/MagmaOrbExplosion"), new EffectData
                        {
                            origin = self.transform.position,
                            scale = 16f
                        }, true);
                    }
                }
            };
        }
    }
}

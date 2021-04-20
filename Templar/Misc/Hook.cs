using On.RoR2;
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
			On.RoR2.CharacterBody.RecalculateStats += delegate(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
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
			On.RoR2.HealthComponent.TakeDamage += delegate(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo di)
			{
				orig(self, di);
				bool flag = di.inflictor != null && self != null && di.attacker != null && di.attacker.GetComponent<RoR2.CharacterBody>() != null && di.attacker.GetComponent<RoR2.CharacterBody>().baseNameToken == "Templar_Survivor" && di.damageType.HasFlag(DamageType.BypassOneShotProtection) && self.GetComponent<RoR2.CharacterBody>().HasBuff(RoR2.RoR2Content.Buffs.ClayGoo) && !self.GetComponent<RoR2.CharacterBody>().HasBuff(Buffs.TemplarigniteDebuff);
				if (flag)
				{
					self.GetComponent<RoR2.CharacterBody>().AddTimedBuff(Buffs.TemplarigniteDebuff, 12f);
					bool flag2 = self.GetComponent<RoR2.CharacterBody>().modelLocator;
					bool flag3 = flag2;
					if (flag3)
					{
						Transform modelTransform = self.GetComponent<RoR2.CharacterBody>().modelLocator.modelTransform;
						bool flag4 = modelTransform.GetComponent<RoR2.CharacterModel>();
						bool flag5 = flag4;
						if (flag5)
						{
							RoR2.TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
							temporaryOverlay.duration = 16f;
							temporaryOverlay.animateShaderAlpha = true;
							temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
							temporaryOverlay.destroyComponentOnEnd = true;
							temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matDoppelganger");
							temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<RoR2.CharacterModel>());
						}
						RoR2.BlastAttack blastAttack = new RoR2.BlastAttack
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
						RoR2.BlastAttack blastAttack2 = new RoR2.BlastAttack
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
						RoR2.EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MagmaOrbExplosion"), new RoR2.EffectData
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

using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.ParentMonster;
using EntityStates.ScavMonster;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarSwapWeapon : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			SkillDef skillDef = base.characterBody.skillLocator.primary.skillDef;
			bool flag = skillDef.skillNameToken == "TEMPLAR_PRIMARY_MINIGUN_NAME";
			bool flag2 = flag;
			if (flag2)
			{
				TemplarSwapWeapon.currentWeapon = 0;
			}
			else
			{
				bool flag3 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
				bool flag4 = flag3;
				if (flag4)
				{
					TemplarSwapWeapon.currentWeapon = 1;
				}
				else
				{
					bool flag5 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_RAILGUN_NAME";
					bool flag6 = flag5;
					if (flag6)
					{
						TemplarSwapWeapon.currentWeapon = 2;
					}
					else
					{
						bool flag7 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_BAZOOKA_NAME";
						bool flag8 = flag7;
						if (flag8)
						{
							TemplarSwapWeapon.currentWeapon = 2;
						}
						else
						{
							bool flag9 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
							bool flag10 = flag9;
							if (flag10)
							{
								TemplarSwapWeapon.currentWeapon = 3;
							}
						}
					}
				}
			}
			TemplarSwapWeapon.currentWeapon++;
			bool flag11 = TemplarSwapWeapon.currentWeapon > 3;
			bool flag12 = flag11;
			if (flag12)
			{
				TemplarSwapWeapon.currentWeapon = 0;
			}
			base.characterBody.skillLocator.primary.SetBaseSkill(base.characterBody.skillLocator.primary.skillFamily.variants[TemplarSwapWeapon.currentWeapon].skillDef);
			bool flag13 = TemplarSwapWeapon.currentWeapon == 0;
			bool flag14 = flag13;
			if (flag14)
			{
				base.characterBody._defaultCrosshairPrefab = TemplarSwapWeapon.crosshair1;
			}
			else
			{
				bool flag15 = TemplarSwapWeapon.currentWeapon == 1;
				bool flag16 = flag15;
				if (flag16)
				{
					base.characterBody._defaultCrosshairPrefab = TemplarSwapWeapon.crosshair2;
				}
				else
				{
					bool flag17 = TemplarSwapWeapon.currentWeapon == 2;
					bool flag18 = flag17;
					if (flag18)
					{
						base.characterBody._defaultCrosshairPrefab = TemplarSwapWeapon.crosshair3;
					}
					else
					{
						bool flag19 = TemplarSwapWeapon.currentWeapon == 3;
						bool flag20 = flag19;
						if (flag20)
						{
							base.characterBody._defaultCrosshairPrefab = TemplarSwapWeapon.crosshair4;
						}
					}
				}
			}
			Util.PlaySound(FindItem.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			bool flag21 = this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
			bool flag22 = flag21;
			if (flag22)
			{
				this.swapInstance = UnityEngine.Object.Instantiate<GameObject>(LoomingPresence.blinkPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.swapInstance.transform.parent = this.muzzleTransform;
			}
			this.duration = TemplarSwapWeapon.baseDuration / this.attackSpeedStat;
		}

		public override void OnExit()
		{
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			bool flag2 = flag;
			if (flag2)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		public static float baseDuration = 0.35f;

		public static int currentWeapon;

		public static GameObject crosshair1 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/BanditCrosshair");

		public static GameObject crosshair2 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/StandardCrosshair");

		public static GameObject crosshair3 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/BanditCrosshair");

		public static GameObject crosshair4 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");

		private float duration;

		private Transform muzzleTransform;

		private GameObject swapInstance;
	}
}

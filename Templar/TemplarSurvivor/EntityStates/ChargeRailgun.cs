using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Toolbot;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class ChargeRailgun : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SMGCrosshair");
			this.minChargeDuration = 0.75f * (ChargeSpear.baseMinChargeDuration / this.attackSpeedStat);
			this.chargeDuration = 0.75f * (ChargeSpear.baseChargeDuration / this.attackSpeedStat);
			Util.PlaySound(MinigunSpinUp.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			bool flag = this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
			bool flag2 = flag;
			if (flag2)
			{
				this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.chargeInstance.transform.parent = this.muzzleTransform;
				ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
				bool flag3 = component;
				bool flag4 = flag3;
				if (flag4)
				{
					component.newDuration = this.minChargeDuration;
				}
			}
		}

		public override void OnExit()
		{
			base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			bool flag = this.chargeInstance;
			bool flag2 = flag;
			if (flag2)
			{
				EntityState.Destroy(this.chargeInstance);
			}
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.OnExit();
		}

		public override void Update()
		{
			base.Update();
			base.characterBody.SetSpreadBloom(1f - base.age / this.chargeDuration, true);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
			float num = base.fixedAge - this.chargeDuration;
			bool flag = num >= 0f;
			bool flag2 = flag;
			if (flag2)
			{
				float perfectChargeWindow = ChargeSpear.perfectChargeWindow;
			}
			float charge = this.chargeDuration / base.fixedAge;
			bool isAuthority = base.isAuthority;
			bool flag3 = isAuthority;
			if (flag3)
			{
				bool flag4 = !this.released && (!base.inputBank || !base.inputBank.skill1.down);
				bool flag5 = flag4;
				if (flag5)
				{
					this.released = true;
				}
				bool flag6 = this.released && base.fixedAge >= this.minChargeDuration;
				bool flag7 = flag6;
				if (flag7)
				{
					this.outer.SetNextState(new FireSpear
					{
						charge = charge
					});
				}
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		private float minChargeDuration;

		private float chargeDuration;

		private GameObject chargeInstance;

		private bool released;

		private Transform muzzleTransform;
	}
}

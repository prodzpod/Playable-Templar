using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarMinigunSpinUp : TemplarMinigunState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.SetSpreadBloom(2f, false);
			base.characterBody.crosshairPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
			this.duration = MinigunSpinUp.baseDuration / this.attackSpeedStat;
			Util.PlaySound(MinigunSpinUp.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
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
					component.newDuration = this.duration;
				}
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			bool flag2 = flag;
			if (flag2)
			{
				this.outer.SetNextState(new TemplarMinigunFire());
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			bool flag2 = flag;
			if (flag2)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		private GameObject chargeInstance;

		private float duration;
	}
}

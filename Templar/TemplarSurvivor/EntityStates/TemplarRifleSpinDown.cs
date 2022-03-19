using EntityStates.ClayBruiser.Weapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarRifleSpinDown : TemplarRifleState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			this.duration = MinigunSpinDown.baseDuration * 0.1f / this.attackSpeedStat;
			Util.PlayAttackSpeedSound(MinigunSpinDown.sound, base.gameObject, this.attackSpeedStat);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
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

		private float duration;
	}
}

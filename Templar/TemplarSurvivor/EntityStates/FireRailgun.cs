using EntityStates;
using EntityStates.Toolbot;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class FireRailgun : GenericBulletBaseState
	{
		protected override void ModifyBullet(BulletAttack bulletAttack)
		{
			base.ModifyBullet(bulletAttack);
			bulletAttack.stopperMask = LayerIndex.world.mask;
			bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
		}

		protected override void FireBullet(Ray aimRay)
		{
			base.FireBullet(aimRay);
			base.characterBody.SetSpreadBloom(0.2f, false);
			base.AddRecoil(-0.6f * FireSpear.recoilAmplitude, -0.8f * FireSpear.recoilAmplitude, -0.1f * FireSpear.recoilAmplitude, 0.1f * FireSpear.recoilAmplitude);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		public float charge;
	}
}

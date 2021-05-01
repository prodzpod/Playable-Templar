using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarMinigunFire : TemplarMinigunState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (this.muzzleTransform && MinigunFire.muzzleVfxPrefab)
			{
				this.muzzleVfxTransform = UnityEngine.Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
			}
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			//this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;
			this.currentFireRate = TemplarMinigunFire.minFireRate;
			this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
			this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
			Util.PlaySound(MinigunFire.startSound, base.gameObject);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			//this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;
			this.fireTimer -= Time.fixedDeltaTime;
			if (this.fireTimer <= 0f)
			{
				this.attackSpeedStat = base.characterBody.attackSpeed;
				float num = MinigunFire.baseFireInterval / this.attackSpeedStat / this.currentFireRate;
				this.fireTimer += num;
				this.OnFireShared();
			}

			if (base.isAuthority && !base.skillButtonState.down)
			{
				this.outer.SetNextState(new TemplarMinigunSpinDown());
			}
		}

		private void UpdateCrits()
		{
			this.critStat = base.characterBody.crit;
			if (this.lastCritCheck.timeSince >= 0.15f)
			{
				this.lastCritCheck = Run.FixedTimeStamp.now;
				if (base.RollCrit())
				{
					this.critEndTime = Run.FixedTimeStamp.now + 0.25f;
				}
			}
		}

		public override void OnExit()
		{
			Util.PlaySound(MinigunFire.endSound, base.gameObject);
			if (this.muzzleVfxTransform)
			{
				EntityState.Destroy(this.muzzleVfxTransform.gameObject);
				this.muzzleVfxTransform = null;
			}
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.OnExit();
		}

		private void OnFireShared()
		{
			Util.PlaySound(MinigunFire.fireSound, base.gameObject);
			if (base.isAuthority)
			{
				this.OnFireAuthority();
			}
		}

		private void OnFireAuthority()
		{
			this.UpdateCrits();
			bool isCrit = !this.critEndTime.hasPassed;
			base.characterBody.AddSpreadBloom(0.25f);
			base.AddRecoil(-0.6f * TemplarMinigunFire.recoilAmplitude, -0.8f * TemplarMinigunFire.recoilAmplitude, -0.3f * TemplarMinigunFire.recoilAmplitude, 0.3f * TemplarMinigunFire.recoilAmplitude);
			this.currentFireRate = Mathf.Clamp(this.currentFireRate + TemplarMinigunFire.fireRateGrowth, TemplarMinigunFire.minFireRate, TemplarMinigunFire.maxFireRate);
			float damage = TemplarMinigunFire.baseDamageCoefficient * this.damageStat;
			float force = TemplarMinigunFire.baseForce;
			float procCoefficient = TemplarMinigunFire.baseProcCoefficient;
			Ray aimRay = base.GetAimRay();
			new BulletAttack
			{
				bulletCount = (uint)MinigunFire.baseBulletCount,
				aimVector = aimRay.direction,
				origin = aimRay.origin,
				damage = damage,
				damageColorIndex = DamageColorIndex.Default,
				damageType = DamageType.Generic,
				falloffModel = BulletAttack.FalloffModel.DefaultBullet,
				maxDistance = MinigunFire.bulletMaxDistance,
				force = force,
				hitMask = LayerIndex.CommonMasks.bullet,
				minSpread = MinigunFire.bulletMinSpread,
				maxSpread = MinigunFire.bulletMaxSpread * 1.5f,
				isCrit = isCrit,
				owner = base.gameObject,
				muzzleName = MinigunState.muzzleName,
				smartCollision = false,
				procChainMask = default(ProcChainMask),
				procCoefficient = procCoefficient,
				radius = 0f,
				sniper = false,
				stopperMask = LayerIndex.CommonMasks.bullet,
				weapon = null,
				tracerEffectPrefab = MinigunFire.bulletTracerEffectPrefab,
				spreadPitchScale = 1f,
				spreadYawScale = 1f,
				queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
				hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
				HitEffectNormal = MinigunFire.bulletHitEffectNormal
			}.Fire();
		}


		public static float baseDamageCoefficient = Templar.minigunDamageCoefficient.Value;

		public static float baseForce = Templar.minigunForce.Value;

		public static float baseProcCoefficient = Templar.minigunProcCoefficient.Value;

		public static float recoilAmplitude = 2f;

		public static float minFireRate = Templar.minigunMinFireRate.Value;

		public static float maxFireRate = Templar.minigunMaxFireRate.Value;

		public static float fireRateGrowth = Templar.minigunFireRateGrowth.Value;

		private float fireTimer;

		private Transform muzzleVfxTransform;

		private float baseFireRate;

		private float baseBulletsPerSecond;

		private Run.FixedTimeStamp critEndTime;

		private Run.FixedTimeStamp lastCritCheck;

		private float currentFireRate;
	}
}

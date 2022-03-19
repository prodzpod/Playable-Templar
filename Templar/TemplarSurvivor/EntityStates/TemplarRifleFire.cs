using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Squid.SquidWeapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarRifleFire : TemplarRifleState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			bool flag = this.muzzleTransform && MinigunFire.muzzleVfxPrefab;
			bool flag2 = flag;
			if (flag2)
			{
				this.muzzleVfxTransform = UnityEngine.Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
			}
			base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 2f * this.baseFireRate;
			this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
			this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
			Util.PlaySound(MinigunFire.startSound, base.gameObject);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
		}

		private void UpdateCrits()
		{
			this.critStat = base.characterBody.crit;
			bool flag = this.lastCritCheck.timeSince >= 0.1f;
			bool flag2 = flag;
			if (flag2)
			{
				this.lastCritCheck = Run.FixedTimeStamp.now;
				bool flag3 = base.RollCrit();
				bool flag4 = flag3;
				if (flag4)
				{
					this.critEndTime = Run.FixedTimeStamp.now + 0.4f;
				}
			}
		}

		public override void OnExit()
		{
			Util.PlaySound(MinigunFire.endSound, base.gameObject);
			bool flag = this.muzzleVfxTransform;
			bool flag2 = flag;
			if (flag2)
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
			bool isAuthority = base.isAuthority;
			bool flag = isAuthority;
			if (flag)
			{
				this.OnFireAuthority();
			}
		}

		private void OnFireAuthority()
		{
			this.UpdateCrits();
			bool isCrit = !this.critEndTime.hasPassed;
			base.AddRecoil(-0.5f * TemplarRifleFire.recoilAmplitude, -0.5f * TemplarRifleFire.recoilAmplitude, -0.5f * TemplarRifleFire.recoilAmplitude, 0.5f * TemplarRifleFire.recoilAmplitude);
			float damage = TemplarRifleFire.baseDamageCoefficient * this.damageStat;
			float force = TemplarRifleFire.baseForcePerSecond / this.baseBulletsPerSecond;
			float procCoefficient = TemplarRifleFire.baseProcCoefficient;
			Ray aimRay = base.GetAimRay();
			new BulletAttack
			{
				bulletCount = (uint)MinigunFire.baseBulletCount,
				aimVector = aimRay.direction,
				origin = aimRay.origin,
				damage = damage,
				damageColorIndex = DamageColorIndex.Default,
				damageType = DamageType.ClayGoo,
				falloffModel = BulletAttack.FalloffModel.DefaultBullet,
				maxDistance = MinigunFire.bulletMaxDistance * 0.75f,
				force = force,
				hitMask = LayerIndex.CommonMasks.bullet,
				minSpread = MinigunFire.bulletMinSpread,
				maxSpread = MinigunFire.bulletMaxSpread,
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
				spreadPitchScale = 0.35f,
				spreadYawScale = 0.35f,
				queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
				hitEffectPrefab = FireSpine.hitEffectPrefab,
				HitEffectNormal = FireSpine.hitEffectPrefab
			}.Fire();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.baseFireRate = 1f / (MinigunFire.baseFireInterval * 1f);
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 1f * this.baseFireRate * 1f;
			this.fireTimer -= Time.fixedDeltaTime;
			bool flag = this.fireTimer <= 0f;
			bool flag2 = flag;
			if (flag2)
			{
				this.attackSpeedStat = base.characterBody.attackSpeed;
				float num = TemplarRifleFire.fireRate * (MinigunFire.baseFireInterval / this.attackSpeedStat);
				this.fireTimer += num;
				this.OnFireShared();
			}
			bool flag3 = base.isAuthority && !base.skillButtonState.down;
			bool flag4 = flag3;
			if (flag4)
			{
				this.outer.SetNextState(new TemplarRifleSpinDown());
			}
		}

		public static float baseDamageCoefficient = Templar.rifleDamageCoefficient.Value;

		public static float baseForcePerSecond = Templar.rifleForce.Value;

		public static float baseProcCoefficient = Templar.rifleProcCoefficient.Value;

		public static float fireRate = Templar.rifleFireRate.Value;

		public static float recoilAmplitude = 0.75f;

		private float fireTimer;

		private Transform muzzleVfxTransform;

		private float baseFireRate;

		private float baseBulletsPerSecond;

		private Run.FixedTimeStamp critEndTime;

		private Run.FixedTimeStamp lastCritCheck;
	}
}

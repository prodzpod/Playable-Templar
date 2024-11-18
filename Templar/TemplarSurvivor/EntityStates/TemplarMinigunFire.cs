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
            if (muzzleTransform && MinigunFire.muzzleVfxPrefab)
            {
                muzzleVfxTransform = UnityEngine.Object.Instantiate(MinigunFire.muzzleVfxPrefab, muzzleTransform).transform;
            }
            baseFireRate = 1f / MinigunFire.baseFireInterval;
            //this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;
            currentFireRate = minFireRate;
            critEndTime = Run.FixedTimeStamp.negativeInfinity;
            lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
            Util.PlaySound(MinigunFire.startSound, gameObject);
            PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            baseFireRate = 1f / MinigunFire.baseFireInterval;
            //this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;
            fireTimer -= Time.fixedDeltaTime;
            if (fireTimer <= 0f)
            {
                attackSpeedStat = characterBody.attackSpeed;
                float num = MinigunFire.baseFireInterval / attackSpeedStat / currentFireRate;
                fireTimer += num;
                OnFireShared();
            }

            if (isAuthority && !skillButtonState.down)
            {
                outer.SetNextState(new TemplarMinigunSpinDown());
            }
        }

        private void UpdateCrits()
        {
            critStat = characterBody.crit;
            if (lastCritCheck.timeSince >= 0.15f)
            {
                lastCritCheck = Run.FixedTimeStamp.now;
                if (RollCrit())
                {
                    critEndTime = Run.FixedTimeStamp.now + 0.25f;
                }
            }
        }

        public override void OnExit()
        {
            Util.PlaySound(MinigunFire.endSound, gameObject);
            if (muzzleVfxTransform)
            {
                Destroy(muzzleVfxTransform.gameObject);
                muzzleVfxTransform = null;
            }
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            base.OnExit();
        }

        private void OnFireShared()
        {
            Util.PlaySound(MinigunFire.fireSound, gameObject);
            if (isAuthority)
            {
                OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            UpdateCrits();
            bool isCrit = !critEndTime.hasPassed;
            characterBody.AddSpreadBloom(0.25f);
            AddRecoil(-0.6f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
            currentFireRate = Mathf.Clamp(currentFireRate + fireRateGrowth, minFireRate, maxFireRate);
            float damage = baseDamageCoefficient * damageStat;
            float force = baseForce;
            float procCoefficient = baseProcCoefficient;
            Ray aimRay = GetAimRay();
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
                owner = gameObject,
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

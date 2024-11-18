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
            GetModelAnimator().SetBool("WeaponIsReady", true);
            bool flag = muzzleTransform && MinigunFire.muzzleVfxPrefab;
            bool flag2 = flag;
            if (flag2)
            {
                muzzleVfxTransform = UnityEngine.Object.Instantiate(MinigunFire.muzzleVfxPrefab, muzzleTransform).transform;
            }
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
            baseFireRate = 1f / MinigunFire.baseFireInterval;
            baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 2f * baseFireRate;
            critEndTime = Run.FixedTimeStamp.negativeInfinity;
            lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
            Util.PlaySound(MinigunFire.startSound, gameObject);
            PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
        }

        private void UpdateCrits()
        {
            critStat = characterBody.crit;
            bool flag = lastCritCheck.timeSince >= 0.1f;
            bool flag2 = flag;
            if (flag2)
            {
                lastCritCheck = Run.FixedTimeStamp.now;
                bool flag3 = RollCrit();
                bool flag4 = flag3;
                if (flag4)
                {
                    critEndTime = Run.FixedTimeStamp.now + 0.4f;
                }
            }
        }

        public override void OnExit()
        {
            Util.PlaySound(MinigunFire.endSound, gameObject);
            bool flag = muzzleVfxTransform;
            bool flag2 = flag;
            if (flag2)
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
            bool isAuthority = base.isAuthority;
            bool flag = isAuthority;
            if (flag)
            {
                OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            UpdateCrits();
            bool isCrit = !critEndTime.hasPassed;
            AddRecoil(-0.5f * recoilAmplitude, -0.5f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);
            float damage = baseDamageCoefficient * damageStat;
            float force = baseForcePerSecond / baseBulletsPerSecond;
            float procCoefficient = baseProcCoefficient;
            Ray aimRay = GetAimRay();
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
            baseFireRate = 1f / (MinigunFire.baseFireInterval * 1f);
            baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 1f * baseFireRate * 1f;
            fireTimer -= Time.fixedDeltaTime;
            bool flag = fireTimer <= 0f;
            bool flag2 = flag;
            if (flag2)
            {
                attackSpeedStat = characterBody.attackSpeed;
                float num = fireRate * (MinigunFire.baseFireInterval / attackSpeedStat);
                fireTimer += num;
                OnFireShared();
            }
            bool flag3 = isAuthority && !skillButtonState.down;
            bool flag4 = flag3;
            if (flag4)
            {
                outer.SetNextState(new TemplarRifleSpinDown());
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

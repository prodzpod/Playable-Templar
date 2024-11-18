using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Sniper.SniperWeapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarShotgun : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.skillLocator.primary.rechargeStopwatch = 0f;
            AddRecoil(-1f * recoilAmplitude, -2f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);
            maxDuration = baseMaxDuration / attackSpeedStat;
            minDuration = baseMinDuration / attackSpeedStat;
            Ray aimRay = GetAimRay();
            StartAimMode(2f, false);
            Util.PlayAttackSpeedSound(FireRifle.attackSoundString, gameObject, 0.8f);
            GetModelAnimator().SetBool("WeaponIsReady", true);
            PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
            string muzzleName = MinigunState.muzzleName;
            bool flag = MinigunFire.muzzleVfxPrefab;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                EffectManager.SimpleMuzzleFlash(MinigunFire.muzzleVfxPrefab, gameObject, muzzleName, false);
            }
            bool isAuthority = base.isAuthority;
            bool flag4 = isAuthority;
            bool flag5 = flag4;
            if (flag5)
            {
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    bulletCount = 1U,
                    procCoefficient = procCoefficient,
                    damageType = DamageType.Generic,
                    damage = damageCoefficient * damageStat,
                    force = force,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    tracerEffectPrefab = tracerEffectPrefab,
                    muzzleName = muzzleName,
                    hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
                    isCrit = Util.CheckRoll(critStat, characterBody.master),
                    HitEffectNormal = false,
                    radius = bulletRadius,
                    smartCollision = true,
                    stopperMask = LayerIndex.world.mask
                }.Fire();
                bool flag6 = pelletCount > 1U;
                bool flag7 = flag6;
                bool flag8 = flag7;
                if (flag8)
                {
                    new BulletAttack
                    {
                        owner = gameObject,
                        weapon = gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = spreadBloomValue / (pelletCount - 1f),
                        maxSpread = spreadBloomValue,
                        bulletCount = pelletCount - 1U,
                        procCoefficient = procCoefficient,
                        damageType = DamageType.Generic,
                        damage = damageCoefficient * damageStat,
                        force = force,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        tracerEffectPrefab = tracerEffectPrefab,
                        muzzleName = muzzleName,
                        hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
                        isCrit = Util.CheckRoll(critStat, characterBody.master),
                        HitEffectNormal = false,
                        radius = bulletRadius,
                        smartCollision = true,
                        stopperMask = LayerIndex.world.mask
                    }.Fire();
                }
                characterBody.AddSpreadBloom(spreadBloomValue);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            GetModelAnimator().SetBool("WeaponIsReady", false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            buttonReleased |= !inputBank.skill1.down;
            bool flag = fixedAge >= maxDuration && isAuthority;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            bool flag = buttonReleased && fixedAge >= minDuration;
            bool flag2 = flag;
            bool flag3 = flag2;
            InterruptPriority result;
            if (flag3)
            {
                result = InterruptPriority.Any;
            }
            else
            {
                result = InterruptPriority.Skill;
            }
            return result;
        }

        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoBoost");

        public static float damageCoefficient = Templar.blunderbussDamageCoefficient.Value;

        public static float force = 5f;

        public static float bulletRadius = 1.5f;

        public static float baseMaxDuration = 0.75f;

        public static float baseMinDuration = 0.5f;

        public static float recoilAmplitude = 5f;

        public static float spreadBloomValue = Templar.blunderbussSpread.Value;

        public static uint pelletCount = (uint)Templar.blunderbussPelletCount.Value;

        public static float procCoefficient = Templar.blunderbussProcCoefficient.Value;

        private float maxDuration;

        private float minDuration;

        private bool buttonReleased;
    }
}

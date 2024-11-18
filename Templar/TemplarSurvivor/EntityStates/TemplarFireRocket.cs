using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarFireRocket : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            StartAimMode(2f, false);
            jelly = false;
            rocketPrefab = Templar.templarRocket;
            effectPrefab = FireMegaFireball.muzzleflashEffectPrefab;
            rocketPrefab = Templar.templarRocket;
            AddRecoil(-1f * recoilAmplitude, -2f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);
            duration = baseDuration / attackSpeedStat;
            fireDuration = baseFireDuration / attackSpeedStat;
            Util.PlaySound(FireMegaFireball.attackString, gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            GetModelAnimator().SetBool("WeaponIsReady", false);
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            string muzzleName = MinigunState.muzzleName;
            bool isAuthority = base.isAuthority;
            bool flag = isAuthority;
            if (flag)
            {
                int num = Mathf.FloorToInt(fixedAge / fireDuration * (float)projectileCount);
                bool flag2 = projectilesFired <= num && projectilesFired < projectileCount;
                bool flag3 = flag2;
                if (flag3)
                {
                    EffectManager.SimpleMuzzleFlash(effectPrefab, gameObject, muzzleName, false);
                    Ray aimRay = GetAimRay();
                    float speedOverride = FireMegaFireball.projectileSpeed * 2f;
                    float bonusYaw = (float)Mathf.FloorToInt((float)projectilesFired - (float)(projectileCount - 1) / 2f) / (float)(projectileCount - 1) * totalYawSpread;
                    bonusYaw = 0f;
                    Vector3 forward = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, bonusYaw, 0f);
                    ProjectileManager.instance.FireProjectile(rocketPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(forward), gameObject, damageStat * damageCoefficient, force, Util.CheckRoll(critStat, characterBody.master), DamageColorIndex.Default, null, speedOverride);
                    projectilesFired++;
                }
            }
            bool flag4 = fixedAge >= duration && base.isAuthority;
            bool flag5 = flag4;
            if (flag5)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public static GameObject rocketPrefab;

        public static GameObject effectPrefab;

        public static int projectileCount = 1;

        public static float totalYawSpread = 1f;

        public static float baseDuration = 0.25f;

        public static float baseFireDuration = 0.2f;

        public static float damageCoefficient = Templar.bazookaDamageCoefficient.Value;

        public static float force = 25f;

        public static float recoilAmplitude = 10f;

        private float duration;

        private float fireDuration;

        private int projectilesFired;

        private bool jelly;
    }
}

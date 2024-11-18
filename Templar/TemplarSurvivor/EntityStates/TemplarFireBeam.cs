using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.GolemMonster;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarFireBeam : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            modifiedAimRay = GetAimRay();
            modifiedAimRay.direction = laserDirection;
            animator = GetModelAnimator();
            Transform modelTransform = GetModelTransform();
            Ray aimRay = GetAimRay();
            StartAimMode(2f, false);
            float num = recoilAmplitude / attackSpeedStat;
            AddRecoil(-1f * num, -1.5f * num, -0.25f * num, 0.25f * num);
            characterBody.AddSpreadBloom(bloom);
            Util.PlayAttackSpeedSound(FireLaser.attackSoundString, gameObject, 2f);
            string muzzleName = MinigunState.muzzleName;
            bool flag = FireLaser.effectPrefab;
            bool flag2 = flag;
            if (flag2)
            {
                EffectManager.SimpleMuzzleFlash(FireLaser.effectPrefab, gameObject, muzzleName, false);
            }
            bool isAuthority = base.isAuthority;
            bool flag3 = isAuthority;
            if (flag3)
            {
                float num2 = 1000f;
                Vector3 vector = modifiedAimRay.origin + modifiedAimRay.direction * num2;
                RaycastHit raycastHit;
                bool flag4 = Physics.Raycast(modifiedAimRay, out raycastHit, num2, LayerIndex.world.mask | LayerIndex.defaultLayer.mask | LayerIndex.entityPrecise.mask);
                bool flag5 = flag4;
                if (flag5)
                {
                    vector = raycastHit.point;
                }
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    bulletCount = (uint)bulletCount,
                    procCoefficient = procCoefficient,
                    damageType = DamageType.BypassOneShotProtection,
                    damage = damageCoefficient * damageStat,
                    force = force,
                    falloffModel = BulletAttack.FalloffModel.None,
                    tracerEffectPrefab = FireLaser.tracerEffectPrefab,
                    muzzleName = MinigunState.muzzleName,
                    hitEffectPrefab = FireLaser.hitEffectPrefab,
                    isCrit = Util.CheckRoll(critStat, characterBody.master),
                    HitEffectNormal = false,
                    radius = radius,
                    smartCollision = true,
                    stopperMask = LayerIndex.world.mask
                }.Fire();
            }
            PlayCrossfade("Gesture, Additive", "FireMinigun", 0.75f * duration);
        }

        public override void OnExit()
        {
            base.OnExit();
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            animator.SetBool("WeaponIsReady", false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bool flag = fixedAge >= duration && isAuthority;
            bool flag2 = flag;
            if (flag2)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public static float damageCoefficient = 8f;

        public static float procCoefficient = 1f;

        public static float force = 500f;

        public static float minSpread = 0f;

        public static float maxSpread = 0f;

        public static int bulletCount = 1;

        public static float baseDuration = 0.5f;

        public static float radius = 1.25f;

        public float recoilAmplitude = 10f;

        public float bloom = 6f;

        public Vector3 laserDirection;

        private float duration;

        private Ray modifiedAimRay;

        private Animator animator;
    }
}

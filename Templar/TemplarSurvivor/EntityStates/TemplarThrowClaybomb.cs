using EntityStates;
using EntityStates.ClayBoss;
using EntityStates.ClayBoss.ClayBossWeapon;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarThrowClaybomb : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireDuration = 0.2f * duration;
            characterBody.SetAimTimer(2f);
            animator = GetModelAnimator();
            muzzleName = MinigunState.muzzleName;
            animator.SetBool("WeaponIsReady", true);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void ThrowBomb()
        {
            bool flag = !hasFired;
            bool flag2 = flag;
            if (flag2)
            {
                hasFired = true;
                Util.PlaySound(FireBombardment.shootSoundString, gameObject);
                Ray aimRay = GetAimRay();
                EffectManager.SimpleMuzzleFlash(FireTarball.effectPrefab, gameObject, muzzleName, false);
                bool isAuthority = base.isAuthority;
                bool flag3 = isAuthority;
                if (flag3)
                {
                    ProjectileManager.instance.FireProjectile(Templar.templarGrenade, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), gameObject, damageCoefficient * damageStat, 0f, Util.CheckRoll(critStat, characterBody.master), DamageColorIndex.Default, null, throwForce);
                }
                animator.SetBool("WeaponIsReady", false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bool flag = fixedAge >= fireDuration;
            bool flag2 = flag;
            if (flag2)
            {
                ThrowBomb();
            }
            bool flag3 = fixedAge >= duration && isAuthority;
            bool flag4 = flag3;
            if (flag4)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public float damageCoefficient = 1f;

        public float baseDuration = 0.4f;

        public float throwForce = 85f;

        private float duration;

        private float fireDuration;

        private bool hasFired;

        private Animator animator;

        private string muzzleName;
    }
}

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
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
            duration = MinigunSpinDown.baseDuration * 0.1f / attackSpeedStat;
            Util.PlayAttackSpeedSound(MinigunSpinDown.sound, gameObject, attackSpeedStat);
            GetModelAnimator().SetBool("WeaponIsReady", false);
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

        private float duration;
    }
}

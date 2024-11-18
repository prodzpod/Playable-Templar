using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Toolbot;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class ChargeRailgun : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SMGCrosshair");
            minChargeDuration = 0.75f * (ChargeSpear.baseMinChargeDuration / attackSpeedStat);
            chargeDuration = 0.75f * (ChargeSpear.baseChargeDuration / attackSpeedStat);
            Util.PlaySound(MinigunSpinUp.sound, gameObject);
            GetModelAnimator().SetBool("WeaponIsReady", true);
            muzzleTransform = FindModelChild(MinigunState.muzzleName);
            bool flag = muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
            bool flag2 = flag;
            if (flag2)
            {
                chargeInstance = UnityEngine.Object.Instantiate(MinigunSpinUp.chargeEffectPrefab, muzzleTransform.position, muzzleTransform.rotation);
                chargeInstance.transform.parent = muzzleTransform;
                ScaleParticleSystemDuration component = chargeInstance.GetComponent<ScaleParticleSystemDuration>();
                bool flag3 = component;
                bool flag4 = flag3;
                if (flag4)
                {
                    component.newDuration = minChargeDuration;
                }
            }
        }

        public override void OnExit()
        {
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
            bool flag = chargeInstance;
            bool flag2 = flag;
            if (flag2)
            {
                Destroy(chargeInstance);
            }
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            GetModelAnimator().SetBool("WeaponIsReady", false);
            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
            characterBody.SetSpreadBloom(1f - age / chargeDuration, true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode(2f, false);
            float num = fixedAge - chargeDuration;
            bool flag = num >= 0f;
            bool flag2 = flag;
            if (flag2)
            {
                float perfectChargeWindow = ChargeSpear.perfectChargeWindow;
            }
            float charge = chargeDuration / fixedAge;
            bool isAuthority = base.isAuthority;
            bool flag3 = isAuthority;
            if (flag3)
            {
                bool flag4 = !released && (!inputBank || !inputBank.skill1.down);
                bool flag5 = flag4;
                if (flag5)
                {
                    released = true;
                }
                bool flag6 = released && fixedAge >= minChargeDuration;
                bool flag7 = flag6;
                if (flag7)
                {
                    outer.SetNextState(new FireSpear
                    {
                        charge = charge
                    });
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private float minChargeDuration;

        private float chargeDuration;

        private GameObject chargeInstance;

        private bool released;

        private Transform muzzleTransform;
    }
}

using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarMinigunSpinUp : TemplarMinigunState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetSpreadBloom(2f, false);
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
            duration = MinigunSpinUp.baseDuration / attackSpeedStat;
            Util.PlaySound(MinigunSpinUp.sound, gameObject);
            GetModelAnimator().SetBool("WeaponIsReady", true);
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
                    component.newDuration = duration;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bool flag = fixedAge >= duration && isAuthority;
            bool flag2 = flag;
            if (flag2)
            {
                outer.SetNextState(new TemplarMinigunFire());
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            bool flag = chargeInstance;
            bool flag2 = flag;
            if (flag2)
            {
                Destroy(chargeInstance);
            }
        }

        private GameObject chargeInstance;

        private float duration;
    }
}

using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarChargeMiniRocket : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            UnityEngine.Object modelAnimator = GetModelAnimator();
            Transform modelTransform = GetModelTransform();
            GetModelAnimator().SetBool("WeaponIsReady", true);
            PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/ToolbotGrenadeLauncherCrosshair");
            Util.PlayAttackSpeedSound(ChargeMegaFireball.attackString, gameObject, attackSpeedStat);
            bool flag = modelTransform;
            bool flag2 = flag;
            if (flag2)
            {
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                bool flag3 = component;
                bool flag4 = flag3;
                if (flag4)
                {
                    Transform transform = component.FindChild(MinigunState.muzzleName);
                    bool flag5 = transform && ChargeMegaFireball.chargeEffectPrefab;
                    bool flag6 = flag5;
                    if (flag6)
                    {
                        chargeInstance = UnityEngine.Object.Instantiate(ChargeMegaFireball.chargeEffectPrefab, transform.position, transform.rotation);
                        chargeInstance.transform.parent = transform;
                        ScaleParticleSystemDuration component2 = chargeInstance.GetComponent<ScaleParticleSystemDuration>();
                        bool flag7 = component2;
                        bool flag8 = flag7;
                        if (flag8)
                        {
                            component2.newDuration = duration;
                        }
                    }
                }
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

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode(2f, false);
            bool flag = fixedAge >= duration && isAuthority;
            bool flag2 = flag;
            if (flag2)
            {
                TemplarFireMiniRocket nextState = new();
                outer.SetNextState(nextState);
            }
        }

        protected ref InputBankTest.ButtonState skillButtonState
        {
            get
            {
                return ref inputBank.skill1;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public static float baseDuration = 1.25f;

        private float duration;

        private GameObject chargeInstance;
    }
}

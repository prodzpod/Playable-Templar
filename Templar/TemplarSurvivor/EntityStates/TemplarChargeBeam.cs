using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.GolemMonster;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarChargeBeam : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            Transform modelTransform = GetModelTransform();
            animator = GetModelAnimator();
            StartAimMode(duration + 2f, false);
            animator.SetBool("WeaponIsReady", true);
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
            chargePlayID = Util.PlayAttackSpeedSound(ChargeLaser.attackSoundString, gameObject, attackSpeedStat);
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
                    bool flag5 = transform;
                    bool flag6 = flag5;
                    if (flag6)
                    {
                        bool flag7 = ChargeLaser.effectPrefab;
                        bool flag8 = flag7;
                        if (flag8)
                        {
                            chargeEffect = UnityEngine.Object.Instantiate(ChargeLaser.effectPrefab, transform.position, transform.rotation);
                            chargeEffect.transform.parent = transform;
                            ScaleParticleSystemDuration component2 = chargeEffect.GetComponent<ScaleParticleSystemDuration>();
                            bool flag9 = component2;
                            bool flag10 = flag9;
                            if (flag10)
                            {
                                component2.newDuration = duration;
                            }
                        }
                        bool flag11 = ChargeLaser.laserPrefab;
                        bool flag12 = flag11;
                        if (flag12)
                        {
                            laserEffect = UnityEngine.Object.Instantiate(ChargeLaser.laserPrefab, transform.position, transform.rotation);
                            laserEffect.transform.parent = transform;
                            laserLineComponent = laserEffect.GetComponent<LineRenderer>();
                        }
                    }
                }
            }
            bool flag13 = characterBody;
            bool flag14 = flag13;
            if (flag14)
            {
                characterBody.SetAimTimer(duration);
            }
            flashTimer = 0f;
            laserOn = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            bool flag = chargeEffect;
            bool flag2 = flag;
            if (flag2)
            {
                Destroy(chargeEffect);
            }
            bool flag3 = laserEffect;
            bool flag4 = flag3;
            if (flag4)
            {
                Destroy(laserEffect);
            }
        }

        public override void Update()
        {
            base.Update();
            bool flag = laserEffect && laserLineComponent;
            bool flag2 = flag;
            if (flag2)
            {
                float num = 1000f;
                Ray aimRay = GetAimRay();
                Vector3 position = laserEffect.transform.parent.position;
                Vector3 point = aimRay.GetPoint(num);
                laserDirection = point - position;
                RaycastHit raycastHit;
                bool flag3 = Physics.Raycast(aimRay, out raycastHit, num, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
                bool flag4 = flag3;
                if (flag4)
                {
                    point = raycastHit.point;
                }
                laserLineComponent.SetPosition(0, position);
                laserLineComponent.SetPosition(1, point);
                bool flag5 = duration - age > 0.5f;
                bool flag6 = flag5;
                float num2;
                if (flag6)
                {
                    num2 = age / duration;
                }
                else
                {
                    flashTimer -= Time.deltaTime;
                    bool flag7 = flashTimer <= 0f;
                    bool flag8 = flag7;
                    if (flag8)
                    {
                        laserOn = !laserOn;
                        flashTimer = 0.0333333351f;
                    }
                    num2 = (laserOn ? 1f : 0f);
                }
                num2 *= laserMaxWidth;
                laserLineComponent.startWidth = num2;
                laserLineComponent.endWidth = num2;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bool flag = fixedAge >= duration && isAuthority;
            bool flag2 = flag;
            if (flag2)
            {
                TemplarFireBeam templarFireBeam = new();
                templarFireBeam.laserDirection = laserDirection;
                outer.SetNextState(templarFireBeam);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public static float baseDuration = 1f;

        public static float laserMaxWidth = 0.2f;

        private float duration;

        private uint chargePlayID;

        private GameObject chargeEffect;

        private GameObject laserEffect;

        private LineRenderer laserLineComponent;

        private Vector3 laserDirection;

        private Vector3 visualEndPosition;

        private float flashTimer;

        private bool laserOn;

        private Animator animator;
    }
}

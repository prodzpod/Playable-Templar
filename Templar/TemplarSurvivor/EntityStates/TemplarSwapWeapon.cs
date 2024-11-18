using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.ParentMonster;
using EntityStates.ScavMonster;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarSwapWeapon : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            SkillDef skillDef = characterBody.skillLocator.primary.skillDef;
            bool flag = skillDef.skillNameToken == "TEMPLAR_PRIMARY_MINIGUN_NAME";
            bool flag2 = flag;
            if (flag2)
            {
                currentWeapon = 0;
            }
            else
            {
                bool flag3 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
                bool flag4 = flag3;
                if (flag4)
                {
                    currentWeapon = 1;
                }
                else
                {
                    bool flag5 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_RAILGUN_NAME";
                    bool flag6 = flag5;
                    if (flag6)
                    {
                        currentWeapon = 2;
                    }
                    else
                    {
                        bool flag7 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_BAZOOKA_NAME";
                        bool flag8 = flag7;
                        if (flag8)
                        {
                            currentWeapon = 2;
                        }
                        else
                        {
                            bool flag9 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
                            bool flag10 = flag9;
                            if (flag10)
                            {
                                currentWeapon = 3;
                            }
                        }
                    }
                }
            }
            currentWeapon++;
            bool flag11 = currentWeapon > 3;
            bool flag12 = flag11;
            if (flag12)
            {
                currentWeapon = 0;
            }
            characterBody.skillLocator.primary.SetBaseSkill(characterBody.skillLocator.primary.skillFamily.variants[currentWeapon].skillDef);
            bool flag13 = currentWeapon == 0;
            bool flag14 = flag13;
            if (flag14)
            {
                characterBody._defaultCrosshairPrefab = crosshair1;
            }
            else
            {
                bool flag15 = currentWeapon == 1;
                bool flag16 = flag15;
                if (flag16)
                {
                    characterBody._defaultCrosshairPrefab = crosshair2;
                }
                else
                {
                    bool flag17 = currentWeapon == 2;
                    bool flag18 = flag17;
                    if (flag18)
                    {
                        characterBody._defaultCrosshairPrefab = crosshair3;
                    }
                    else
                    {
                        bool flag19 = currentWeapon == 3;
                        bool flag20 = flag19;
                        if (flag20)
                        {
                            characterBody._defaultCrosshairPrefab = crosshair4;
                        }
                    }
                }
            }
            Util.PlaySound(FindItem.sound, gameObject);
            GetModelAnimator().SetBool("WeaponIsReady", true);
            muzzleTransform = FindModelChild(MinigunState.muzzleName);
            bool flag21 = muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
            bool flag22 = flag21;
            if (flag22)
            {
                swapInstance = UnityEngine.Object.Instantiate(LoomingPresence.blinkPrefab, muzzleTransform.position, muzzleTransform.rotation);
                swapInstance.transform.parent = muzzleTransform;
            }
            duration = baseDuration / attackSpeedStat;
        }

        public override void OnExit()
        {
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            GetModelAnimator().SetBool("WeaponIsReady", false);
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
            base.OnExit();
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
            return InterruptPriority.Skill;
        }

        public static float baseDuration = 0.35f;

        public static int currentWeapon;

        public static GameObject crosshair1 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/BanditCrosshair");

        public static GameObject crosshair2 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/StandardCrosshair");

        public static GameObject crosshair3 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/BanditCrosshair");

        public static GameObject crosshair4 = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");

        private float duration;

        private Transform muzzleTransform;

        private GameObject swapInstance;
    }
}

using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;

using UnityEngine;
using UnityEngine.Networking;

namespace Templar
{
    public class TemplarFlamethrower : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            entryDuration = baseEntryDuration / attackSpeedStat;
            exitDuration = baseExitDuration / attackSpeedStat;
            animator = GetModelAnimator();
            Transform modelTransform = GetModelTransform();
            childLocator = modelTransform.GetComponent<ChildLocator>();
            animator.SetBool("WeaponIsReady", true);
            flamethrowerDuration = 5f;
            bool flag = NetworkServer.active && characterBody;
            bool flag2 = flag;
            if (flag2)
            {
                characterBody.AddBuff(Buffs.TemplArmorBuff);
            }
            float num = attackSpeedStat * tickFrequency;
            tickDamageCoefficient = damageCoefficientPerTick;
        }

        public override void OnExit()
        {
            Util.PlaySound(Flamebreath.endAttackSoundString, gameObject);
            bool flag = flamethrowerEffectInstance;
            bool flag2 = flag;
            if (flag2)
            {
                Destroy(flamethrowerEffectInstance.gameObject);
            }
            bool flag3 = NetworkServer.active && characterBody;
            bool flag4 = flag3;
            if (flag4)
            {
                characterBody.RemoveBuff(Buffs.TemplArmorBuff);
            }
            animator.SetBool("WeaponIsReady", false);
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            base.OnExit();
        }

        private void FireFlame(string muzzleString)
        {
            GetAimRay();
            bool flag = isAuthority && muzzleTransform;
            bool flag2 = flag;
            if (flag2)
            {
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = muzzleTransform.position,
                    aimVector = muzzleTransform.forward,
                    minSpread = 0f,
                    maxSpread = maxSpread,
                    damage = tickDamageCoefficient * damageStat,
                    force = force,
                    muzzleName = muzzleString,
                    hitEffectPrefab = Flamebreath.impactEffectPrefab,
                    isCrit = RollCrit(),
                    radius = radius,
                    falloffModel = BulletAttack.FalloffModel.None,
                    procCoefficient = procCoefficientPerTick,
                    maxDistance = maxDistance,
                    smartCollision = true,
                    damageType = DamageType.BypassOneShotProtection
                }.Fire();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            StartAimMode(0.5f, false);
            characterBody.isSprinting = false;
            bool flag = stopwatch >= entryDuration && stopwatch < entryDuration + flamethrowerDuration && !hasBegunFlamethrower;
            bool flag2 = flag;
            if (flag2)
            {
                hasBegunFlamethrower = true;
                Util.PlaySound(Flamebreath.startAttackSoundString, gameObject);
                bool flag3 = childLocator;
                bool flag4 = flag3;
                if (flag4)
                {
                    muzzleTransform = childLocator.FindChild(MinigunState.muzzleName);
                    flamethrowerEffectInstance = Object.Instantiate(Flamebreath.flamethrowerEffectPrefab, muzzleTransform).transform;
                    flamethrowerEffectInstance.transform.localPosition = Vector3.zero;
                    flamethrowerEffectInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = 2f;
                    foreach (ParticleSystem particleSystem in flamethrowerEffectInstance.GetComponentsInChildren<ParticleSystem>())
                    {
                        bool flag5 = particleSystem;
                        bool flag6 = flag5;
                        if (flag6)
                        {
                            var main = particleSystem.main;
                            main.loop = true;
                        }
                    }
                }
            }
            bool flag7 = stopwatch >= entryDuration && hasBegunFlamethrower && !inputBank.skill1.down;
            bool flag8 = flag7;
            if (flag8)
            {
                hasBegunFlamethrower = false;
                bool flag9 = flamethrowerEffectInstance;
                bool flag10 = flag9;
                if (flag10)
                {
                    Destroy(flamethrowerEffectInstance.gameObject);
                }
                outer.SetNextStateToMain();
            }
            else
            {
                bool flag11 = hasBegunFlamethrower;
                bool flag12 = flag11;
                if (flag12)
                {
                    flamethrowerStopwatch += Time.fixedDeltaTime;
                    bool flag13 = flamethrowerStopwatch > tickFrequency / attackSpeedStat;
                    bool flag14 = flag13;
                    if (flag14)
                    {
                        flamethrowerStopwatch -= tickFrequency / attackSpeedStat;
                        FireFlame(MinigunState.muzzleName);
                        PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
                    }
                    flamethrowerDuration = stopwatch + baseExitDuration;
                }
                else
                {
                    bool flag15 = flamethrowerEffectInstance;
                    bool flag16 = flag15;
                    if (flag16)
                    {
                        Destroy(flamethrowerEffectInstance.gameObject);
                    }
                }
                bool flag17 = stopwatch >= flamethrowerDuration + entryDuration && isAuthority;
                bool flag18 = flag17;
                if (flag18)
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public static float maxDistance = 20f;

        public static float radius = 1.5f;

        public static float baseEntryDuration = 0.1f;

        public static float baseExitDuration = 0.75f;

        public static float damageCoefficientPerTick = 2.5f;

        public static float procCoefficientPerTick = 0.4f;

        public static float tickFrequency = 0.25f;

        public static float force = 1f;

        public static float maxSpread = 0.25f;

        public static GameObject flamethrowerEffectPrefab;

        private float tickDamageCoefficient;

        private float flamethrowerStopwatch;

        private float stopwatch;

        private float entryDuration;

        private float exitDuration;

        private float flamethrowerDuration;

        private bool hasBegunFlamethrower;

        private ChildLocator childLocator;

        private Transform flamethrowerEffectInstance;

        private Transform muzzleTransform;

        private Animator animator;
    }
}

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
			this.stopwatch = 0f;
			this.entryDuration = TemplarFlamethrower.baseEntryDuration / this.attackSpeedStat;
			this.exitDuration = TemplarFlamethrower.baseExitDuration / this.attackSpeedStat;
			this.animator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			this.childLocator = modelTransform.GetComponent<ChildLocator>();
			this.animator.SetBool("WeaponIsReady", true);
			this.flamethrowerDuration = 5f;
			bool flag = NetworkServer.active && base.characterBody;
			bool flag2 = flag;
			if (flag2)
			{
				base.characterBody.AddBuff(Buffs.TemplArmorBuff);
			}
			float num = this.attackSpeedStat * TemplarFlamethrower.tickFrequency;
			this.tickDamageCoefficient = TemplarFlamethrower.damageCoefficientPerTick;
		}

		public override void OnExit()
		{
			Util.PlaySound(Flamebreath.endAttackSoundString, base.gameObject);
			bool flag = this.flamethrowerEffectInstance;
			bool flag2 = flag;
			if (flag2)
			{
				EntityState.Destroy(this.flamethrowerEffectInstance.gameObject);
			}
			bool flag3 = NetworkServer.active && base.characterBody;
			bool flag4 = flag3;
			if (flag4)
			{
				base.characterBody.RemoveBuff(Buffs.TemplArmorBuff);
			}
			this.animator.SetBool("WeaponIsReady", false);
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.OnExit();
		}

		private void FireFlame(string muzzleString)
		{
			base.GetAimRay();
			bool flag = base.isAuthority && this.muzzleTransform;
			bool flag2 = flag;
			if (flag2)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = this.muzzleTransform.position,
					aimVector = this.muzzleTransform.forward,
					minSpread = 0f,
					maxSpread = TemplarFlamethrower.maxSpread,
					damage = this.tickDamageCoefficient * this.damageStat,
					force = TemplarFlamethrower.force,
					muzzleName = muzzleString,
					hitEffectPrefab = Flamebreath.impactEffectPrefab,
					isCrit = base.RollCrit(),
					radius = TemplarFlamethrower.radius,
					falloffModel = BulletAttack.FalloffModel.None,
					procCoefficient = TemplarFlamethrower.procCoefficientPerTick,
					maxDistance = TemplarFlamethrower.maxDistance,
					smartCollision = true,
					damageType = DamageType.BypassOneShotProtection
				}.Fire();
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			base.StartAimMode(0.5f, false);
			base.characterBody.isSprinting = false;
			bool flag = this.stopwatch >= this.entryDuration && this.stopwatch < this.entryDuration + this.flamethrowerDuration && !this.hasBegunFlamethrower;
			bool flag2 = flag;
			if (flag2)
			{
				this.hasBegunFlamethrower = true;
				Util.PlaySound(Flamebreath.startAttackSoundString, base.gameObject);
				bool flag3 = this.childLocator;
				bool flag4 = flag3;
				if (flag4)
				{
					this.muzzleTransform = this.childLocator.FindChild(MinigunState.muzzleName);
					this.flamethrowerEffectInstance = UnityEngine.Object.Instantiate<GameObject>(Flamebreath.flamethrowerEffectPrefab, this.muzzleTransform).transform;
					this.flamethrowerEffectInstance.transform.localPosition = Vector3.zero;
					this.flamethrowerEffectInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = 2f;
					foreach (ParticleSystem particleSystem in this.flamethrowerEffectInstance.GetComponentsInChildren<ParticleSystem>())
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
			bool flag7 = this.stopwatch >= this.entryDuration && this.hasBegunFlamethrower && !base.inputBank.skill1.down;
			bool flag8 = flag7;
			if (flag8)
			{
				this.hasBegunFlamethrower = false;
				bool flag9 = this.flamethrowerEffectInstance;
				bool flag10 = flag9;
				if (flag10)
				{
					EntityState.Destroy(this.flamethrowerEffectInstance.gameObject);
				}
				this.outer.SetNextStateToMain();
			}
			else
			{
				bool flag11 = this.hasBegunFlamethrower;
				bool flag12 = flag11;
				if (flag12)
				{
					this.flamethrowerStopwatch += Time.fixedDeltaTime;
					bool flag13 = this.flamethrowerStopwatch > TemplarFlamethrower.tickFrequency / this.attackSpeedStat;
					bool flag14 = flag13;
					if (flag14)
					{
						this.flamethrowerStopwatch -= TemplarFlamethrower.tickFrequency / this.attackSpeedStat;
						this.FireFlame(MinigunState.muzzleName);
						base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
					}
					this.flamethrowerDuration = this.stopwatch + TemplarFlamethrower.baseExitDuration;
				}
				else
				{
					bool flag15 = this.flamethrowerEffectInstance;
					bool flag16 = flag15;
					if (flag16)
					{
						EntityState.Destroy(this.flamethrowerEffectInstance.gameObject);
					}
				}
				bool flag17 = this.stopwatch >= this.flamethrowerDuration + this.entryDuration && base.isAuthority;
				bool flag18 = flag17;
				if (flag18)
				{
					this.outer.SetNextStateToMain();
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

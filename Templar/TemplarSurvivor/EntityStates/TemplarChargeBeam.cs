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
			this.duration = TemplarChargeBeam.baseDuration / this.attackSpeedStat;
			Transform modelTransform = base.GetModelTransform();
			this.animator = base.GetModelAnimator();
			base.StartAimMode(this.duration + 2f, false);
			this.animator.SetBool("WeaponIsReady", true);
			base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
			this.chargePlayID = Util.PlayAttackSpeedSound(ChargeLaser.attackSoundString, base.gameObject, this.attackSpeedStat);
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
							this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(ChargeLaser.effectPrefab, transform.position, transform.rotation);
							this.chargeEffect.transform.parent = transform;
							ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<ScaleParticleSystemDuration>();
							bool flag9 = component2;
							bool flag10 = flag9;
							if (flag10)
							{
								component2.newDuration = this.duration;
							}
						}
						bool flag11 = ChargeLaser.laserPrefab;
						bool flag12 = flag11;
						if (flag12)
						{
							this.laserEffect = UnityEngine.Object.Instantiate<GameObject>(ChargeLaser.laserPrefab, transform.position, transform.rotation);
							this.laserEffect.transform.parent = transform;
							this.laserLineComponent = this.laserEffect.GetComponent<LineRenderer>();
						}
					}
				}
			}
			bool flag13 = base.characterBody;
			bool flag14 = flag13;
			if (flag14)
			{
				base.characterBody.SetAimTimer(this.duration);
			}
			this.flashTimer = 0f;
			this.laserOn = true;
		}

		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeEffect;
			bool flag2 = flag;
			if (flag2)
			{
				EntityState.Destroy(this.chargeEffect);
			}
			bool flag3 = this.laserEffect;
			bool flag4 = flag3;
			if (flag4)
			{
				EntityState.Destroy(this.laserEffect);
			}
		}

		public override void Update()
		{
			base.Update();
			bool flag = this.laserEffect && this.laserLineComponent;
			bool flag2 = flag;
			if (flag2)
			{
				float num = 1000f;
				Ray aimRay = base.GetAimRay();
				Vector3 position = this.laserEffect.transform.parent.position;
				Vector3 point = aimRay.GetPoint(num);
				this.laserDirection = point - position;
				RaycastHit raycastHit;
				bool flag3 = Physics.Raycast(aimRay, out raycastHit, num, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
				bool flag4 = flag3;
				if (flag4)
				{
					point = raycastHit.point;
				}
				this.laserLineComponent.SetPosition(0, position);
				this.laserLineComponent.SetPosition(1, point);
				bool flag5 = this.duration - base.age > 0.5f;
				bool flag6 = flag5;
				float num2;
				if (flag6)
				{
					num2 = base.age / this.duration;
				}
				else
				{
					this.flashTimer -= Time.deltaTime;
					bool flag7 = this.flashTimer <= 0f;
					bool flag8 = flag7;
					if (flag8)
					{
						this.laserOn = !this.laserOn;
						this.flashTimer = 0.0333333351f;
					}
					num2 = (this.laserOn ? 1f : 0f);
				}
				num2 *= TemplarChargeBeam.laserMaxWidth;
				this.laserLineComponent.startWidth = num2;
				this.laserLineComponent.endWidth = num2;
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			bool flag2 = flag;
			if (flag2)
			{
				TemplarFireBeam templarFireBeam = new TemplarFireBeam();
				templarFireBeam.laserDirection = this.laserDirection;
				this.outer.SetNextState(templarFireBeam);
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

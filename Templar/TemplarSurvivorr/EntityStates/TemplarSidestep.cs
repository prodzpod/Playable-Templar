using EntityStates;
using EntityStates.ClayBruiserMonster;
using EntityStates.Commando;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Templar
{
	public class TemplarSidestep : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Util.PlaySound(DodgeState.dodgeSoundString, base.gameObject);
			this.animator = base.GetModelAnimator();
			ChildLocator component = this.animator.GetComponent<ChildLocator>();
			bool flag = base.isAuthority && base.inputBank && base.characterDirection;
			bool flag2 = flag;
			if (flag2)
			{
				this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
			}
			Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
			Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);
			float num = Vector3.Dot(this.forwardDirection, rhs);
			float num2 = Vector3.Dot(this.forwardDirection, rhs2);
			this.animator.SetFloat("forwardSpeed", num, 0.1f, Time.fixedDeltaTime);
			this.animator.SetFloat("rightSpeed", num2, 0.1f, Time.fixedDeltaTime);
			bool flag3 = Mathf.Abs(num) > Mathf.Abs(num2);
			bool flag4 = flag3;
			if (flag4)
			{
				base.PlayAnimation("Body", (num > 0f) ? "DodgeForward" : "DodgeBackward", "Dodge.playbackRate", this.duration);
			}
			else
			{
				base.PlayAnimation("Body", (num2 > 0f) ? "DodgeRight" : "DodgeLeft", "Dodge.playbackRate", this.duration);
			}
			bool flag5 = SpawnState.spawnEffectPrefab;
			bool flag6 = flag5;
			if (flag6)
			{
				Transform transform = component.FindChild("chest");
				bool flag7 = transform;
				bool flag8 = flag7;
				if (flag8)
				{
					UnityEngine.Object.Instantiate<GameObject>(SpawnState.spawnEffectPrefab, transform);
				}
			}
			this.RecalculateSpeed();
			bool flag9 = base.characterMotor && base.characterDirection;
			bool flag10 = flag9;
			if (flag10)
			{
				CharacterMotor characterMotor = base.characterMotor;
				characterMotor.velocity.y = characterMotor.velocity.y * 0.2f;
				base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
			}
			Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
			this.previousPosition = base.transform.position - b;
		}

		private void RecalculateSpeed()
		{
			this.rollSpeed = (4f + 0.3f * this.moveSpeedStat) * Mathf.Lerp(TemplarSidestep.initialSpeedCoefficient, TemplarSidestep.finalSpeedCoefficient, base.fixedAge / this.duration);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.RecalculateSpeed();
			bool flag = base.cameraTargetParams;
			bool flag2 = flag;
			if (flag2)
			{
				base.cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
			}
			Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
			bool flag3 = base.characterMotor && base.characterDirection && normalized != Vector3.zero;
			bool flag4 = flag3;
			if (flag4)
			{
				Vector3 vector = normalized * this.rollSpeed;
				float y = vector.y;
				vector.y = 0f;
				float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
				vector = this.forwardDirection * d;
				vector.y += Mathf.Max(y, 0f);
				base.characterMotor.velocity = vector;
			}
			this.previousPosition = base.transform.position;
			bool flag5 = base.fixedAge >= this.duration && base.isAuthority;
			bool flag6 = flag5;
			if (flag6)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override void OnExit()
		{
			bool flag = base.cameraTargetParams;
			bool flag2 = flag;
			if (flag2)
			{
				base.cameraTargetParams.fovOverride = -1f;
			}
			base.OnExit();
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.forwardDirection);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.forwardDirection = reader.ReadVector3();
		}

		[SerializeField]
		public float duration = 0.3f;

		public static GameObject dodgeEffect;

		public static float initialSpeedCoefficient = 10f;

		public static float finalSpeedCoefficient = 0.25f;

		private float rollSpeed;

		private Vector3 forwardDirection;

		private Animator animator;

		private Vector3 previousPosition;
	}
}

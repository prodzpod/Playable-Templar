using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Templar
{
	public class TemplarMinigunState : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.oldMass = base.characterMotor.mass;
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			bool flag = NetworkServer.active && base.characterBody;
			bool flag2 = flag;
			if (flag2)
			{
				base.characterBody.AddBuff(TemplarMinigunState.slowBuff);
				base.characterBody.AddBuff(Buffs.TemplArmorBuff);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2.5f, false);
			base.characterBody.isSprinting = false;
			bool flag = base.characterMotor.velocity.x == 0f && base.characterMotor.velocity.z == 0f && base.characterMotor.isGrounded;
			bool isGrounded = base.characterMotor.isGrounded;
			bool flag2 = flag && isGrounded;
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = !base.characterBody.HasBuff(Buffs.TemplarstationaryArmorBuff);
				bool flag5 = flag4;
				if (flag5)
				{
					bool active = NetworkServer.active;
					bool flag6 = active;
					if (flag6)
					{
						base.characterBody.RemoveBuff(Buffs.TemplArmorBuff);
						base.characterBody.AddBuff(Buffs.TemplarstationaryArmorBuff);
					}
					base.characterMotor.mass = 10000f;
				}
			}
			else
			{
				bool flag7 = base.characterBody.HasBuff(Buffs.TemplarstationaryArmorBuff);
				bool flag8 = flag7;
				if (flag8)
				{
					bool active2 = NetworkServer.active;
					bool flag9 = active2;
					if (flag9)
					{
						base.characterBody.RemoveBuff(Buffs.TemplarstationaryArmorBuff);
						base.characterBody.AddBuff(Buffs.TemplArmorBuff);
					}
					base.characterMotor.mass = this.oldMass;
				}
			}
		}

		public override void OnExit()
		{
			bool flag = NetworkServer.active && base.characterBody;
			bool flag2 = flag;
			if (flag2)
			{
				base.characterBody.RemoveBuff(TemplarMinigunState.slowBuff);
				bool flag3 = base.HasBuff(Buffs.TemplArmorBuff);
				bool flag4 = flag3;
				if (flag4)
				{
					base.characterBody.RemoveBuff(Buffs.TemplArmorBuff);
				}
				bool flag5 = base.HasBuff(Buffs.TemplarstationaryArmorBuff);
				bool flag6 = flag5;
				if (flag6)
				{
					base.characterBody.RemoveBuff(Buffs.TemplarstationaryArmorBuff);
				}
			}
			base.characterMotor.mass = this.oldMass;
			base.OnExit();
		}

		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill1;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		private static readonly BuffIndex slowBuff;

		private bool standStill;

		protected Transform muzzleTransform;

		private float oldMass;
	}
}

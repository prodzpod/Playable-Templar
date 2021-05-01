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
			if (NetworkServer.active && base.characterBody)
			{

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
				if (!base.characterBody.HasBuff(Buffs.TemplarstationaryArmorBuff))
				{
					if (NetworkServer.active)
					{
						base.characterBody.RemoveBuff(Buffs.TemplArmorBuff);
						base.characterBody.AddBuff(Buffs.TemplarstationaryArmorBuff);
					}
					base.characterMotor.mass = 10000f;
				}
			}
			else
			{
				if (base.characterBody.HasBuff(Buffs.TemplarstationaryArmorBuff))
				{
					if (NetworkServer.active)
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
			if (NetworkServer.active && base.characterBody)
			{
				if (base.HasBuff(Buffs.TemplArmorBuff))
				{
					base.characterBody.RemoveBuff(Buffs.TemplArmorBuff);
				}
				if (base.HasBuff(Buffs.TemplarstationaryArmorBuff))
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

		//private static readonly BuffIndex slowBuff;

		private bool standStill;

		protected Transform muzzleTransform;

		private float oldMass;
	}
}

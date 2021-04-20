using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
	public class TemplarRifleState : BaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(0.5f, false);
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		// (get) Token: 0x0600007B RID: 123 RVA: 0x00005F54 File Offset: 0x00004154
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

		protected Transform muzzleTransform;
	}
}

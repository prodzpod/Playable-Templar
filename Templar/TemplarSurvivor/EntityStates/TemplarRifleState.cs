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

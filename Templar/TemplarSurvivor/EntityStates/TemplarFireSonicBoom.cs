using EntityStates.Treebot.Weapon;
using RoR2;
using System;

namespace Templar
{
	public class TemplarFireSonicBoom : FireSonicBoom
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.baseDuration = 0.5f;
		}

		public override void AddDebuff(CharacterBody body)
		{
			body.AddTimedBuff((BuffIndex)21, this.slowDuration * 3f);
		}

		public override void OnExit()
		{
			bool flag = !this.outer.destroying;
			bool flag2 = flag;
			if (flag2)
			{
				base.GetModelAnimator().SetBool("WeaponIsReady", false);
			}
			base.OnExit();
		}
	}
}

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
            GetModelAnimator().SetBool("WeaponIsReady", true);
            baseDuration = 0.5f;
        }

        public override void AddDebuff(CharacterBody body)
        {
            body.AddTimedBuff((BuffIndex)21, slowDuration * 3f);
        }

        public override void OnExit()
        {
            bool flag = !outer.destroying;
            bool flag2 = flag;
            if (flag2)
            {
                GetModelAnimator().SetBool("WeaponIsReady", false);
            }
            base.OnExit();
        }
    }
}

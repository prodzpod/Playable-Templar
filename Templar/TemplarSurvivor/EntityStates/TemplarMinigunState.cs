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
            oldMass = characterMotor.mass;
            muzzleTransform = FindModelChild(MinigunState.muzzleName);
            if (NetworkServer.active && characterBody)
            {

                characterBody.AddBuff(Buffs.TemplArmorBuff);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode(2.5f, false);
            characterBody.isSprinting = false;
            bool flag = characterMotor.velocity.x == 0f && characterMotor.velocity.z == 0f && characterMotor.isGrounded;
            bool isGrounded = characterMotor.isGrounded;
            bool flag2 = flag && isGrounded;
            bool flag3 = flag2;
            if (flag3)
            {
                if (!characterBody.HasBuff(Buffs.TemplarstationaryArmorBuff))
                {
                    if (NetworkServer.active)
                    {
                        characterBody.RemoveBuff(Buffs.TemplArmorBuff);
                        characterBody.AddBuff(Buffs.TemplarstationaryArmorBuff);
                    }
                    characterMotor.mass = 10000f;
                }
            }
            else
            {
                if (characterBody.HasBuff(Buffs.TemplarstationaryArmorBuff))
                {
                    if (NetworkServer.active)
                    {
                        characterBody.RemoveBuff(Buffs.TemplarstationaryArmorBuff);
                        characterBody.AddBuff(Buffs.TemplArmorBuff);
                    }
                    characterMotor.mass = oldMass;
                }
            }
        }

        public override void OnExit()
        {
            if (NetworkServer.active && characterBody)
            {
                if (HasBuff(Buffs.TemplArmorBuff))
                {
                    characterBody.RemoveBuff(Buffs.TemplArmorBuff);
                }
                if (HasBuff(Buffs.TemplarstationaryArmorBuff))
                {
                    characterBody.RemoveBuff(Buffs.TemplarstationaryArmorBuff);
                }
            }
            characterMotor.mass = oldMass;
            base.OnExit();
        }

        protected ref InputBankTest.ButtonState skillButtonState
        {
            get
            {
                return ref inputBank.skill1;
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

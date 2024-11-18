﻿using EntityStates;
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
            Util.PlaySound(DodgeState.dodgeSoundString, gameObject);
            animator = GetModelAnimator();
            ChildLocator component = animator.GetComponent<ChildLocator>();
            bool flag = isAuthority && inputBank && characterDirection;
            bool flag2 = flag;
            if (flag2)
            {
                forwardDirection = ((inputBank.moveVector == Vector3.zero) ? characterDirection.forward : inputBank.moveVector).normalized;
            }
            Vector3 rhs = characterDirection ? characterDirection.forward : forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);
            float num = Vector3.Dot(forwardDirection, rhs);
            float num2 = Vector3.Dot(forwardDirection, rhs2);
            animator.SetFloat("forwardSpeed", num, 0.1f, Time.fixedDeltaTime);
            animator.SetFloat("rightSpeed", num2, 0.1f, Time.fixedDeltaTime);
            bool flag3 = Mathf.Abs(num) > Mathf.Abs(num2);
            bool flag4 = flag3;
            if (flag4)
            {
                PlayAnimation("Body", (num > 0f) ? "DodgeForward" : "DodgeBackward", "Dodge.playbackRate", duration);
            }
            else
            {
                PlayAnimation("Body", (num2 > 0f) ? "DodgeRight" : "DodgeLeft", "Dodge.playbackRate", duration);
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
                    UnityEngine.Object.Instantiate(SpawnState.spawnEffectPrefab, transform);
                }
            }
            RecalculateSpeed();
            bool flag9 = characterMotor && characterDirection;
            bool flag10 = flag9;
            if (flag10)
            {
                CharacterMotor characterMotor = base.characterMotor;
                characterMotor.velocity.y = characterMotor.velocity.y * 0.2f;
                base.characterMotor.velocity = forwardDirection * rollSpeed;
            }
            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;
        }

        private void RecalculateSpeed()
        {
            rollSpeed = (4f + 0.3f * moveSpeedStat) * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateSpeed();
            bool flag = cameraTargetParams;
            bool flag2 = flag;
            if (flag2)
            {
                cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, fixedAge / duration);
            }
            Vector3 normalized = (transform.position - previousPosition).normalized;
            bool flag3 = characterMotor && characterDirection && normalized != Vector3.zero;
            bool flag4 = flag3;
            if (flag4)
            {
                Vector3 vector = normalized * rollSpeed;
                float y = vector.y;
                vector.y = 0f;
                float d = Mathf.Max(Vector3.Dot(vector, forwardDirection), 0f);
                vector = forwardDirection * d;
                vector.y += Mathf.Max(y, 0f);
                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;
            bool flag5 = fixedAge >= duration && isAuthority;
            bool flag6 = flag5;
            if (flag6)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            bool flag = cameraTargetParams;
            bool flag2 = flag;
            if (flag2)
            {
                cameraTargetParams.fovOverride = -1f;
            }
            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
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

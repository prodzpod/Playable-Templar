using EntityStates;
using EntityStates.ClayBoss;
using RoR2;
using System;
using UnityEngine;

namespace Templar
{
    public class TemplarOverdrive : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            bool flag = characterBody;
            bool flag2 = flag;
            if (flag2)
            {
                characterBody.AddTimedBuff(Buffs.TemplarOverdriveBuff, 3f);
            }
            EffectManager.SimpleMuzzleFlash(FireTarball.effectPrefab, gameObject, "Root", false);
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosivePotExplosion"), new EffectData
            {
                origin = characterBody.corePosition,
                scale = 12f
            }, true);

            BlastAttack blastAttack = new()
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = teamComponent.teamIndex,
                baseForce = pushForce,
                bonusForce = Vector3.zero,
                position = transform.position,
                radius = 12f,
                falloffModel = BlastAttack.FalloffModel.None,
                crit = false,
                baseDamage = 0f,
                procCoefficient = 0f,
                damageType = DamageType.ClayGoo
            };
            blastAttack.Fire();
            modelTransform = GetModelTransform();
            bool flag3 = modelTransform;
            bool flag4 = flag3;
            if (flag4)
            {
                TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                temporaryOverlay.duration = 8f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matClayGooDebuff");
                temporaryOverlay.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
            }
            Util.PlayAttackSpeedSound(FireTarball.attackSoundString, gameObject, 0.75f);
            outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public static float pushForce = 2500f;

        private Transform modelTransform;
    }
}

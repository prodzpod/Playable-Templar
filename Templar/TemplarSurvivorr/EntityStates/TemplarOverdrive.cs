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
			bool flag = base.characterBody;
			bool flag2 = flag;
			if (flag2)
			{
				base.characterBody.AddTimedBuff(Buffs.TemplarOverdriveBuff, 3f);
			}
			EffectManager.SimpleMuzzleFlash(FireTarball.effectPrefab, base.gameObject, "Root", false);
			BlastAttack blastAttack = new BlastAttack
			{
				attacker = base.gameObject,
				inflictor = base.gameObject,
				teamIndex = base.teamComponent.teamIndex,
				baseForce = TemplarOverdrive.pushForce,
				bonusForce = Vector3.zero,
				position = base.transform.position,
				radius = 12f,
				falloffModel = BlastAttack.FalloffModel.None,
				crit = false,
				baseDamage = 0f,
				procCoefficient = 0f,
				damageType = DamageType.ClayGoo
			};
			blastAttack.Fire();
			this.modelTransform = base.GetModelTransform();
			bool flag3 = this.modelTransform;
			bool flag4 = flag3;
			if (flag4)
			{
				TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
				temporaryOverlay.duration = 8f;
				temporaryOverlay.animateShaderAlpha = true;
				temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
				temporaryOverlay.destroyComponentOnEnd = true;
				temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matClayGooDebuff");
				temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
			}
			Util.PlayAttackSpeedSound(FireTarball.attackSoundString, base.gameObject, 0.75f);
			this.outer.SetNextStateToMain();
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public static float pushForce = 2500f;

		private Transform modelTransform;
	}
}

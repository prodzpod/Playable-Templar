﻿using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BetterUI;
using EntityStates;
using EntityStates.ClayBruiserMonster;
using KinematicCharacterController;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace Templar
{
	public class Templar
	{
		internal static void TemplarSetup()
		{
			Templar.TokenSetup();
			Templar.TemplarSurvivor();
			Templar.SkillSetup();
			Templar.CreateMaster();
			TemplarSkins.RegisterSkins();
			Templar.IsBUDefined = Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI");
			bool isBUDefined = Templar.IsBUDefined;
			if (isBUDefined)
			{
				Templar.BetterSetter();
			}
		}

		internal static void TemplarSurvivor()
		{
			Templar.myCharacter = Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").InstantiateClone("Templar_Survivor", true, "C:\\Users\\Tymmey\\Desktop\\RoRStuff\\Templar\\Templar\\Survivor\\Templar\\Templar.cs", "TemplarSurvivor", 37);
			Templar.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.localScale = Vector3.one * 0.9f;
			Templar.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.Translate(new Vector3(0f, 1.6f, 0f));
			foreach (KinematicCharacterMotor kinematicCharacterMotor in Templar.myCharacter.GetComponentsInChildren<KinematicCharacterMotor>())
			{
				kinematicCharacterMotor.SetCapsuleDimensions(kinematicCharacterMotor.Capsule.radius * 0.5f, kinematicCharacterMotor.Capsule.height * 0.5f, 0f);
			}
			Templar.myCharacter.GetComponent<CharacterBody>().aimOriginTransform.Translate(new Vector3(0f, 0f, 0.8f));
			Templar.myCharacter.GetComponent<SetStateOnHurt>().canBeHitStunned = false;
			Templar.myCharacter.tag = "Player";
			Templar.characterDisplay = Templar.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.InstantiateClone("TemplarDisplay", true, "C:TemplarClean.cs", "RegisterLemurian", 153);
			Templar.characterDisplay.transform.localScale = Vector3.one * 0.8f;
			Templar.characterDisplay.AddComponent<Templar.TemplarMenuAnim>();
			Templar.characterDisplay.AddComponent<NetworkIdentity>();
			Templar.myCharacter.GetComponent<CameraTargetParams>().cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CameraTargetParams>().cameraParams;
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/CharacterBodies/Pot2Body");
			MeshRenderer componentInChildren = gameObject.GetComponentInChildren<MeshRenderer>();
			GameObject gameObject2 = componentInChildren.gameObject.InstantiateClone("VagabondHead", false, "C:Lemurian.cs", "RegisterLemurian", 679);
			UnityEngine.Object.Destroy(gameObject2.GetComponent<HurtBoxGroup>());
			UnityEngine.Object.Destroy(gameObject2.transform.GetComponentInChildren<HurtBox>());
			UnityEngine.Object.Destroy(gameObject2.transform.GetChild(0).gameObject);
			gameObject2.transform.parent = Templar.myCharacter.GetComponentInChildren<ChildLocator>().FindChild("Head");
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1.25f);
			gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
			gameObject2.transform.localPosition = new Vector3(0f, -0.24f, 0f);
			MeshRenderer componentInChildren2 = gameObject2.GetComponentInChildren<MeshRenderer>();
			CharacterModel.RendererInfo[] baseRendererInfos = Templar.myCharacter.GetComponentInChildren<CharacterModel>().baseRendererInfos;
			CharacterModel.RendererInfo[] baseRendererInfos2 = new CharacterModel.RendererInfo[]
			{
				baseRendererInfos[0],
				baseRendererInfos[1],
				baseRendererInfos[2],
				baseRendererInfos[3],
				baseRendererInfos[4],
				baseRendererInfos[5],
				new CharacterModel.RendererInfo
				{
					defaultMaterial = componentInChildren.material,
					renderer = componentInChildren2,
					defaultShadowCastingMode = ShadowCastingMode.On,
					ignoreOverlays = false
				}
			};
			Templar.myCharacter.GetComponentInChildren<CharacterModel>().baseRendererInfos = baseRendererInfos2;
			CharacterBody component = Templar.myCharacter.GetComponent<CharacterBody>();
			component.portraitIcon = Assets.templarIcon;
			component.SetSpreadBloom(0f, false);
			component.spreadBloomCurve = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CharacterBody>().spreadBloomCurve;
			component.spreadBloomDecayTime = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CharacterBody>().spreadBloomDecayTime;
			component.name = "Templar_Survivor";
			component.baseNameToken = "Templar_Survivor";
			component.subtitleNameToken = "Templar_Subtitle";
			component.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
			component.baseMaxHealth = Templar.baseHealth.Value;
			component.levelMaxHealth = Templar.healthGrowth.Value;
			component.baseRegen = Templar.baseRegen.Value;
			component.levelRegen = Templar.regenGrowth.Value;
			component.baseDamage = Templar.baseDamage.Value;
			component.levelDamage = Templar.damageGrowth.Value;
			component.baseArmor = Templar.baseArmor.Value;
			component.baseJumpPower = 15f;
			component.baseCrit = 1f;
			component.baseMoveSpeed = 7f;
			component.autoCalculateLevelStats = true;
			component.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
			component.preferredPodPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CharacterBody>().preferredPodPrefab;
			component.levelMoveSpeed = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().levelMoveSpeed;
			component.sprintingSpeedMultiplier = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().sprintingSpeedMultiplier;
			component.GetComponent<CharacterMotor>().mass = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponentInChildren<CharacterMotor>().mass;
			Templar.TemplarPrefab = Templar.myCharacter;
			SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
			survivorDef.cachedName = "Templar_Survivor";
			survivorDef.descriptionToken = "Templar_Description";
			survivorDef.primaryColor = Templar.CHAR_COLOR;
			survivorDef.bodyPrefab = Templar.TemplarPrefab;
			survivorDef.displayPrefab = Templar.characterDisplay;
			survivorDef.outroFlavorToken = "Templar_ENDING";
			survivorDef.desiredSortPosition = 16f;
			Loader.survivorDefs.Add(survivorDef);
			Loader.bodyPrefabs.Add(Templar.myCharacter);
		}

		internal static void SkillSetup()
		{
			GenericSkill[] componentsInChildren = Templar.myCharacter.GetComponentsInChildren<GenericSkill>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
			}
			Templar.myCharacter.GetComponent<SkillLocator>();
			Templar.PrimarySetup();
			Templar.SecondarySetup();
			Templar.UtilitySetup();
			Templar.SpecialSetup();
			Templar.PassiveSetup();
			LoadoutAPI.AddSkill(typeof(TemplarChargeMiniRocket));
			LoadoutAPI.AddSkill(typeof(TemplarChargeRocket));
			LoadoutAPI.AddSkill(typeof(TemplarFireMiniRocket));
			LoadoutAPI.AddSkill(typeof(TemplarFireRocket));
			LoadoutAPI.AddSkill(typeof(TemplarFireSonicBoom));
			LoadoutAPI.AddSkill(typeof(TemplarMinigunFire));
			LoadoutAPI.AddSkill(typeof(TemplarMinigunSpinDown));
			LoadoutAPI.AddSkill(typeof(TemplarMinigunSpinUp));
			LoadoutAPI.AddSkill(typeof(TemplarMinigunState));
			LoadoutAPI.AddSkill(typeof(TemplarRifleFire));
			LoadoutAPI.AddSkill(typeof(TemplarRifleSpinDown));
			LoadoutAPI.AddSkill(typeof(TemplarRifleState));
			LoadoutAPI.AddSkill(typeof(TemplarShotgun));
			LoadoutAPI.AddSkill(typeof(TemplarSidestep));
			LoadoutAPI.AddSkill(typeof(TemplarSwapWeapon));
			LoadoutAPI.AddSkill(typeof(TemplarThrowClaybomb));
			LoadoutAPI.AddSkill(typeof(TemplarChargeBeam));
			LoadoutAPI.AddSkill(typeof(TemplarFireBeam));
			LoadoutAPI.AddSkill(typeof(TemplarFlamethrower));
			LoadoutAPI.AddSkill(typeof(TemplarOverdrive));
		}

		internal static void PrimarySetup()
		{
			SkillLocator component = Templar.myCharacter.GetComponent<SkillLocator>();
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarMinigunSpinUp));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_MINIGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_MINIGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_MINIGUN_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_RAPIDFIRE"
			};
			skillDef.cancelSprintingOnActivation = true;
			LoadoutAPI.AddSkillDef(skillDef);
			component.primary = Templar.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			component.primary.SetFieldValue("_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.primary.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarRifleFire));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = true;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1b;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_PRECISEMINIGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef.cancelSprintingOnActivation = true;
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeBeam));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1c;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_RAILGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_RAILGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_RAILGUN_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_EXPLOSIVE"
			};
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef.cancelSprintingOnActivation = true;
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarFlamethrower));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1d;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_FLAMETHROWER_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_EXPLOSIVE"
			};
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			bool value = Templar.bazookaGoBoom.Value;
			bool flag = value;
			if (flag)
			{
				LanguageAPI.Add("TEMPLAR_PRIMARY_BAZOOKA_NAME", "Bazooka Mk. 2");
				LanguageAPI.Add("TEMPLAR_PRIMARY_BAZOOKA_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + (Templar.miniBazookaDamageCoefficient.Value * 100f).ToString() + "% damage</style>.");
				skillDef = ScriptableObject.CreateInstance<SkillDef>();
				skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeMiniRocket));
				skillDef.activationStateMachineName = "Weapon";
				skillDef.baseMaxStock = 1;
				skillDef.baseRechargeInterval = 0.1f;
				skillDef.beginSkillCooldownOnSkillEnd = true;
				skillDef.canceledFromSprinting = false;
				skillDef.fullRestockOnAssign = true;
				skillDef.interruptPriority = InterruptPriority.Any;
				skillDef.isCombatSkill = true;
				skillDef.mustKeyPress = false;
				skillDef.rechargeStock = 1;
				skillDef.requiredStock = 1;
				skillDef.stockToConsume = 1;
				skillDef.icon = Assets.icon4;
				skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_BAZOOKA_DESCRIPTION";
				skillDef.skillName = "TEMPLAR_PRIMARY_BAZOOKA_NAME";
				skillDef.skillNameToken = "TEMPLAR_PRIMARY_BAZOOKA_NAME";
				skillDef.keywordTokens = new string[]
				{
					"KEYWORD_EXPLOSIVE"
				};
				skillDef.cancelSprintingOnActivation = true;
				LoadoutAPI.AddSkillDef(skillDef);
				Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
				skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
				{
					skillDef = skillDef,
					viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
				};
			}
		}

		internal static void SecondarySetup()
		{
			SkillLocator component = Templar.myCharacter.GetComponent<SkillLocator>();
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarThrowClaybomb));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = Templar.clayGrenadeStock.Value;
			skillDef.baseRechargeInterval = Templar.clayGrenadeCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Skill;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon2;
			skillDef.skillDescriptionToken = "TEMPLAR_SECONDARY_GRENADE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SECONDARY_GRENADE_NAME";
			skillDef.skillNameToken = "TEMPLAR_SECONDARY_GRENADE_NAME";
			skillDef.cancelSprintingOnActivation = true;
			LoadoutAPI.AddSkillDef(skillDef);
			component.secondary = Templar.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			component.secondary.SetFieldValue("_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.secondary.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarShotgun));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = Templar.blunderbussStock.Value;
			skillDef.baseRechargeInterval = Templar.blunderbussCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Skill;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon2b;
			skillDef.skillDescriptionToken = "TEMPLAR_SECONDARY_SHOTGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SECONDARY_SHOTGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_SECONDARY_SHOTGUN_NAME";
			skillDef.cancelSprintingOnActivation = true;
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void UtilitySetup()
		{
			SkillLocator component = Templar.myCharacter.GetComponent<SkillLocator>();
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarOverdrive));
			skillDef.activationStateMachineName = "Body";
			skillDef.baseMaxStock = Templar.overdriveStock.Value;
			skillDef.baseRechargeInterval = Templar.overdriveCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon3;
			skillDef.skillDescriptionToken = "TEMPLAR_UTILITY_OVERDRIVE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_UTILITY_OVERDRIVE_NAME";
			skillDef.skillNameToken = "TEMPLAR_UTILITY_OVERDRIVE_NAME";
			skillDef.cancelSprintingOnActivation = false;
			LoadoutAPI.AddSkillDef(skillDef);
			component.utility = Templar.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			component.utility.SetFieldValue("_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.utility.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			SkillDef skillDef2 = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<SkillLocator>().utility.skillFamily.variants[0].skillDef;
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarSidestep));
			skillDef.activationStateMachineName = "Body";
			skillDef.baseRechargeInterval = Templar.dashCooldown.Value;
			skillDef.baseMaxStock = Templar.dashStock.Value;
			skillDef.beginSkillCooldownOnSkillEnd = skillDef2.beginSkillCooldownOnSkillEnd;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = skillDef2.fullRestockOnAssign;
			skillDef.interruptPriority = skillDef2.interruptPriority;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = skillDef2.mustKeyPress;
			skillDef.rechargeStock = skillDef2.rechargeStock;
			skillDef.requiredStock = skillDef2.requiredStock;
			skillDef.stockToConsume = skillDef2.stockToConsume;
			skillDef.icon = Assets.icon3b;
			skillDef.skillDescriptionToken = "TEMPLAR_UTILITY_DODGE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_UTILITY_DODGE_NAME";
			skillDef.skillNameToken = "TEMPLAR_UTILITY_DODGE_NAME";
			skillDef.cancelSprintingOnActivation = false;
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void SpecialSetup()
		{
			SkillLocator component = Templar.myCharacter.GetComponent<SkillLocator>();
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeRocket));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = Templar.bazookaStock.Value;
			skillDef.baseRechargeInterval = Templar.bazookaCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon4;
			skillDef.skillDescriptionToken = "TEMPLAR_SPECIAL_FIRE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SPECIAL_FIRE_NAME";
			skillDef.skillNameToken = "TEMPLAR_SPECIAL_FIRE_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_EXPLOSIVE"
			};
			skillDef.cancelSprintingOnActivation = true;
			LoadoutAPI.AddSkillDef(skillDef);
			component.special = Templar.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			component.special.SetFieldValue("_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.special.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarSwapWeapon));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0.1f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef.isCombatSkill = false;
			skillDef.mustKeyPress = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon4b;
			skillDef.skillDescriptionToken = "TEMPLAR_SPECIAL_SWAP_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SPECIAL_SWAP_NAME";
			skillDef.skillNameToken = "TEMPLAR_SPECIAL_SWAP_NAME";
			skillDef.cancelSprintingOnActivation = true;
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void CreateMaster()
		{
			Templar.doppelganger = Resources.Load<GameObject>("prefabs/charactermasters/commandomonstermaster").InstantiateClone("TemplarMonsterMaster", true, "C:TemplarClean.cs", "CreateMaster", 155);
			Templar.doppelganger.GetComponent<CharacterMaster>().bodyPrefab = Templar.myCharacter;
			Loader.masterPrefabs.Add(Templar.doppelganger);
		}

		internal static void PassiveSetup()
		{
			SkillLocator component = Templar.myCharacter.GetComponent<SkillLocator>();
			LanguageAPI.Add("TEMPLAR_PASSIVE_NAME", "Volatile Tar");
			LanguageAPI.Add("TEMPLAR_PASSIVE_DESCRIPTION", "Certain attacks cover enemies in <style=cIsDamage>tar</style>, <style=cIsUtility>slowing</style> them. <style=cIsDamage>Ignite</style> the <style=cIsDamage>tar</style> to create an <style=cIsDamage>explosion</style> that leaves enemies <style=cIsDamage>Scorched</style>, <style=cIsHealth>reducing their armor</style>.");
			component.passiveSkill.enabled = true;
			component.passiveSkill.skillNameToken = "TEMPLAR_PASSIVE_NAME";
			component.passiveSkill.skillDescriptionToken = "TEMPLAR_PASSIVE_DESCRIPTION";
			component.passiveSkill.icon = Assets.iconP;
		}

		internal static void TokenSetup()
		{
			LanguageAPI.Add("Templar_Survivor", "Templar");
			LanguageAPI.Add("Templar_Subtitle", "Templar");
			LanguageAPI.Add("Templar_ENDING", "..and so it left, reveling in its triumph.");
			string text = "The Clay Templar is a slow, tanky bruiser who uses the many weapons in his arsenal to mow down his opposition with ease.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Minigun takes time to rev up, but inflicts heavy damage at a high rate of fire." + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Tar Rifle is an all around good weapon in most cases, but lacks the high damage of Minigun.</color>" + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Let Blunderbuss reload all 4 shots and unload them all at once for a big burst of damage." + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Use Bazooka after applying tar to deal massive AoE damage with chain explosions." + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > The explosion force from Bazooka can be used to launch yourself high up with good timing.</color>" + Environment.NewLine;
			LanguageAPI.Add("Templar_Description", text);
			LanguageAPI.Add("KEYWORD_RAPIDFIRE", "<style=cKeywordName>Rapidfire</style><style=cSub><style=cIsDamage>Rate of fire</style> increases the longer the skill is held.</style></style>");
			LanguageAPI.Add("KEYWORD_EXPLOSIVE", "<style=cKeywordName>Explosive</style><style=cSub><style=cIsDamage>Ignite</style> <style=cIsUtility>tarred enemies</style>, creating an <style=cIsDamage>explosion</style> for <style=cIsDamage>20% of the damage dealt</style> and <style=cIsHealth>reducing their armor</style>.</style></style>");
			LanguageAPI.Add("TEMPLAR_PRIMARY_MINIGUN_NAME", "Minigun");
			LanguageAPI.Add("TEMPLAR_PRIMARY_MINIGUN_DESCRIPTION", "<style=cIsDamage>Rapidfire</style>. Rev up and fire your <style=cIsUtility>minigun</style>, dealing <style=cIsDamage>" + (Templar.minigunDamageCoefficient.Value * 100f).ToString() + " % damage</style> per bullet. <style=cIsUtility>Slow</style> your movement while shooting, but gain<style=cIsHealing>bonus armor</style>.");
			LanguageAPI.Add("TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME", "Tar Rifle");
			LanguageAPI.Add("TEMPLAR_PRIMARY_PRECISEMINIGUN_DESCRIPTION", "Fire a <style=cIsUtility>tar rifle</style>, dealing <style=cIsDamage>" + (Templar.rifleDamageCoefficient.Value * 100f).ToString() + "% damage</style> per bullet and applying <style=cIsUtility>tar</style> with high accuracy.");
			LanguageAPI.Add("TEMPLAR_PRIMARY_RAILGUN_NAME", "Railgun");
			LanguageAPI.Add("TEMPLAR_PRIMARY_RAILGUN_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Charge up and fire a <style=cIsUtility>piercing bullet</style>, dealing <style=cIsDamage>800% damage</style>.");
			LanguageAPI.Add("TEMPLAR_PRIMARY_FLAMETHROWER_NAME", "Flamethrower");
			LanguageAPI.Add("TEMPLAR_PRIMARY_FLAMETHROWER_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>continuous stream of flames</style>, dealing <style=cIsDamage>1250% damage per second</style>.");
			text = "Throw a <style=cIsUtility>clay bomb</style> that explodes for <style=cIsDamage>" + (Templar.clayGrenadeDamageCoefficient.Value * 100f).ToString() + "% damage</style> and inflicts <style=cIsUtility>tar</style>.";
			bool flag = Templar.clayGrenadeStock.Value > 1;
			bool flag2 = flag;
			if (flag2)
			{
				text = string.Concat(new object[]
				{
					text,
					" Hold up to <style=cIsDamage>",
					Templar.clayGrenadeStock.Value,
					"</style> bombs."
				});
			}
			LanguageAPI.Add("TEMPLAR_SECONDARY_GRENADE_NAME", "Clay Bomb");
			LanguageAPI.Add("TEMPLAR_SECONDARY_GRENADE_DESCRIPTION", text);
			text = string.Concat(new object[]
			{
				"Fire a burst of <style=cIsUtility>pellets</style>, dealing <style=cIsDamage>",
				Templar.blunderbussPelletCount.Value,
				"x",
				Templar.blunderbussDamageCoefficient.Value * 100f,
				"% damage</style>."
			});
			bool flag3 = Templar.blunderbussStock.Value > 1;
			bool flag4 = flag3;
			if (flag4)
			{
				text = string.Concat(new object[]
				{
					text,
					" Store up to ",
					Templar.blunderbussStock.Value,
					" shots."
				});
			}
			LanguageAPI.Add("TEMPLAR_SECONDARY_SHOTGUN_NAME", "Blunderbuss");
			LanguageAPI.Add("TEMPLAR_SECONDARY_SHOTGUN_DESCRIPTION", text);
			LanguageAPI.Add("TEMPLAR_UTILITY_OVERDRIVE_NAME", "Tar Overdrive");
			LanguageAPI.Add("TEMPLAR_UTILITY_OVERDRIVE_DESCRIPTION", "Shift into <style=cIsUtility>maximum overdrive</style>, knocking enemies back and applying <style=cIsUtility>tar</style>. <style=cIsUtility>Gain bonus attack speed and health regen for 3 seconds</style>.");
			text = "<style=cIsUtility>Dash</style> a short distance. Can dash while shooting.";
			bool flag5 = Templar.dashStock.Value > 1;
			bool flag6 = flag5;
			if (flag6)
			{
				text = string.Concat(new object[]
				{
					text,
					" Store up to ",
					Templar.dashStock.Value,
					" charges."
				});
			}
			LanguageAPI.Add("TEMPLAR_UTILITY_DODGE_NAME", "Sidestep");
			LanguageAPI.Add("TEMPLAR_UTILITY_DODGE_DESCRIPTION", text);
			LanguageAPI.Add("TEMPLAR_SPECIAL_FIRE_NAME", "Bazooka");
			LanguageAPI.Add("TEMPLAR_SPECIAL_FIRE_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + (Templar.bazookaDamageCoefficient.Value * 100f).ToString() + "% damage</style>.");
			LanguageAPI.Add("TEMPLAR_SPECIAL_SWAP_NAME", "Weapon Swap");
			LanguageAPI.Add("TEMPLAR_SPECIAL_SWAP_DESCRIPTION", "<style=cIsUtility>Swap</style> your <style=cIsDamage>primary</style> weapon type.");
		}

		internal static void BetterSetter()
		{
			ProcCoefficientCatalog.AddSkill("TEMPLAR_PRIMARY_MINIGUN_NAME", "Bullet", Templar.minigunProcCoefficient.Value);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME", "Bullet", Templar.rifleProcCoefficient.Value);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_PRIMARY_RAILGUN_NAME", "Laser", 1f);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_PRIMARY_FLAMETHROWER_NAME", "Bullet", 0.4f);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_PRIMARY_BAZOOKA_NAME", "Blast", 0.8f);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_SECONDARY_GRENADE_NAME", "Blast", 0.8f);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_SECONDARY_SHOTGUN_NAME", "Bullet", Templar.blunderbussProcCoefficient.Value);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_UTILITY_DODGE_NAME", "Bullet", 0.5f);
			ProcCoefficientCatalog.AddSkill("TEMPLAR_SPECIAL_FIRE_NAME", "Bullet", 0.8f);
		}

		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000890A File Offset: 0x00006B0A
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00008911 File Offset: 0x00006B11
		public static bool IsBUDefined { get; private set; }

		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00008919 File Offset: 0x00006B19
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00008920 File Offset: 0x00006B20
		public static bool IsBUDefined2 { get; private set; }

		public static GameObject myCharacter;

		public static GameObject TemplarPrefab;

		public static GameObject characterDisplay;

		public static GameObject doppelganger;

		private static readonly Color CHAR_COLOR = new Color(0.784313738f, 0.294117659f, 0.05882353f);

		public static GameObject templarRocket;

		public static GameObject templarGrenade;

		public static GameObject templarRocketEffect;

		public static ConfigEntry<float> baseHealth;

		public static ConfigEntry<float> healthGrowth;

		public static ConfigEntry<float> baseArmor;

		public static ConfigEntry<float> baseDamage;

		public static ConfigEntry<float> damageGrowth;

		public static ConfigEntry<float> baseRegen;

		public static ConfigEntry<float> regenGrowth;

		public static ConfigEntry<float> FireBuffDurationBonus;

		public static ConfigEntry<float> baseJumpPower;

		public static ConfigEntry<string> enabled;

		public static ConfigEntry<float> minigunDamageCoefficient;

		public static ConfigEntry<float> minigunProcCoefficient;

		public static ConfigEntry<float> minigunForce;

		public static ConfigEntry<float> minigunArmorBoost;

		public static ConfigEntry<float> minigunStationaryArmorBoost;

		public static ConfigEntry<float> minigunMinFireRate;

		public static ConfigEntry<float> minigunMaxFireRate;

		public static ConfigEntry<float> minigunFireRateGrowth;

		public static ConfigEntry<float> rifleDamageCoefficient;

		public static ConfigEntry<float> rifleProcCoefficient;

		public static ConfigEntry<float> rifleForce;

		public static ConfigEntry<float> rifleFireRate;

		public static ConfigEntry<int> clayGrenadeStock;

		public static ConfigEntry<float> clayGrenadeCooldown;

		public static ConfigEntry<float> clayGrenadeDamageCoefficient;

		public static ConfigEntry<float> clayGrenadeProcCoefficient;

		public static ConfigEntry<float> clayGrenadeRadius;

		public static ConfigEntry<float> clayGrenadeDetonationTime;

		public static ConfigEntry<int> blunderbussStock;

		public static ConfigEntry<float> blunderbussCooldown;

		public static ConfigEntry<int> blunderbussPelletCount;

		public static ConfigEntry<float> blunderbussDamageCoefficient;

		public static ConfigEntry<float> blunderbussProcCoefficient;

		public static ConfigEntry<float> blunderbussSpread;

		public static ConfigEntry<int> tarStock;

		public static ConfigEntry<float> tarCooldown;

		public static ConfigEntry<int> overdriveStock;

		public static ConfigEntry<float> overdriveCooldown;

		public static ConfigEntry<int> dashStock;

		public static ConfigEntry<float> dashCooldown;

		public static ConfigEntry<int> bazookaStock;

		public static ConfigEntry<float> bazookaCooldown;

		public static ConfigEntry<float> bazookaDamageCoefficient;

		public static ConfigEntry<float> bazookaProcCoefficient;

		public static ConfigEntry<float> bazookaBlastRadius;

		public static ConfigEntry<float> miniBazookaDamageCoefficient;

		public static ConfigEntry<bool> jellyfishEvent;

		public static ConfigEntry<bool> bazookaGoBoom;

		public class TemplarMissileController : MonoBehaviour
		{
			private void Awake()
			{
				this.rb = base.GetComponentInChildren<Rigidbody>();
				this.speed = Templar.TemplarMissileController.startSpeed;
			}

			private void FixedUpdate()
			{
				this.speed += Templar.TemplarMissileController.acceleration;
				this.rb.velocity = base.transform.forward * this.speed;
			}

			public static float acceleration = 2.5f;

			public static float startSpeed = 25f;

			private float speed;

			private Rigidbody rb;
		}

		public class TemplarSeparateFromParent : MonoBehaviour
		{
			private void Awake()
			{
				base.transform.SetParent(null);
			}
		}

		public class TemplarExplosionForce : MonoBehaviour
		{
			private void Awake()
			{
				bool flag = true;
				bool flag2 = flag;
				bool flag3 = flag2;
				if (flag3)
				{
					Collider[] array = Physics.OverlapSphere(base.transform.position, this.radius);
					foreach (Collider collider in array)
					{
						CharacterBody componentInChildren = collider.transform.root.GetComponentInChildren<CharacterBody>();
						bool flag4 = componentInChildren != null;
						bool flag5 = flag4;
						if (flag5)
						{
							bool flag6 = componentInChildren.baseNameToken == "Templar_Survivor";
							bool flag7 = flag6;
							if (flag7)
							{
								bool flag8 = componentInChildren.characterMotor != null;
								bool flag9 = flag8;
								if (flag9)
								{
									float num = 16f / Vector3.Distance(componentInChildren.transform.position, base.transform.position);
									Vector3 vector = new Vector3(0f, Mathf.Clamp(num * this.force, 0f, this.maxForce), 0f);
									bool flag10 = !componentInChildren.characterMotor.isGrounded;
									bool flag11 = flag10;
									if (flag11)
									{
										componentInChildren.characterMotor.ApplyForce(vector, false, false);
									}
								}
							}
						}
					}
				}
			}

			public float force = 750f;

			public float radius = 16f;

			public float maxForce = 4000f;
		}

		public class TemplarMenuAnim : MonoBehaviour
		{
			internal void OnEnable()
			{
				base.StartCoroutine(this.SpawnAnim());
			}

			internal void OnDisable()
			{
				bool flag = this.playID > 0U;
				bool flag2 = flag;
				if (flag2)
				{
					AkSoundEngine.StopPlayingID(this.playID);
				}
			}

			private IEnumerator SpawnAnim()
			{
				Animator animator = base.GetComponentInChildren<Animator>();
				EffectManager.SpawnEffect(SpawnState.spawnEffectPrefab, new EffectData
				{
					origin = base.gameObject.transform.position
				}, false);
				this.playID = Util.PlayAttackSpeedSound(SpawnState.spawnSoundString, base.gameObject, 1.25f);
				this.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", 1f, animator);
				animator.SetBool("WeaponIsReady", false);
				yield return 60f;
				animator.SetBool("WeaponIsReady", true);
				yield break;
			}

			private void PlayAnimation(string layerName, string animationStateName, string playbackRateParam, float duration, Animator animator)
			{
				int layerIndex = animator.GetLayerIndex(layerName);
				animator.SetFloat(playbackRateParam, 1f);
				animator.PlayInFixedTime(animationStateName, layerIndex, 0f);
				animator.Update(0f);
				float length = animator.GetCurrentAnimatorStateInfo(layerIndex).length;
				animator.SetFloat(playbackRateParam, length / duration);
			}

			private uint playID;
		}
	}
}

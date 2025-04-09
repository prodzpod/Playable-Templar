using BepInEx.Configuration;
using EntityStates;
using EntityStates.ClayBruiserMonster;
using KinematicCharacterController;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections;
using Templar.TemplarSurvivor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace Templar
{
    public class Templar
    {
        internal static void TemplarSetup()
        {
            TokenSetup();
            TemplarSurvivor();
            SkillSetup();
            CreateMaster();
            TemplarSkins.RegisterSkins();
            if (EnableUnlocks.Value) Achievements.Patch();
        }

        internal static void TemplarSurvivor()
        {
            myCharacter = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").InstantiateClone("Templar_Survivor");
            myCharacter.GetComponent<ModelLocator>().modelTransform = myCharacter.transform.Find("ModelBase/mdlClayBruiser");
            myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.localScale = Vector3.one * 0.9f;
            myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.Translate(new Vector3(0f, 1.6f, 0f));
            foreach (KinematicCharacterMotor kinematicCharacterMotor in myCharacter.GetComponentsInChildren<KinematicCharacterMotor>())
            {
                kinematicCharacterMotor.SetCapsuleDimensions(kinematicCharacterMotor.Capsule.radius * 0.5f, kinematicCharacterMotor.Capsule.height * 0.5f, 0f);
            }
            myCharacter.GetComponent<CharacterBody>().aimOriginTransform.Translate(new Vector3(0f, 0f, 0f));
            myCharacter.GetComponent<SetStateOnHurt>().canBeHitStunned = false;
            myCharacter.tag = "Player";
            characterDisplay = myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject;
            // characterDisplay.transform.localScale = Vector3.one * 0.8f;
            characterDisplay.AddComponent<TemplarMenuAnim>();
            // characterDisplay.AddComponent<NetworkIdentity>();
            GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Pot2Body");
            MeshRenderer componentInChildren = gameObject.GetComponentInChildren<MeshRenderer>();
            GameObject gameObject2 = componentInChildren.gameObject.InstantiateClone("VagabondHead", false);
            UnityEngine.Object.Destroy(gameObject2.GetComponent<HurtBoxGroup>());
            UnityEngine.Object.Destroy(gameObject2.transform.GetComponentInChildren<HurtBox>());
            UnityEngine.Object.Destroy(gameObject2.transform.GetChild(0).gameObject);
            gameObject2.transform.parent = myCharacter.GetComponentInChildren<ChildLocator>().FindChild("Head");
            gameObject2.transform.localScale = new Vector3(1f, 1f, 1.25f);
            gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
            gameObject2.transform.localPosition = new Vector3(0f, -0.24f, 0f);
            MeshRenderer componentInChildren2 = gameObject2.GetComponentInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] baseRendererInfos = myCharacter.GetComponentInChildren<CharacterModel>().baseRendererInfos;
            CharacterModel.RendererInfo[] baseRendererInfos2 =
            [
                baseRendererInfos[0],
                baseRendererInfos[1],
                baseRendererInfos[2],
                baseRendererInfos[3],
                baseRendererInfos[4],
                baseRendererInfos[5],
                new() {
                    defaultMaterial = componentInChildren.material,
                    renderer = componentInChildren2,
                    defaultShadowCastingMode = ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            ];
            myCharacter.GetComponentInChildren<CharacterModel>().baseRendererInfos = baseRendererInfos2;
            CharacterCameraParams TemplarCam = ScriptableObject.CreateInstance<CharacterCameraParams>();
            TemplarCam.name = "TemplarCam";
            TemplarCam.data.minPitch = -70f;
            TemplarCam.data.maxPitch = 70f;
            TemplarCam.data.wallCushion = 0.1f;
            TemplarCam.data.pivotVerticalOffset = 0f;
            TemplarCam.data.idealLocalCameraPos = new Vector3(-0.35f, 2f, -12f);
            myCharacter.GetComponent<CameraTargetParams>().cameraParams = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CameraTargetParams>().cameraParams;

            CharacterBody component = myCharacter.GetComponent<CharacterBody>();
            component.portraitIcon = Assets.templarIcon;
            component.SetSpreadBloom(0f, false);
            component.spreadBloomCurve = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CharacterBody>().spreadBloomCurve;
            component.spreadBloomDecayTime = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CharacterBody>().spreadBloomDecayTime;
            component.name = "Templar_Survivor";
            component.baseNameToken = "Templar_Survivor";
            component.subtitleNameToken = "Templar_Subtitle";
            component.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            component.baseMaxHealth = baseHealth.Value;
            component.levelMaxHealth = healthGrowth.Value;
            component.baseRegen = baseRegen.Value;
            component.levelRegen = regenGrowth.Value;
            component.baseDamage = baseDamage.Value;
            component.levelDamage = damageGrowth.Value;
            component.baseArmor = baseArmor.Value;
            component.baseJumpPower = 15f;
            component.baseCrit = 1f;
            component.baseMoveSpeed = 7f;
            component.autoCalculateLevelStats = true;
            component._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
            component.preferredPodPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CharacterBody>().preferredPodPrefab;
            component.levelMoveSpeed = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().levelMoveSpeed;
            component.sprintingSpeedMultiplier = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().sprintingSpeedMultiplier;
            component.GetComponent<CharacterMotor>().mass = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponentInChildren<CharacterMotor>().mass;
            TemplarPrefab = myCharacter;
            SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
            survivorDef.cachedName = "Templar_Survivor";
            survivorDef.descriptionToken = "Templar_Description";
            survivorDef.primaryColor = CHAR_COLOR;
            survivorDef.bodyPrefab = TemplarPrefab;
            survivorDef.displayPrefab = characterDisplay;
            survivorDef.outroFlavorToken = "Templar_ENDING";
            survivorDef.desiredSortPosition = 16f;
            Main.survivorDefs.Add(survivorDef);
            CharacterBody body = myCharacter.GetComponent<CharacterBody>();
            body.portraitIcon = Assets.TemplarSkins.LoadAsset<Sprite>("Assets/texSurvivorTemplar.png").texture;
            body.baseNameToken = "TEMPLAR_SURVIVOR_NAME";
            body.subtitleNameToken = "TEMPLAR_SURVIVOR_SUBTITLE";
            KinematicCharacterMotor motor = body.GetComponent<KinematicCharacterMotor>();
            if (motor) motor.playerCharacter = true;
            TemplarPrefab.GetComponent<DeathRewards>().logUnlockableDef = null;
            Main.bodyPrefabs.Add(myCharacter);
        }

        internal static void SkillSetup()
        {
            GenericSkill[] componentsInChildren = myCharacter.GetComponentsInChildren<GenericSkill>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
            }
            SkillLocator component = myCharacter.GetComponent<SkillLocator>();
            PrimarySetup();
            SecondarySetup();
            UtilitySetup();
            SpecialSetup();
            PassiveSetup();
            Main.entityStates.Add(typeof(TemplarChargeMiniRocket));
            Main.entityStates.Add(typeof(TemplarChargeRocket));
            Main.entityStates.Add(typeof(TemplarFireMiniRocket));
            Main.entityStates.Add(typeof(TemplarFireRocket));
            Main.entityStates.Add(typeof(TemplarFireSonicBoom));
            Main.entityStates.Add(typeof(TemplarMinigunFire));
            Main.entityStates.Add(typeof(TemplarMinigunSpinDown));
            Main.entityStates.Add(typeof(TemplarMinigunSpinUp));
            Main.entityStates.Add(typeof(TemplarMinigunState));
            Main.entityStates.Add(typeof(TemplarRifleFire));
            Main.entityStates.Add(typeof(TemplarRifleSpinDown));
            Main.entityStates.Add(typeof(TemplarRifleState));
            Main.entityStates.Add(typeof(TemplarShotgun));
            Main.entityStates.Add(typeof(TemplarSidestep));
            Main.entityStates.Add(typeof(TemplarSwapWeapon));
            Main.entityStates.Add(typeof(TemplarThrowClaybomb));
            Main.entityStates.Add(typeof(TemplarChargeBeam));
            Main.entityStates.Add(typeof(TemplarFireBeam));
            Main.entityStates.Add(typeof(TemplarFlamethrower));
            Main.entityStates.Add(typeof(TemplarOverdrive));
            ContentAddition.AddSkillFamily(component.primary.skillFamily);
            ContentAddition.AddSkillFamily(component.secondary.skillFamily);
            ContentAddition.AddSkillFamily(component.utility.skillFamily);
            ContentAddition.AddSkillFamily(component.special.skillFamily);
            if (EnableMalignantOrigins.Value) MalignantOrigins.Patch();
        }

        internal static void PrimarySetup()
        {
            SkillLocator component = myCharacter.GetComponent<SkillLocator>();
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(TemplarMinigunSpinUp));
            skillDef.activationStateMachineName = "Weapon";
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0f;
            skillDef.beginSkillCooldownOnSkillEnd = true;
            skillDef.canceledFromSprinting = false;
            skillDef.cancelSprintingOnActivation = true;
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
            skillDef.keywordTokens =
            [
                "KEYWORD_RAPIDFIRE"
            ];


            component.primary = myCharacter.AddComponent<GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];

            component.primary.SetFieldValue("_skillFamily", skillFamily);
            SkillFamily skillFamily2 = component.primary.skillFamily;
            skillFamily2.variants[0] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };

            SkillDef skillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            skillDef2.activationState = new SerializableEntityStateType(typeof(TemplarRifleFire));
            skillDef2.activationStateMachineName = "Weapon";
            skillDef2.baseMaxStock = 1;
            skillDef2.baseRechargeInterval = 0f;
            skillDef2.beginSkillCooldownOnSkillEnd = true;
            skillDef2.canceledFromSprinting = true;
            skillDef2.cancelSprintingOnActivation = true;
            skillDef2.fullRestockOnAssign = true;
            skillDef2.interruptPriority = InterruptPriority.Any;
            skillDef2.isCombatSkill = true;
            skillDef2.mustKeyPress = false;
            skillDef2.rechargeStock = 1;
            skillDef2.requiredStock = 1;
            skillDef2.stockToConsume = 1;
            skillDef2.icon = Assets.icon1b;
            skillDef2.skillDescriptionToken = "TEMPLAR_PRIMARY_PRECISEMINIGUN_DESCRIPTION";
            skillDef2.skillName = "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
            skillDef2.skillNameToken = "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[^1] = new SkillFamily.Variant
            {
                skillDef = skillDef2,
                viewableNode = new ViewablesCatalog.Node(skillDef2.skillNameToken, false, null)
            };


            SkillDef skillDef3 = ScriptableObject.CreateInstance<SkillDef>();
            skillDef3.activationState = new SerializableEntityStateType(typeof(TemplarChargeBeam));
            skillDef3.activationStateMachineName = "Weapon";
            skillDef3.baseMaxStock = 1;
            skillDef3.baseRechargeInterval = 0f;
            skillDef3.beginSkillCooldownOnSkillEnd = true;
            skillDef3.canceledFromSprinting = false;
            skillDef3.cancelSprintingOnActivation = true;
            skillDef3.fullRestockOnAssign = true;
            skillDef3.interruptPriority = InterruptPriority.Any;
            skillDef3.isCombatSkill = true;
            skillDef3.mustKeyPress = false;
            skillDef3.rechargeStock = 1;
            skillDef3.requiredStock = 1;
            skillDef3.stockToConsume = 1;
            skillDef3.icon = Assets.icon1c;
            skillDef3.skillDescriptionToken = "TEMPLAR_PRIMARY_RAILGUN_DESCRIPTION";
            skillDef3.skillName = "TEMPLAR_PRIMARY_RAILGUN_NAME";
            skillDef3.skillNameToken = "TEMPLAR_PRIMARY_RAILGUN_NAME";
            skillDef3.keywordTokens =
            [
                "KEYWORD_EXPLOSIVE"
            ];
            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[^1] = new SkillFamily.Variant
            {
                skillDef = skillDef3,
                viewableNode = new ViewablesCatalog.Node(skillDef3.skillNameToken, false, null)
            };


            SkillDef skillDef4 = ScriptableObject.CreateInstance<SkillDef>();
            skillDef4.activationState = new SerializableEntityStateType(typeof(TemplarFlamethrower));
            skillDef4.activationStateMachineName = "Weapon";
            skillDef4.baseMaxStock = 1;
            skillDef4.baseRechargeInterval = 0f;
            skillDef4.beginSkillCooldownOnSkillEnd = true;
            skillDef4.canceledFromSprinting = false;
            skillDef4.cancelSprintingOnActivation = true;
            skillDef4.fullRestockOnAssign = true;
            skillDef4.interruptPriority = InterruptPriority.Any;
            skillDef4.isCombatSkill = true;
            skillDef4.mustKeyPress = false;
            skillDef4.rechargeStock = 1;
            skillDef4.requiredStock = 1;
            skillDef4.stockToConsume = 1;
            skillDef4.icon = Assets.icon1d;
            skillDef4.skillDescriptionToken = "TEMPLAR_PRIMARY_FLAMETHROWER_DESCRIPTION";
            skillDef4.skillName = "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
            skillDef4.skillNameToken = "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
            skillDef4.keywordTokens =
            [
                "KEYWORD_EXPLOSIVE"
            ];

            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[^1] = new SkillFamily.Variant
            {
                skillDef = skillDef4,
                viewableNode = new ViewablesCatalog.Node(skillDef4.skillNameToken, false, null)
            };

            bool value = bazookaGoBoom.Value;
            bool flag = value;
            if (flag)
            {
                LanguageAPI.Add("TEMPLAR_PRIMARY_BAZOOKA_NAME", "Bazooka Mk. 2");
                LanguageAPI.Add("TEMPLAR_PRIMARY_BAZOOKA_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + (miniBazookaDamageCoefficient.Value * 100f).ToString() + "% damage</style>.");
                SkillDef skillDef5 = ScriptableObject.CreateInstance<SkillDef>();
                skillDef5.activationState = new SerializableEntityStateType(typeof(TemplarChargeMiniRocket));
                skillDef5.activationStateMachineName = "Weapon";
                skillDef5.baseMaxStock = 1;
                skillDef5.baseRechargeInterval = 0.1f;
                skillDef5.beginSkillCooldownOnSkillEnd = true;
                skillDef5.canceledFromSprinting = false;
                skillDef5.fullRestockOnAssign = true;
                skillDef5.interruptPriority = InterruptPriority.Any;
                skillDef5.isCombatSkill = true;
                skillDef5.mustKeyPress = false;
                skillDef5.rechargeStock = 1;
                skillDef5.requiredStock = 1;
                skillDef5.stockToConsume = 1;
                skillDef5.icon = Assets.icon4;
                skillDef5.skillDescriptionToken = "TEMPLAR_PRIMARY_BAZOOKA_DESCRIPTION";
                skillDef5.skillName = "TEMPLAR_PRIMARY_BAZOOKA_NAME";
                skillDef5.skillNameToken = "TEMPLAR_PRIMARY_BAZOOKA_NAME";
                skillDef5.keywordTokens =
                [
                    "KEYWORD_EXPLOSIVE"
                ];
                skillDef5.cancelSprintingOnActivation = true;

                Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
                skillFamily2.variants[^1] = new SkillFamily.Variant
                {
                    skillDef = skillDef5,
                    viewableNode = new ViewablesCatalog.Node(skillDef5.skillNameToken, false, null)
                };
                Main.entityStates.Add(typeof(TemplarChargeMiniRocket));
                Main.skillDefs.Add(skillDef5);
            }


            Main.entityStates.Add(typeof(TemplarMinigunSpinUp));
            Main.entityStates.Add(typeof(TemplarRifleFire));
            Main.entityStates.Add(typeof(TemplarChargeBeam));
            Main.entityStates.Add(typeof(TemplarFlamethrower));

            Main.skillDefs.Add(skillDef);
            Main.skillDefs.Add(skillDef2);
            Main.skillDefs.Add(skillDef3);
            Main.skillDefs.Add(skillDef4);
        }

        internal static void SecondarySetup()
        {
            SkillLocator component = myCharacter.GetComponent<SkillLocator>();
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(TemplarThrowClaybomb));
            skillDef.activationStateMachineName = "Weapon";
            skillDef.baseMaxStock = clayGrenadeStock.Value;
            skillDef.baseRechargeInterval = clayGrenadeCooldown.Value;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.cancelSprintingOnActivation = true;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Skill;
            skillDef.isCombatSkill = true;
            skillDef.mustKeyPress = true;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.icon2;
            skillDef.skillDescriptionToken = "TEMPLAR_SECONDARY_GRENADE_DESCRIPTION";
            skillDef.skillName = "TEMPLAR_SECONDARY_GRENADE_NAME";
            skillDef.skillNameToken = "TEMPLAR_SECONDARY_GRENADE_NAME";

            component.secondary = myCharacter.AddComponent<GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];

            component.secondary.SetFieldValue("_skillFamily", skillFamily);
            SkillFamily skillFamily2 = component.secondary.skillFamily;
            skillFamily2.variants[0] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
            SkillDef skillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            skillDef2.activationState = new SerializableEntityStateType(typeof(TemplarShotgun));
            skillDef2.activationStateMachineName = "Weapon";
            skillDef2.baseMaxStock = blunderbussStock.Value;
            skillDef2.baseRechargeInterval = blunderbussCooldown.Value;
            skillDef2.beginSkillCooldownOnSkillEnd = false;
            skillDef2.canceledFromSprinting = false;
            skillDef2.cancelSprintingOnActivation = true;
            skillDef2.fullRestockOnAssign = true;
            skillDef2.interruptPriority = InterruptPriority.Skill;
            skillDef2.isCombatSkill = true;
            skillDef2.mustKeyPress = true;
            skillDef2.rechargeStock = 1;
            skillDef2.requiredStock = 1;
            skillDef2.stockToConsume = 1;
            skillDef2.icon = Assets.icon2b;
            skillDef2.skillDescriptionToken = "TEMPLAR_SECONDARY_SHOTGUN_DESCRIPTION";
            skillDef2.skillName = "TEMPLAR_SECONDARY_SHOTGUN_NAME";
            skillDef2.skillNameToken = "TEMPLAR_SECONDARY_SHOTGUN_NAME";

            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[^1] = new SkillFamily.Variant
            {
                skillDef = skillDef2,
                viewableNode = new ViewablesCatalog.Node(skillDef2.skillNameToken, false, null)
            };

            Main.entityStates.Add(typeof(TemplarThrowClaybomb));
            Main.entityStates.Add(typeof(TemplarShotgun));

            Main.skillDefs.Add(skillDef);
            Main.skillDefs.Add(skillDef2);

        }

        internal static void UtilitySetup()
        {
            SkillLocator component = myCharacter.GetComponent<SkillLocator>();
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(TemplarOverdrive));
            skillDef.activationStateMachineName = "Body";
            skillDef.baseMaxStock = overdriveStock.Value;
            skillDef.baseRechargeInterval = overdriveCooldown.Value;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.cancelSprintingOnActivation = false;
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

            component.utility = myCharacter.AddComponent<GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];
            component.utility.SetFieldValue("_skillFamily", skillFamily);
            SkillFamily skillFamily2 = component.utility.skillFamily;
            skillFamily2.variants[0] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
            SkillDef skillDef2 = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<SkillLocator>().utility.skillFamily.variants[0].skillDef;
            skillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            skillDef2.activationState = new SerializableEntityStateType(typeof(TemplarSidestep));
            skillDef2.activationStateMachineName = "Body";
            skillDef2.baseRechargeInterval = dashCooldown.Value;
            skillDef2.baseMaxStock = dashStock.Value;
            skillDef2.beginSkillCooldownOnSkillEnd = skillDef2.beginSkillCooldownOnSkillEnd;
            skillDef2.canceledFromSprinting = false;
            skillDef2.cancelSprintingOnActivation = false;
            skillDef2.fullRestockOnAssign = skillDef2.fullRestockOnAssign;
            skillDef2.interruptPriority = skillDef2.interruptPriority;
            skillDef2.isCombatSkill = true;
            skillDef2.mustKeyPress = skillDef2.mustKeyPress;
            skillDef2.rechargeStock = skillDef2.rechargeStock;
            skillDef2.requiredStock = skillDef2.requiredStock;
            skillDef2.stockToConsume = skillDef2.stockToConsume;
            skillDef2.icon = Assets.icon3b;
            skillDef2.skillDescriptionToken = "TEMPLAR_UTILITY_DODGE_DESCRIPTION";
            skillDef2.skillName = "TEMPLAR_UTILITY_DODGE_NAME";
            skillDef2.skillNameToken = "TEMPLAR_UTILITY_DODGE_NAME";

            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[^1] = new SkillFamily.Variant
            {
                skillDef = skillDef2,
                viewableNode = new ViewablesCatalog.Node(skillDef2.skillNameToken, false, null)
            };

            Main.entityStates.Add(typeof(TemplarOverdrive));
            Main.entityStates.Add(typeof(TemplarSidestep));

            Main.skillDefs.Add(skillDef);
            Main.skillDefs.Add(skillDef2);
        }

        internal static void SpecialSetup()
        {
            SkillLocator component = myCharacter.GetComponent<SkillLocator>();
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeRocket));
            skillDef.activationStateMachineName = "Weapon";
            skillDef.baseMaxStock = bazookaStock.Value;
            skillDef.baseRechargeInterval = bazookaCooldown.Value;
            skillDef.beginSkillCooldownOnSkillEnd = true;
            skillDef.canceledFromSprinting = false;
            skillDef.cancelSprintingOnActivation = true;
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
            skillDef.keywordTokens =
            [
                "KEYWORD_EXPLOSIVE"
            ];


            component.special = myCharacter.AddComponent<GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];

            component.special.SetFieldValue("_skillFamily", skillFamily);
            SkillFamily skillFamily2 = component.special.skillFamily;
            skillFamily2.variants[0] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };

            SkillDef skillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            skillDef2.activationState = new SerializableEntityStateType(typeof(TemplarSwapWeapon));
            skillDef2.activationStateMachineName = "Weapon";
            skillDef2.baseMaxStock = 1;
            skillDef2.baseRechargeInterval = 0.1f;
            skillDef2.beginSkillCooldownOnSkillEnd = true;
            skillDef2.canceledFromSprinting = false;
            skillDef2.cancelSprintingOnActivation = true;
            skillDef2.fullRestockOnAssign = true;
            skillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            skillDef2.isCombatSkill = false;
            skillDef2.mustKeyPress = false;
            skillDef2.rechargeStock = 1;
            skillDef2.requiredStock = 1;
            skillDef2.stockToConsume = 1;
            skillDef2.icon = Assets.icon4b;
            skillDef2.skillDescriptionToken = "TEMPLAR_SPECIAL_SWAP_DESCRIPTION";
            skillDef2.skillName = "TEMPLAR_SPECIAL_SWAP_NAME";
            skillDef2.skillNameToken = "TEMPLAR_SPECIAL_SWAP_NAME";

            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[^1] = new SkillFamily.Variant
            {
                skillDef = skillDef2,
                viewableNode = new ViewablesCatalog.Node(skillDef2.skillNameToken, false, null)
            };

            Main.entityStates.Add(typeof(TemplarChargeRocket));
            Main.entityStates.Add(typeof(TemplarSwapWeapon));

            Main.skillDefs.Add(skillDef);
            Main.skillDefs.Add(skillDef2);
        }

        internal static void CreateMaster()
        {
            doppelganger = LegacyResourcesAPI.Load<GameObject>("prefabs/charactermasters/commandomonstermaster").InstantiateClone("TemplarMonsterMaster");
            doppelganger.GetComponent<CharacterMaster>().bodyPrefab = myCharacter;
            Main.masterPrefabs.Add(doppelganger);
        }

        internal static void PassiveSetup()
        {
            SkillLocator component = myCharacter.GetComponent<SkillLocator>();
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
            LanguageAPI.Add("TEMPLAR_PRIMARY_MINIGUN_DESCRIPTION", "<style=cIsDamage>Rapidfire</style>. Rev up and fire your <style=cIsUtility>minigun</style>, dealing <style=cIsDamage>" + (Templar.minigunDamageCoefficient.Value * 100f).ToString() + "% damage</style> per bullet. <style=cIsUtility>Slow</style> your movement while shooting, but gain <style=cIsHealing>bonus armor</style>.");
            LanguageAPI.Add("TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME", "Tar Rifle");
            LanguageAPI.Add("TEMPLAR_PRIMARY_PRECISEMINIGUN_DESCRIPTION", "Fire a <style=cIsUtility>tar rifle</style>, dealing <style=cIsDamage>" + (rifleDamageCoefficient.Value * 100f).ToString() + "% damage</style> per bullet and applying <style=cIsUtility>tar</style> with high accuracy.");
            LanguageAPI.Add("TEMPLAR_PRIMARY_RAILGUN_NAME", "Railgun");
            LanguageAPI.Add("TEMPLAR_PRIMARY_RAILGUN_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Charge up and fire a <style=cIsUtility>piercing bullet</style>, dealing <style=cIsDamage>800% damage</style>.");
            LanguageAPI.Add("TEMPLAR_PRIMARY_FLAMETHROWER_NAME", "Flamethrower");
            LanguageAPI.Add("TEMPLAR_PRIMARY_FLAMETHROWER_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>continuous stream of flames</style>, dealing <style=cIsDamage>1250% damage per second</style>.");
            text = "Throw a <style=cIsUtility>clay bomb</style> that explodes for <style=cIsDamage>" + (clayGrenadeDamageCoefficient.Value * 100f).ToString() + "% damage</style> and inflicts <style=cIsUtility>tar</style>.";
            bool flag = clayGrenadeStock.Value > 1;
            bool flag2 = flag;
            if (flag2)
            {
                text = string.Concat(new object[]
                {
                    text,
                    " Hold up to <style=cIsDamage>",
                    clayGrenadeStock.Value,
                    "</style> bombs."
                });
            }
            LanguageAPI.Add("TEMPLAR_SECONDARY_GRENADE_NAME", "Clay Bomb");
            LanguageAPI.Add("TEMPLAR_SECONDARY_GRENADE_DESCRIPTION", text);
            text = string.Concat(new object[]
            {
                "Fire a burst of <style=cIsUtility>pellets</style>, dealing <style=cIsDamage>",
                blunderbussPelletCount.Value,
                "x",
                blunderbussDamageCoefficient.Value * 100f,
                "% damage</style>."
            });
            bool flag3 = blunderbussStock.Value > 1;
            bool flag4 = flag3;
            if (flag4)
            {
                text = string.Concat(new object[]
                {
                    text,
                    " Store up to ",
                    blunderbussStock.Value,
                    " shots."
                });
            }
            LanguageAPI.Add("TEMPLAR_SECONDARY_SHOTGUN_NAME", "Blunderbuss");
            LanguageAPI.Add("TEMPLAR_SECONDARY_SHOTGUN_DESCRIPTION", text);
            LanguageAPI.Add("TEMPLAR_UTILITY_OVERDRIVE_NAME", "Tar Overdrive");
            LanguageAPI.Add("TEMPLAR_UTILITY_OVERDRIVE_DESCRIPTION", "Shift into <style=cIsUtility>maximum overdrive</style>, knocking enemies back and applying <style=cIsUtility>tar</style>. <style=cIsUtility>Gain bonus attack speed and health regen for 3 seconds</style>.");
            text = "<style=cIsUtility>Dash</style> a short distance. Can dash while shooting.";
            bool flag5 = dashStock.Value > 1;
            bool flag6 = flag5;
            if (flag6)
            {
                text = string.Concat(new object[]
                {
                    text,
                    " Store up to ",
                    dashStock.Value,
                    " charges."
                });
            }
            LanguageAPI.Add("TEMPLAR_UTILITY_DODGE_NAME", "Sidestep");
            LanguageAPI.Add("TEMPLAR_UTILITY_DODGE_DESCRIPTION", text);
            LanguageAPI.Add("TEMPLAR_SPECIAL_FIRE_NAME", "Bazooka");
            LanguageAPI.Add("TEMPLAR_SPECIAL_FIRE_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + (bazookaDamageCoefficient.Value * 100f).ToString() + "% damage</style>.");
            LanguageAPI.Add("TEMPLAR_SPECIAL_SWAP_NAME", "Weapon Swap");
            LanguageAPI.Add("TEMPLAR_SPECIAL_SWAP_DESCRIPTION", "<style=cIsUtility>Swap</style> your <style=cIsDamage>primary</style> weapon type.");
        }

        public class TemplarMissileController : MonoBehaviour
        {
            private void Awake()
            {
                rb = GetComponentInChildren<Rigidbody>();
                speed = startSpeed;
            }

            private void FixedUpdate()
            {
                speed += acceleration;
                rb.velocity = transform.forward * speed;
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
                transform.SetParent(null);
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
                    Collider[] array = Physics.OverlapSphere(transform.position, radius);
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
                                    float num = 16f / Vector3.Distance(componentInChildren.transform.position, transform.position);
                                    Vector3 vector = new(0f, Mathf.Clamp(num * force, 0f, maxForce), 0f);
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
                StartCoroutine(SpawnAnim());
            }

            internal void OnDisable()
            {
                bool flag = playID > 0U;
                bool flag2 = flag;
                if (flag2)
                {
                    AkSoundEngine.StopPlayingID(playID);
                }
            }

            private IEnumerator SpawnAnim()
            {
                Animator animator = GetComponentInChildren<Animator>();
                EffectManager.SpawnEffect(SpawnState.spawnEffectPrefab, new EffectData
                {
                    origin = gameObject.transform.position
                }, false);
                playID = Util.PlayAttackSpeedSound(SpawnState.spawnSoundString, gameObject, 1.25f);
                PlayAnimation("Body", "Spawn", "Spawn.playbackRate", 1f, animator);
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

        public static GameObject myCharacter;
        public static GameObject TemplarPrefab;
        public static GameObject characterDisplay;
        public static GameObject doppelganger;
        private static readonly Color CHAR_COLOR = new(0.784313738f, 0.294117659f, 0.05882353f);
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
        public static ConfigEntry<bool> EnableTemplarSkin;
        public static ConfigEntry<bool> EnableEngiVoidSkin;
        public static ConfigEntry<bool> EnableMalignantOrigins;
        public static ConfigEntry<bool> EnableUnlocks;
    }
}

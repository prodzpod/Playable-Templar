using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Templar
{
	public static class TemplarItemDisplays
	{
		internal static void InitializeItemDisplays()
		{
			TemplarItemDisplays.PopulateDisplays();
			CharacterModel componentInChildren = Templar.myCharacter.GetComponentInChildren<CharacterModel>();
			TemplarItemDisplays.itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
			TemplarItemDisplays.itemDisplayRuleSet.name = "idrsTemplar";
			componentInChildren.itemDisplayRuleSet = TemplarItemDisplays.itemDisplayRuleSet;
		}

		internal static void SetItemDisplays()
		{
			TemplarItemDisplays.itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.AffixRed,
				displayRuleGroup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().itemDisplayRuleSet.FindDisplayRuleGroup(RoR2Content.Equipment.AffixRed)
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.AffixBlue,
				displayRuleGroup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().itemDisplayRuleSet.FindDisplayRuleGroup(RoR2Content.Equipment.AffixBlue)
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.AffixWhite,
				displayRuleGroup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().itemDisplayRuleSet.FindDisplayRuleGroup(RoR2Content.Equipment.AffixWhite)
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.AffixPoison,
				displayRuleGroup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().itemDisplayRuleSet.FindDisplayRuleGroup(RoR2Content.Equipment.AffixPoison)
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.AffixHaunted,
				displayRuleGroup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<CharacterModel>().itemDisplayRuleSet.FindDisplayRuleGroup(RoR2Content.Equipment.AffixHaunted)
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.Jetpack,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBugWings"),
							childName = "Head",
							localPos = new Vector3(-0.15f, -0.25f, 0f),
							localAngles = new Vector3(0f, 45f, 0f),
							localScale = new Vector3(0.25f, 0.25f, 0.25f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.GoldGat,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayGoldGat"),
							childName = "Muzzle",
							localPos = new Vector3(1f, 0.8f, -1f),
							localAngles = new Vector3(45f, 90f, 0f),
							localScale = new Vector3(0.5f, 0.5f, 0.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.CritGlasses,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayGlasses"),
							childName = "Head",
							localPos = new Vector3(0f, 0.4f, 0.25f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.5f, 0.5f, 0.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.AttackSpeedOnCrit,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayWolfPelt"),
							childName = "Head",
							localPos = new Vector3(0f, 0.5f, 0.067f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.JumpBoost,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayWaxBird"),
							childName = "Head",
							localPos = new Vector3(0f, -0.7f, -0.05f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(2f, 2f, 2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Bandolier,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBandolier"),
							childName = "Muzzle",
							localPos = new Vector3(0.05f, -0.05f, -1.75f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.75f, 1f, 1f),
							limbMask = LimbFlags.None
						},
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBandolier"),
							childName = "Head",
							localPos = new Vector3(-0.2f, -0.5f, 0.25f),
							localAngles = new Vector3(-45f, 180f, 0f),
							localScale = new Vector3(1f, 2f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.DeathMark,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayDeathMark"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -0.25f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.1f, 0.1f, 0.1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.WarCryOnMultiKill,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayPauldron"),
							childName = "Head",
							localPos = new Vector3(0f, 0.1f, -0.25f),
							localAngles = new Vector3(-90f, 0f, 0f),
							localScale = new Vector3(1.5f, 1.5f, 1.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Mushroom,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayMushroom"),
							childName = "Head",
							localPos = new Vector3(-0.25f, 0f, 0f),
							localAngles = new Vector3(0f, 0f, 45f),
							localScale = new Vector3(0.25f, 0.25f, 0.25f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.BarrierOnOverHeal,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayAegis"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0.25f, -1f),
							localAngles = new Vector3(180f, 180f, 180f),
							localScale = new Vector3(0.75f, 0.75f, 0.75f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Behemoth,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBehemoth"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -0.9f),
							localAngles = new Vector3(90f, 0f, 0f),
							localScale = new Vector3(0.5f, 0.5f, 0.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.NearbyDamageBonus,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayDiamond"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -2.75f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.7f, 0.7f, 0.7f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.FireRing,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayFireRing"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -1f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(3.5f, 3.5f, 3.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.IceRing,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayIceRing"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -1.2f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(3.5f, 3.5f, 3.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.ArmorPlate,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
							childName = "Head",
							localPos = new Vector3(-0.4f, -0.5f, 0.15f),
							localAngles = new Vector3(-90f, 260f, 45f),
							localScale = new Vector3(0.8f, 0.6f, 0.6f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Bear,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBear"),
							childName = "Head",
							localPos = new Vector3(0f, 0f, -0.4f),
							localAngles = new Vector3(0f, 180f, 0f),
							localScale = new Vector3(0.6f, 0.6f, 0.6f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Medkit,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayMedkit"),
							childName = "Head",
							localPos = new Vector3(-0.4f, -0.5f, 0.15f),
							localAngles = new Vector3(-90f, -90f, 0f),
							localScale = new Vector3(1.5f, 1.5f, 1.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Dagger,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayDagger"),
							childName = "Head",
							localPos = new Vector3(0f, 0f, 0f),
							localAngles = new Vector3(-90f, -90f, 0f),
							localScale = new Vector3(2f, 2f, 2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.ChainLightning,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayUkulele"),
							childName = "Muzzle",
							localPos = new Vector3(0.4f, 0f, -1.75f),
							localAngles = new Vector3(0f, 85f, 90f),
							localScale = new Vector3(0.8f, 0.8f, 0.8f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Syringe,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplaySyringeCluster"),
							childName = "Head",
							localPos = new Vector3(0f, 0f, -0.15f),
							localAngles = new Vector3(-60f, 0f, 0f),
							localScale = new Vector3(0.5f, 0.5f, 0.5f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.ArmorReductionOnHit,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayWarhammer"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0.25f, 0f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.FallBoots,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayGravBoots"),
							childName = "FootL",
							localPos = new Vector3(0f, -0.05f, 0f),
							localAngles = new Vector3(45f, 0f, 0f),
							localScale = new Vector3(0.25f, 0.25f, 0.25f),
							limbMask = LimbFlags.None
						},
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayGravBoots"),
							childName = "FootR",
							localPos = new Vector3(0f, -0.05f, 0f),
							localAngles = new Vector3(70f, 0f, 0f),
							localScale = new Vector3(0.25f, 0.25f, 0.25f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.BounceNearby,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayHook"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -0.25f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.UtilitySkillMagazine,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0f, -0.75f),
							localAngles = new Vector3(0f, -90f, 90f),
							localScale = new Vector3(2f, 2f, 2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Hoof,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayHoof"),
							childName = "FootL",
							localPos = new Vector3(0f, 0.05f, -0.15f),
							localAngles = new Vector3(50f, 180f, 180f),
							localScale = new Vector3(0.15f, 0.15f, 0.075f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.HealWhileSafe,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplaySnail"),
							childName = "FootR",
							localPos = new Vector3(0f, -0.05f, 0f),
							localAngles = new Vector3(0f, 0f, 180f),
							localScale = new Vector3(0.1f, 0.1f, 0.1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.LunarPrimaryReplacement,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBirdEye"),
							childName = "Muzzle",
							localPos = new Vector3(0f, -0.1f, -0.25f),
							localAngles = new Vector3(-90f, 0f, 0f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.SecondarySkillMagazine,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayDoubleMag"),
							childName = "Muzzle",
							localPos = new Vector3(0f, -0.8f, -2f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(-0.2f, -0.2f, -0.2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Pearl,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayPearl"),
							childName = "Head",
							localPos = new Vector3(-1f, 0f, 0f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.2f, 0.2f, 0.2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.ShinyPearl,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayShinyPearl"),
							childName = "Head",
							localPos = new Vector3(1f, 0f, 0f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.2f, 0.2f, 0.2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.SprintOutOfCombat,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayWhip"),
							childName = "Head",
							localPos = new Vector3(-0.5f, -1.2f, 0.25f),
							localAngles = new Vector3(0f, 0f, -20f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.Fruit,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayFruit"),
							childName = "Head",
							localPos = new Vector3(0.6f, -1.2f, 0f),
							localAngles = new Vector3(0f, -45f, 45f),
							localScale = new Vector3(0.75f, 0.75f, 0.75f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.BFG,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayBFG"),
							childName = "Muzzle",
							localPos = new Vector3(0f, 0.2f, -0.25f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.75f, 0.75f, 0.75f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.Meteor,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayMeteor"),
							childName = "Root",
							localPos = new Vector3(0f, 2f, -0.75f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(2f, 2f, 2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Equipment.Blackhole,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayGravCube"),
							childName = "Root",
							localPos = new Vector3(0f, 2f, -0.75f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Icicle,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayFrostRelic"),
							childName = "Root",
							localPos = new Vector3(-1f, 2f, -1f),
							localAngles = new Vector3(90f, 0f, 0f),
							localScale = new Vector3(2f, 2f, 2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.Talisman,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayTalisman"),
							childName = "Root",
							localPos = new Vector3(1f, 2f, -1f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(1f, 1f, 1f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
			{
				keyAsset = RoR2Content.Items.FocusConvergence,
				displayRuleGroup = new DisplayRuleGroup
				{
					rules = new ItemDisplayRule[]
					{
						new ItemDisplayRule
						{
							ruleType = ItemDisplayRuleType.ParentedPrefab,
							followerPrefab = TemplarItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
							childName = "Root",
							localPos = new Vector3(0.5f, 2.5f, -3f),
							localAngles = new Vector3(0f, 0f, 0f),
							localScale = new Vector3(0.2f, 0.2f, 0.2f),
							limbMask = LimbFlags.None
						}
					}
				}
			});
			TemplarItemDisplays.itemDisplayRuleSet.keyAssetRuleGroups = TemplarItemDisplays.itemDisplayRules.ToArray();
			TemplarItemDisplays.itemDisplayRuleSet.GenerateRuntimeValues();
		}

		internal static void PopulateDisplays()
		{
			ItemDisplayRuleSet itemDisplayRuleSet = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
			ItemDisplayRuleSet.KeyAssetRuleGroup[] keyAssetRuleGroups = itemDisplayRuleSet.keyAssetRuleGroups;
			for (int i = 0; i < keyAssetRuleGroups.Length; i++)
			{
				ItemDisplayRule[] rules = keyAssetRuleGroups[i].displayRuleGroup.rules;
				for (int j = 0; j < rules.Length; j++)
				{
					GameObject followerPrefab = rules[j].followerPrefab;
					bool flag = followerPrefab;
					if (flag)
					{
						string name = followerPrefab.name;
						string key = (name != null) ? name.ToLower() : null;
						bool flag2 = !TemplarItemDisplays.itemDisplayPrefabs.ContainsKey(key);
						if (flag2)
						{
							TemplarItemDisplays.itemDisplayPrefabs[key] = followerPrefab;
						}
					}
				}
			}
		}

		internal static GameObject LoadDisplay(string name)
		{
			bool flag = TemplarItemDisplays.itemDisplayPrefabs.ContainsKey(name.ToLower());
			if (flag)
			{
				bool flag2 = TemplarItemDisplays.itemDisplayPrefabs[name.ToLower()];
				if (flag2)
				{
					return TemplarItemDisplays.itemDisplayPrefabs[name.ToLower()];
				}
			}
			return null;
		}

		internal static ItemDisplayRuleSet itemDisplayRuleSet;

		internal static List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules;

		private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();
	}
}

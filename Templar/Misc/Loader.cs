using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using RoR2.Skills;

namespace Templar
{
	[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin("com.Tymmey.Templar", "Templar", "1.1.1")]
	[BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
	[R2APISubmoduleDependency(new string[]
	{
		"PrefabAPI",
		"SurvivorAPI",
		"LanguageAPI",
		"LoadoutAPI",
		"BuffAPI",
		"EffectAPI"
	}
	)]
	public class Loader : BaseUnityPlugin
	{
		public void Awake()
		{
			this.ReadConfig();
			Assets.Initialize();
			Buffs.RegisterBuffs();
			Projectiles.ProjectileSetup();
			Hook.HookSetup();
			Templar.TemplarSetup();
			TemplarItemDisplays.InitializeItemDisplays();
			ContentManager.collectContentPackProviders += this.ContentManager_collectContentPackProviders;

			RoR2Application.onLoad += AddItemDisplays;
		}

		public void ReadConfig()
		{
			Templar.baseDamage = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Damage"), 12f, new ConfigDescription("Base damage", null, Array.Empty<object>()));
			Templar.damageGrowth = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Damage growth"), 2.4f, new ConfigDescription("Damage per level", null, Array.Empty<object>()));
			Templar.baseHealth = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Health"), 200f, new ConfigDescription("Base health", null, Array.Empty<object>()));
			Templar.healthGrowth = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Health growth"), 48f, new ConfigDescription("Health per level", null, Array.Empty<object>()));
			Templar.baseArmor = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Armor"), 15f, new ConfigDescription("Base armor", null, Array.Empty<object>()));
			Templar.baseRegen = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Regen"), 0.5f, new ConfigDescription("Base HP regen", null, Array.Empty<object>()));
			Templar.regenGrowth = base.Config.Bind<float>(new ConfigDefinition("1 - Templar", "Regen growth"), 0.5f, new ConfigDescription("HP regen per level", null, Array.Empty<object>()));
			Templar.bazookaGoBoom = base.Config.Bind<bool>(new ConfigDefinition("01 - Templar", "Mini Bazooka Primary"), false, new ConfigDescription("Enables Bazooka Mk.2", null, Array.Empty<object>()));
			Templar.minigunDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Damage"), 0.4f, new ConfigDescription("Minigun damage per bullet", null, Array.Empty<object>()));
			Templar.minigunProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Proc Coefficient"), 0.4f, new ConfigDescription("Minigun proc coefficient per bullet", null, Array.Empty<object>()));
			Templar.minigunForce = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Force"), 3f, new ConfigDescription("Minigun bullet force", null, Array.Empty<object>()));
			Templar.minigunArmorBoost = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Armor Bonus"), 50f, new ConfigDescription("Bonus armor while firing", null, Array.Empty<object>()));
			Templar.minigunStationaryArmorBoost = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Stationary Armor Bonus"), 100f, new ConfigDescription("Bonus armor while standing still and firing", null, Array.Empty<object>()));
			Templar.minigunMinFireRate = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Minimum Fire Rate"), 0.75f, new ConfigDescription("Starting fire rate", null, Array.Empty<object>()));
			Templar.minigunMaxFireRate = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Maximum Fire Rate"), 1.35f, new ConfigDescription("Max fire rate", null, Array.Empty<object>()));
			Templar.minigunFireRateGrowth = base.Config.Bind<float>(new ConfigDefinition("2 - Minigun", "Fire Rate Growth"), 0.01f, new ConfigDescription("Amount the fire rate increases per shot", null, Array.Empty<object>()));
			Templar.rifleDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("3 - Tar Rifle", "Damage"), 0.5f, new ConfigDescription("Rifle damage per bullet", null, Array.Empty<object>()));
			Templar.rifleProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("3 - Tar Rifle", "Proc Coefficient"), 0.7f, new ConfigDescription("Rifle proc coefficient per bullet", null, Array.Empty<object>()));
			Templar.rifleForce = base.Config.Bind<float>(new ConfigDefinition("3 - Tar Rifle", "Force"), 5f, new ConfigDescription("Rifle bullet force", null, Array.Empty<object>()));
			Templar.rifleFireRate = base.Config.Bind<float>(new ConfigDefinition("3 - Tar Rifle", "Fire Rate"), 1.6f, new ConfigDescription("Higher value = lower fire rate, due to spaghetti code", null, Array.Empty<object>()));
			Templar.clayGrenadeStock = base.Config.Bind<int>(new ConfigDefinition("4 - Clay Bomb", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			Templar.clayGrenadeCooldown = base.Config.Bind<float>(new ConfigDefinition("4 - Clay Bomb", "Cooldown"), 5f, new ConfigDescription("Bomb cooldown", null, Array.Empty<object>()));
			Templar.clayGrenadeDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("4 - Clay Bomb", "Damage"), 4f, new ConfigDescription("Explosion damage", null, Array.Empty<object>()));
			Templar.clayGrenadeProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("4 - Clay Bomb", "Proc Coefficient"), 0.8f, new ConfigDescription("Explosion proc coefficient", null, Array.Empty<object>()));
			Templar.clayGrenadeRadius = base.Config.Bind<float>(new ConfigDefinition("4 - Clay Bomb", "Radius"), 12f, new ConfigDescription("Explosion radius", null, Array.Empty<object>()));
			Templar.clayGrenadeDetonationTime = base.Config.Bind<float>(new ConfigDefinition("4 - Clay Bomb", "Detonation Time"), 0.15f, new ConfigDescription("The time it takes to explode after hitting something", null, Array.Empty<object>()));
			Templar.blunderbussStock = base.Config.Bind<int>(new ConfigDefinition("5 - Blunderbuss", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			Templar.blunderbussCooldown = base.Config.Bind<float>(new ConfigDefinition("5 - Blunderbuss", "Cooldown"), 2.5f, new ConfigDescription("Cooldown per shot", null, Array.Empty<object>()));
			Templar.blunderbussPelletCount = base.Config.Bind<int>(new ConfigDefinition("5 - Blunderbuss", "Pellet count"), 6, new ConfigDescription("Pellets fired per shot", null, Array.Empty<object>()));
			Templar.blunderbussDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("5 - Blunderbuss", "Damage"), 1f, new ConfigDescription("Pellet damage", null, Array.Empty<object>()));
			Templar.blunderbussProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("5 - Blunderbuss", "Proc Coefficient"), 1f, new ConfigDescription("Pellet proc coefficient", null, Array.Empty<object>()));
			Templar.blunderbussSpread = base.Config.Bind<float>(new ConfigDefinition("5 - Blunderbuss", "Pellet spread"), 10f, new ConfigDescription("Pellet spread when fired", null, Array.Empty<object>()));
			Templar.tarStock = base.Config.Bind<int>(new ConfigDefinition("6 - Tar Blast", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			Templar.tarCooldown = base.Config.Bind<float>(new ConfigDefinition("6 - Tar Blast", "Cooldown"), 1f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			Templar.overdriveStock = base.Config.Bind<int>(new ConfigDefinition("6 - Tar Overdrive", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			Templar.overdriveCooldown = base.Config.Bind<float>(new ConfigDefinition("6 - Tar Overdrive", "Cooldown"), 12f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			Templar.dashStock = base.Config.Bind<int>(new ConfigDefinition("7 - Sidestep", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			Templar.dashCooldown = base.Config.Bind<float>(new ConfigDefinition("7 - Sidestep", "Cooldown"), 7f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			Templar.bazookaStock = base.Config.Bind<int>(new ConfigDefinition("8 - Bazooka", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			Templar.bazookaCooldown = base.Config.Bind<float>(new ConfigDefinition("8 - Bazooka", "Cooldown"), 14f, new ConfigDescription("Bazooka cooldown", null, Array.Empty<object>()));
			Templar.bazookaDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("8 - Bazooka", "Damage"), 12f, new ConfigDescription("Bazooka damage", null, Array.Empty<object>()));
			Templar.bazookaProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("8 - Bazooka", "Proc Coefficient"), 0.6f, new ConfigDescription("Bazooka proc coefficient", null, Array.Empty<object>()));
			Templar.bazookaBlastRadius = base.Config.Bind<float>(new ConfigDefinition("8 - Bazooka", "Blast Radius"), 16f, new ConfigDescription("Bazooka blast radius", null, Array.Empty<object>()));
			Templar.miniBazookaDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("8 - Bazooka Mk.2", "Damage"), 5f, new ConfigDescription("Bazooka Mk.2 damage", null, Array.Empty<object>()));
		}

		public static void AddItemDisplays()
		{
			TemplarItemDisplays.SetItemDisplays();
		}

		private void ContentManager_collectContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
		{
			addContentPackProvider(new ContentPacks());
		}

		public static bool IsBUDefined { get; private set; }

		internal static List<GameObject> projectilePrefabs = new List<GameObject>();
		internal static List<SurvivorDef> survivorDefs = new List<SurvivorDef>();
		public static List<GameObject> bodyPrefabs = new List<GameObject>();
		internal static List<GameObject> masterPrefabs = new List<GameObject>();
		internal static List<Type> entityStates = new List<Type>();
		internal static List<SkillDef> skillDefs = new List<SkillDef>();
		internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();


	}
}

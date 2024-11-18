using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using RoR2.Skills;
using BepInEx.Logging;

namespace Templar
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.Tymmey.Templar", "Templar", "1.1.2")]
    public class Loader : BaseUnityPlugin
    {
        public static Loader Instance;
        public static ManualLogSource Log;
        public static PluginInfo pluginInfo;
        public void Awake()
        {
            Instance = this;
            Log = Logger;
            pluginInfo = Info;
            ReadConfig();
            Assets.Initialize();
            Buffs.RegisterBuffs();
            Projectiles.ProjectileSetup();
            Hook.HookSetup();
            Templar.TemplarSetup();
            TemplarItemDisplays.InitializeItemDisplays();
            ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
            RoR2Application.onLoad += AddItemDisplays;
        }

        public void ReadConfig()
        {
            Templar.baseDamage = Config.Bind(new ConfigDefinition("1 - Templar", "Damage"), 12f, new ConfigDescription("Base damage", null, []));
            Templar.damageGrowth = Config.Bind(new ConfigDefinition("1 - Templar", "Damage growth"), 2.4f, new ConfigDescription("Damage per level", null, []));
            Templar.baseHealth = Config.Bind(new ConfigDefinition("1 - Templar", "Health"), 200f, new ConfigDescription("Base health", null, []));
            Templar.healthGrowth = Config.Bind(new ConfigDefinition("1 - Templar", "Health growth"), 48f, new ConfigDescription("Health per level", null, []));
            Templar.baseArmor = Config.Bind(new ConfigDefinition("1 - Templar", "Armor"), 15f, new ConfigDescription("Base armor", null, []));
            Templar.baseRegen = Config.Bind(new ConfigDefinition("1 - Templar", "Regen"), 0.5f, new ConfigDescription("Base HP regen", null, []));
            Templar.regenGrowth = Config.Bind(new ConfigDefinition("1 - Templar", "Regen growth"), 0.5f, new ConfigDescription("HP regen per level", null, []));
            Templar.bazookaGoBoom = Config.Bind(new ConfigDefinition("01 - Templar", "Mini Bazooka Primary"), false, new ConfigDescription("Enables Bazooka Mk.2", null, []));
            Templar.minigunDamageCoefficient = Config.Bind(new ConfigDefinition("2 - Minigun", "Damage"), 0.4f, new ConfigDescription("Minigun damage per bullet", null, []));
            Templar.minigunProcCoefficient = Config.Bind(new ConfigDefinition("2 - Minigun", "Proc Coefficient"), 0.4f, new ConfigDescription("Minigun proc coefficient per bullet", null, []));
            Templar.minigunForce = Config.Bind(new ConfigDefinition("2 - Minigun", "Force"), 3f, new ConfigDescription("Minigun bullet force", null, []));
            Templar.minigunArmorBoost = Config.Bind(new ConfigDefinition("2 - Minigun", "Armor Bonus"), 50f, new ConfigDescription("Bonus armor while firing", null, []));
            Templar.minigunStationaryArmorBoost = Config.Bind(new ConfigDefinition("2 - Minigun", "Stationary Armor Bonus"), 100f, new ConfigDescription("Bonus armor while standing still and firing", null, []));
            Templar.minigunMinFireRate = Config.Bind(new ConfigDefinition("2 - Minigun", "Minimum Fire Rate"), 0.75f, new ConfigDescription("Starting fire rate", null, []));
            Templar.minigunMaxFireRate = Config.Bind(new ConfigDefinition("2 - Minigun", "Maximum Fire Rate"), 1.35f, new ConfigDescription("Max fire rate", null, []));
            Templar.minigunFireRateGrowth = Config.Bind(new ConfigDefinition("2 - Minigun", "Fire Rate Growth"), 0.01f, new ConfigDescription("Amount the fire rate increases per shot", null, []));
            Templar.rifleDamageCoefficient = Config.Bind(new ConfigDefinition("3 - Tar Rifle", "Damage"), 0.5f, new ConfigDescription("Rifle damage per bullet", null, []));
            Templar.rifleProcCoefficient = Config.Bind(new ConfigDefinition("3 - Tar Rifle", "Proc Coefficient"), 0.7f, new ConfigDescription("Rifle proc coefficient per bullet", null, []));
            Templar.rifleForce = Config.Bind(new ConfigDefinition("3 - Tar Rifle", "Force"), 5f, new ConfigDescription("Rifle bullet force", null, []));
            Templar.rifleFireRate = Config.Bind(new ConfigDefinition("3 - Tar Rifle", "Fire Rate"), 1.6f, new ConfigDescription("Higher value = lower fire rate, due to spaghetti code", null, []));
            Templar.clayGrenadeStock = Config.Bind(new ConfigDefinition("4 - Clay Bomb", "Stock"), 2, new ConfigDescription("Maximum stock", null, []));
            Templar.clayGrenadeCooldown = Config.Bind(new ConfigDefinition("4 - Clay Bomb", "Cooldown"), 5f, new ConfigDescription("Bomb cooldown", null, []));
            Templar.clayGrenadeDamageCoefficient = Config.Bind(new ConfigDefinition("4 - Clay Bomb", "Damage"), 4f, new ConfigDescription("Explosion damage", null, []));
            Templar.clayGrenadeProcCoefficient = Config.Bind(new ConfigDefinition("4 - Clay Bomb", "Proc Coefficient"), 0.8f, new ConfigDescription("Explosion proc coefficient", null, []));
            Templar.clayGrenadeRadius = Config.Bind(new ConfigDefinition("4 - Clay Bomb", "Radius"), 12f, new ConfigDescription("Explosion radius", null, []));
            Templar.clayGrenadeDetonationTime = Config.Bind(new ConfigDefinition("4 - Clay Bomb", "Detonation Time"), 0.15f, new ConfigDescription("The time it takes to explode after hitting something", null, []));
            Templar.blunderbussStock = Config.Bind(new ConfigDefinition("5 - Blunderbuss", "Stock"), 2, new ConfigDescription("Maximum stock", null, []));
            Templar.blunderbussCooldown = Config.Bind(new ConfigDefinition("5 - Blunderbuss", "Cooldown"), 2.5f, new ConfigDescription("Cooldown per shot", null, []));
            Templar.blunderbussPelletCount = Config.Bind(new ConfigDefinition("5 - Blunderbuss", "Pellet count"), 6, new ConfigDescription("Pellets fired per shot", null, []));
            Templar.blunderbussDamageCoefficient = Config.Bind(new ConfigDefinition("5 - Blunderbuss", "Damage"), 1f, new ConfigDescription("Pellet damage", null, []));
            Templar.blunderbussProcCoefficient = Config.Bind(new ConfigDefinition("5 - Blunderbuss", "Proc Coefficient"), 1f, new ConfigDescription("Pellet proc coefficient", null, []));
            Templar.blunderbussSpread = Config.Bind(new ConfigDefinition("5 - Blunderbuss", "Pellet spread"), 10f, new ConfigDescription("Pellet spread when fired", null, []));
            Templar.tarStock = Config.Bind(new ConfigDefinition("6 - Tar Blast", "Stock"), 1, new ConfigDescription("Maximum stock", null, []));
            Templar.tarCooldown = Config.Bind(new ConfigDefinition("6 - Tar Blast", "Cooldown"), 1f, new ConfigDescription("Cooldown", null, []));
            Templar.overdriveStock = Config.Bind(new ConfigDefinition("6 - Tar Overdrive", "Stock"), 1, new ConfigDescription("Maximum stock", null, []));
            Templar.overdriveCooldown = Config.Bind(new ConfigDefinition("6 - Tar Overdrive", "Cooldown"), 12f, new ConfigDescription("Cooldown", null, []));
            Templar.dashStock = Config.Bind(new ConfigDefinition("7 - Sidestep", "Stock"), 2, new ConfigDescription("Maximum stock", null, []));
            Templar.dashCooldown = Config.Bind(new ConfigDefinition("7 - Sidestep", "Cooldown"), 7f, new ConfigDescription("Cooldown", null, []));
            Templar.bazookaStock = Config.Bind(new ConfigDefinition("8 - Bazooka", "Stock"), 1, new ConfigDescription("Maximum stock", null, []));
            Templar.bazookaCooldown = Config.Bind(new ConfigDefinition("8 - Bazooka", "Cooldown"), 14f, new ConfigDescription("Bazooka cooldown", null, []));
            Templar.bazookaDamageCoefficient = Config.Bind(new ConfigDefinition("8 - Bazooka", "Damage"), 12f, new ConfigDescription("Bazooka damage", null, []));
            Templar.bazookaProcCoefficient = Config.Bind(new ConfigDefinition("8 - Bazooka", "Proc Coefficient"), 0.6f, new ConfigDescription("Bazooka proc coefficient", null, []));
            Templar.bazookaBlastRadius = Config.Bind(new ConfigDefinition("8 - Bazooka", "Blast Radius"), 16f, new ConfigDescription("Bazooka blast radius", null, []));
            Templar.miniBazookaDamageCoefficient = Config.Bind(new ConfigDefinition("8 - Bazooka Mk.2", "Damage"), 5f, new ConfigDescription("Bazooka Mk.2 damage", null, []));
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

        internal static List<GameObject> projectilePrefabs = new();
        internal static List<SurvivorDef> survivorDefs = new();
        public static List<GameObject> bodyPrefabs = new();
        internal static List<GameObject> masterPrefabs = new();
        internal static List<Type> entityStates = new();
        internal static List<SkillDef> skillDefs = new();
        internal static List<SkillFamily> skillFamilies = new();


    }
}

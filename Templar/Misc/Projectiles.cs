using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Templar
{
	public static class Projectiles
	{
		internal static void ProjectileSetup()
		{
			Templar.templarGrenade = Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("TemplarGrenadeProjectile", true, "C:Lemurian.cs", "ProjectileSetup", 527);
			Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 4f;
			Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.8f;
			Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().blastRadius = 12f;
			Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().falloffModel = BlastAttack.FalloffModel.Linear;
			Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().lifetimeAfterImpact = 0.15f;
			Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().impactEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosivePotExplosion");
			Templar.templarGrenade.GetComponent<ProjectileDamage>().damageType = DamageType.ClayGoo;
			GameObject gameObject = Assets.clayBombModel.InstantiateClone("TemplarBombModel", true, "C:Lemurian.cs", "ProjectileSetup", 541);
			gameObject.AddComponent<ProjectileGhostController>();
			gameObject.transform.GetChild(0).localScale *= 0.5f;
			Templar.templarGrenade.GetComponent<ProjectileController>().ghostPrefab = gameObject;
			gameObject.AddComponent<NetworkIdentity>();
			gameObject.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Models/TemplarBombModel", 500);
			Templar.templarGrenade.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Projectiles/TemplarGrenadeProjectile", 48);
			Templar.templarRocket = Resources.Load<GameObject>("Prefabs/Projectiles/LemurianBigFireball").InstantiateClone("TemplarRocketProjectile", true, "C:Lemurian.cs", "ProjectileSetup", 565);
			Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 1f;
			Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().blastRadius = 16f;
			Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.8f;
			Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().destroyOnEnemy = true;
			Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().destroyOnWorld = true;
			bool flag = Templar.templarRocket.GetComponent<MissileController>() != null;
			bool flag2 = flag;
			if (flag2)
			{
				UnityEngine.Object.Destroy(Templar.templarRocket.GetComponent<MissileController>());
			}
			Templar.templarRocket.AddComponent<Templar.TemplarMissileController>();
			GameObject gameObject2 = Assets.clayMissileModel.InstantiateClone("TemplarMissileModel", true, "C:Lemurian.cs", "ProjectileSetup", 580);
			gameObject2.AddComponent<ProjectileGhostController>();
			gameObject2.transform.GetChild(1).SetParent(gameObject2.transform.GetChild(0));
			gameObject2.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 90f, 0f);
			gameObject2.transform.GetChild(0).localScale *= 0.5f;
			gameObject2.transform.GetChild(0).GetChild(0).GetChild(0).SetParent(gameObject2.transform.GetChild(0));
			GameObject gameObject3 = gameObject2.transform.GetChild(0).GetChild(0).gameObject;
			gameObject3.AddComponent<Templar.TemplarSeparateFromParent>();
			gameObject3.transform.localScale *= 0.8f;
			Templar.templarRocket.GetComponent<ProjectileController>().ghostPrefab = gameObject2;
			Templar.templarRocketEffect = Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().impactEffect.InstantiateClone("TemplarRocketImpact", true, "CLemurian.cs", "ProjectileSetup", 664);
			Templar.templarRocketEffect.AddComponent<Templar.TemplarExplosionForce>();
			bool flag3 = Templar.templarRocketEffect.GetComponent<VFXAttributes>();
			bool flag4 = flag3;
			if (flag4)
			{
				Templar.templarRocketEffect.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
			}
			EffectAPI.AddEffect(Templar.templarRocketEffect);
			Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().impactEffect = Templar.templarRocketEffect;
			Templar.templarRocket.GetComponent<ProjectileDamage>().damageType = DamageType.BypassOneShotProtection;
			gameObject2.AddComponent<NetworkIdentity>();
			gameObject2.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Models/TemplarMissileModel", 501);
			Templar.templarRocket.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Projectiles/TemplarRocketProjectile", 44);
			Templar.templarRocketEffect.AddComponent<NetworkIdentity>();
			Templar.templarRocketEffect.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/effects/TemplarRocketImpact", 45);
			Loader.projectilePrefabs.Add(Templar.templarGrenade);
			Loader.projectilePrefabs.Add(Templar.templarRocket);
		}
	}
}

//using Il2CppAssets.Scripts.Models.Effects;
//using Il2CppAssets.Scripts.Models.Towers;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
//using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
//using BTD_Mod_Helper.Api.Towers;
//using BTD_Mod_Helper.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Il2CppSystem.Collections.Generic;
//using Il2CppAssets.Scripts.Unity;
//using MelonLoader;
//using Il2CppAssets.Scripts.Models;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors;
//using Il2CppAssets.Scripts.Models.Towers.Projectiles;
//using Il2CppAssets.Scripts.Models.Bloons;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
//using BTD_Mod_Helper.Api.Display;
//using Il2CppAssets.Scripts.Unity.Display;
//using UnityEngine;
//using Il2CppAssets.Scripts.Models.Towers.Weapons;
//using Il2CppAssets.Scripts.Models.Towers.Filters;
//using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
//using Il2CppInterop.Runtime.InteropTypes.Arrays;
//using Il2Cpp;

//namespace FrostsTowers.WaterMonkey
//{
//    public class BubbleBlast : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-100";

//        public override int Path => TOP;
//        public override int Tier => 1;
//        public override int Cost => 500;

//        public override string Description => "Shoots a spread bubbles that burst on contact, damaging nearby bloons.";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            tower.range = 40;

//            AttackModel newAttack = new AttackModel("BlastAttack", new Il2CppReferenceArray<WeaponModel>(1),
//                40, new Il2CppReferenceArray<Model>(0), null, 0, 0, 0, false, false, 0, false, 0);


//            ProjectileModel explosion = Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().projectile.Duplicate();
//            //explosion.display = "";
//            explosion.GetBehavior<DamageModel>().immuneBloonProperties = BloonProperties.Lead;
//            explosion.RemoveBehavior<TravelStraitModel>();
//            explosion.pierce = 8;
//            explosion.AddBehavior(new AgeModel("Age", 0.1f, 0, false, null));

//            ProjectileModel proj = Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().projectile.Duplicate();
//            //proj.display = "c73fd08146403e14fbcebd3cbf600b88";
//            proj.ApplyDisplay<BubbleProjectileDisplay>();
//            proj.RemoveBehavior<DamageModel>();
//            proj.RemoveBehavior<TravelStraitModel>();

//            proj.pierce = 1;
//            proj.maxPierce = 1;
//            proj.CapPierce(1);
//            proj.AddBehavior(new AgeModel("age", 1f, 0, false, null));
//            proj.AddBehavior(new TravelStraightSlowdownModel("travel", 80, 1f, 0.9f, 1, false));
//            proj.AddBehavior(new DamageModel("Damage", 0, 0, false, false, false, BloonProperties.Lead, BloonProperties.Lead));
//            proj.AddBehavior(new CreateProjectileOnContactModel("Explode", explosion, new SingleEmissionModel("e", null), false, false, false));
//            proj.AddBehavior(new CreateEffectOnContactModel("ExplosionEffect", new EffectModel("effect", Game.instance.model.GetTower("IceMonkey", 0, 0, 0).GetDescendant<EjectEffectModel>().effectModel.assetId, 1, 2)));


//            newAttack.weapons[0] = new WeaponModel("weapon", 1, 2.5f, proj, 0, 0, 0, 0, false, false, new RandomArcEmissionModel("em", 3, 0, 50, 15, 0, null), null);

//            newAttack.AddBehavior(new RotateToTargetModel("rotate", true, false, false, 0, true, false));
//            newAttack.AddBehavior(new AttackFilterModel("filter", new Il2CppReferenceArray<FilterModel>(new FilterModel[]
//                {
//                    new FilterInvisibleModel("camo", true, false)
//                })));
//            tower.GetAttackModel().RemoveBehavior<TargetCloseModel>();
//            newAttack.AddBehavior(new TargetFirstModel("first", true, false));
//            newAttack.AddBehavior(new TargetLastModel("last", true, false));
//            newAttack.AddBehavior(new TargetCloseModel("close", true, false));
//            newAttack.AddBehavior(new TargetStrongModel("strong", true, false));

//            tower.AddBehavior(newAttack);
//        }
//    }

//    public class ScaldingWater : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-200";

//        public override int Path => TOP;
//        public override int Tier => 2;
//        public override int Cost => 850;

//        public override string Description => "Attacks deal more damage and are hot enough to pop lead bloons.";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            tower.GetAttackModels()[0].weapons[0].projectile.GetDamageModel().damage += 1;
//            tower.GetAttackModels()[0].weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
//        }
//    }

//    public class Torrent : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-100";

//        public override int Path => TOP;
//        public override int Tier => 3;
//        public override int Cost => 3200;

//        public override string Description => "Shoots a constant stream of water with high pierce.";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            ProjectileModel proj = Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().projectile.Duplicate();
//            //proj.display = "c73fd08146403e14fbcebd3cbf600b88";
//            proj.RemoveBehavior<DamageModel>();

//            proj.pierce = 20;
//            proj.GetBehavior<TravelStraitModel>().speed = 120;
//            proj.GetBehavior<TravelStraitModel>().lifespan = 0.6f;
//            proj.AddBehavior(new DamageModel("Damage", 1, 0, false, false, true, BloonProperties.None, BloonProperties.None));
//            proj.ApplyDisplay<TorrentProjectileDisplay>();

//            proj.collisionPasses = new Il2CppStructArray<int>(2);
//            proj.collisionPasses[0] = 0;
//            proj.collisionPasses[1] = -1;

//            //Crosspath
//            if (tower.tiers[1] >= 2)
//            {
//                KnockbackModel knockback3 = new KnockbackModel("torrentKnockback", 0, 0f, 0.8f, 0.5f, "TorrentKnockback", null);
//                proj.AddBehavior(knockback3);
//            }

//            tower.GetAttackModels()[1].AddWeapon(new WeaponModel("torrent", 1, 0.125f, proj, 0, 0, 0, 0, false, false, new SingleEmmisionTowardsTargetModel("em", null, 0), null));
//        }
//    }

//    public class HighPressureBlast : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-200";

//        public override int Path => TOP;
//        public override int Tier => 4;
//        public override int Cost => 5600;

//        public override string Description => "Waves have greatly increased damage, bubbles and torrent fire faster with more pierce, and all attacks do more damage to ceramics.";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            DamageModifierForTagModel waveCeramBonus = new DamageModifierForTagModel("ceramBonus", "Ceramic", 1, 3, false, false);
//            DamageModifierForTagModel bubbleCeramBonus = new DamageModifierForTagModel("ceramBonus", "Ceramic", 1, 2, false, false);
//            DamageModifierForTagModel torrentCeramBonus = new DamageModifierForTagModel("ceramBonus", "Ceramic", 1, 1, false, false);

//            tower.GetAttackModels()[0].weapons[0].projectile.GetDamageModel().damage += 3;
//            tower.GetAttackModels()[0].weapons[0].projectile.pierce = 40;
//            tower.GetAttackModels()[0].weapons[0].projectile.AddBehavior(waveCeramBonus);

//            tower.GetAttackModels()[1].weapons[0].rate = 1f;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 16;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(bubbleCeramBonus);

//            tower.GetAttackModels()[1].weapons[1].rate = 0.1f;
//            tower.GetAttackModels()[1].weapons[1].projectile.pierce = 32;
//            tower.GetAttackModels()[1].weapons[1].projectile.radius *= 1.5f;
//            tower.GetAttackModels()[1].weapons[1].projectile.scale = 1.5f;
//            tower.GetAttackModels()[1].weapons[1].projectile.AddBehavior(torrentCeramBonus);
//        }
//    }

//    public class Hydroblitz : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-100";

//        public override int Path => TOP;
//        public override int Tier => 5;
//        public override int Cost => 30000;

//        public override string Description => "Washes away bloons with incredible high speed torrents";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            tower.range += 10;

//            tower.GetAttackModels()[0].range += 15;
//            tower.GetAttackModels()[0].weapons[0].projectile.radius += 15;
//            tower.GetAttackModels()[0].weapons[0].projectile.GetDamageModel().damage += 10;
//            tower.GetAttackModels()[0].weapons[0].projectile.pierce = 100;
//            tower.GetAttackModels()[0].weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 10;

//            tower.GetAttackModels()[1].range += 10;
//            tower.GetAttackModels()[1].weapons[0].emission = new RandomArcEmissionModel("em", 8, 0, 70, 20, 0, null);
//            tower.GetAttackModels()[1].weapons[0].rate = 0.6f;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 32;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius *= 1.5f;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale = 1.5f;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 4;
//            tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 8;

//            tower.GetAttackModels()[1].weapons[1].projectile.GetDamageModel().damage = 3;
//            tower.GetAttackModels()[1].weapons[1].projectile.pierce = 80;
//            tower.GetAttackModels()[1].weapons[1].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 5;
//        }
//    }

//    public class BubbleProjectileDisplay : ModDisplay
//    {
//        public override string BaseDisplay => "c73fd08146403e14fbcebd3cbf600b88";

//        public override void ModifyDisplayNode(UnityDisplayNode node)
//        {
//            Set2DTexture(node, "BubbleProjectileTexture");
//        }
//    }

//    public class TorrentProjectileDisplay : ModDisplay
//    {
//        public override string BaseDisplay => "c73fd08146403e14fbcebd3cbf600b88";

//        public override void ModifyDisplayNode(UnityDisplayNode node)
//        {
//            Set2DTexture(node, "TorrentProjectileTexture");
//        }
//    }
//}

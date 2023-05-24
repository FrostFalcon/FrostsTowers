//using Il2CppAssets.Scripts.Models;
//using Il2CppAssets.Scripts.Models.Effects;
//using Il2CppAssets.Scripts.Models.GenericBehaviors;
//using Il2CppAssets.Scripts.Models.Map;
//using Il2CppAssets.Scripts.Models.Towers;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
//using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
//using Il2CppAssets.Scripts.Models.Towers.Filters;
//using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
//using Il2CppAssets.Scripts.Models.Towers.Weapons;
//using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
//using Il2CppAssets.Scripts.Models.TowerSets;
//using Il2CppAssets.Scripts.Simulation.Towers.Weapons.Behaviors;
//using Il2CppAssets.Scripts.Unity;
//using Il2CppAssets.Scripts.Unity.Display;
//using BTD_Mod_Helper.Api;
//using BTD_Mod_Helper.Api.Display;
//using BTD_Mod_Helper.Api.Towers;
//using BTD_Mod_Helper.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using Il2Cpp;
//using Il2CppInterop.Runtime.InteropTypes.Arrays;
//using MelonLoader;

//namespace FrostsTowers.WaterMonkey
//{
//    public class WaterMonkey : ModTower
//    {
//        public override TowerSet TowerSet => TowerSet.Magic;

//        public override string BaseTower => TowerType.IceMonkey;

//        public override int Cost => 450;

//        public override int TopPathUpgrades => 5;

//        public override int MiddlePathUpgrades => 2;

//        public override int BottomPathUpgrades => 0;

//        public override string Description => "Attacks bloons with strong waves";

//        public override void ModifyBaseTowerModel(TowerModel towerModel)
//        {
//            towerModel.areaTypes = Game.instance.model.GetTowerFromId("MonkeySub").areaTypes;

//            WeaponModel weapon = towerModel.GetAttackModels()[0].weapons[0];

//            towerModel.range = 25;
//            towerModel.GetAttackModel().range = 25;
//            weapon.projectile.radius = towerModel.range;
//            weapon.projectile.pierce = 20;
//            weapon.rate = 2;
//            weapon.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;
//            weapon.projectile.RemoveBehavior<FreezeModel>();
//            weapon.projectile.RemoveBehavior<ProjectileFilterModel>();
//            weapon.RemoveBehavior<EjectEffectModel>();
//            towerModel.RemoveBehavior<LinkProjectileRadiusToTowerRangeModel>();
//            towerModel.GetAttackModel().RemoveBehavior<AttackFilterModel>();

//            towerModel.GetAttackModel().AddBehavior(new AttackFilterModel("Afilter", new Il2CppReferenceArray<FilterModel>(new FilterModel[]
//                {
//                    new FilterInvisibleModel("NoCamo", true, false)
//                })));

//            weapon.projectile.AddBehavior(new ProjectileFilterModel("Pfilter", new Il2CppReferenceArray<FilterModel>(new FilterModel[]
//                {
//                    new FilterInvisibleModel("NoCamo", true, false)
//                })));

//            weapon.AddBehavior(new EjectEffectModel("SplashEffect", ModContent.CreatePrefabReference<SplashEffectDisplay>(),
//                new EffectModel("effect", ModContent.CreatePrefabReference<SplashEffectDisplay>(), 1, 1), 0, false, false, false, false, false));
//        }

//        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
//        {
//            return towerSet.First(model => model.towerId == TowerType.NinjaMonkey).towerIndex + 1;
//        }
//    }

//    public class WaterMonkeyBaseDisplay : ModTowerDisplay<WaterMonkey>
//    {
//        public override string BaseDisplay => GetDisplay(TowerType.IceMonkey);

//        public override bool UseForTower(int[] tiers)
//        {
//            return true;
//        }

//        public override void ModifyDisplayNode(UnityDisplayNode node)
//        {
//            SetMeshTexture(node, "WaterMonkeyBaseDisplay");
//        }
//    }

//    public class SplashEffectDisplay : ModDisplay
//    {
//        public override string BaseDisplay => "f73f2e12a1827cd40b99cf65312a3a2f";

//        public override void ModifyDisplayNode(UnityDisplayNode node)
//        {
//            //Set2DTexture(node, "SplashEffectTexture");
//        }
//    }
//}

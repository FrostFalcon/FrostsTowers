using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace FrostsTowers.SparkTower
{
    public class SparkTower : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Primary;

        public override string BaseTower => TowerType.DartMonkey;

        public override int Cost => 250;

        public override int TopPathUpgrades => 5;

        public override int MiddlePathUpgrades => 5;

        public override int BottomPathUpgrades => 5;

        public override ParagonMode ParagonMode => ParagonMode.Base000;

        public override string Description => "Shoots bolts of lightning at bloons";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            WeaponModel weapon = towerModel.GetAttackModels()[0].weapons[0];

            weapon.projectile.ApplyDisplay<SparkProjectileDisplay>();
            weapon.ejectX = 0;
            weapon.ejectY = 0;
            weapon.projectile.pierce = 1;
            weapon.projectile.GetDamageModel().damage = 2;
            ProjectileModelBehaviorExt.GetBehavior<TravelStraitModel>(weapon.projectile).speed = 600;
            weapon.rate = 1.5f;
            weapon.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            towerModel.range = 30;
            towerModel.GetAttackModel().range = towerModel.range;
            towerModel.GetWeapon().projectile.collisionPasses = new int[] { 0, -1 };

            RotateToTargetModel aim = new RotateToTargetModel("rotate", true, false, false, 0, false, false);
            towerModel.GetAttackModel().RemoveBehavior<RotateToTargetModel>();
            towerModel.GetAttackModel().AddBehavior(aim);
        }

        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.TackShooter).towerIndex + 1;
        }
    }

    public class SparkParagonUpgrade : ModParagonUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-100";

        public override int Cost => 400000;

        public override string DisplayName => "Superstorm Catalyst";
        public override string Description => "A perfect storm of bloon electrocuting power.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            //Tower
            tower.range = 60;
            tower.GetAttackModel().range = 60;
            tower.GetWeapon().rate = 0.25f;

            WeaponModel weapon2 = tower.GetWeapon().Duplicate();

            //Base
            ProjectileModel oldProj = tower.GetWeapon().projectile.Duplicate();
            ProjectileModel newProj = Game.instance.model.GetTower("Druid", 2, 0, 0).GetWeapons()[1].projectile.Duplicate();

            newProj.RemoveBehavior<DamageModel>();
            newProj.GetBehavior<LightningModel>().splits = 3;
            newProj.GetBehavior<LightningModel>().splitRange = 60;
            newProj.pierce = 20;
            newProj.RemoveBehavior<RemoveBloonModifiersModel>();
            newProj.AddBehavior(oldProj.GetDamageModel().Duplicate());
            newProj.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            newProj.GetDamageModel().damage = 200;

            //Boss Damage
            DamageModifierForTagModel bossDamage = new DamageModifierForTagModel("Boss", "Bosses", 1, 50, false, false);
            newProj.AddBehavior(bossDamage);

            //Stun
            SlowModel stun = new SlowModel("BigSparkStun", 0, 1f, "SparkTowerBigStun", 99999, "LaserShock", false, false, new EffectModel("effectModel", new Il2CppAssets.Scripts.Utils.PrefabReference(), 1, -1), false, false, false);
            newProj.AddBehavior(stun);

            tower.GetWeapon().projectile = newProj;

            weapon2.emission = new RandomEmissionModel("emm", 5, 360, 6, null, false, 0, 0, 0, false);
            weapon2.rate = 0.1f;
            weapon2.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            weapon2.projectile.GetDamageModel().damage = 80;
            weapon2.projectile.GetBehavior<TravelStraitModel>().speed = 1000;

            //Chain lightning
            RetargetOnContactModel bounce = new RetargetOnContactModel("Retarget", 100, 999, "Close", 0, true);
            weapon2.projectile.AddBehavior(bounce);
            weapon2.projectile.pierce = 12;

            //Static Burst
            ProjectileModel proj = Game.instance.model.GetTower("BombShooter", 3, 0, 0).GetDescendant<ProjectileModel>().GetBehavior<CreateProjectileOnContactModel>().projectile.Clone().Cast<ProjectileModel>();
            proj.ApplyDisplay<StaticBurstDisplay>();
            proj.GetDamageModel().damage = 40;
            CreateProjectileOnExhaustPierceModel exp = new CreateProjectileOnExhaustPierceModel("Exp", proj, new SingleEmissionModel("em", null), 1, 99999, 1, false, new Il2CppAssets.Scripts.Utils.PrefabReference(), 0, false);
            weapon2.projectile.AddBehavior(exp);
            weapon2.projectile.GetBehavior<RetargetOnContactModel>().delay = 0.02f;

            tower.GetAttackModel().AddBehavior(weapon2);

            tower.GetDescendants<FilterInvisibleModel>().ForEach(f => f.isActive = false);
        }
    }

    public class SparkProjectileDisplay : ModDisplay
    {
        public override string BaseDisplay => "548c26e4e668dac4a850a4c016916016";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }

    public class SparkTowerBaseDisplay : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override bool UseForTower(int[] tiers)
        {
            return tiers[0] < 3 && tiers[1] < 3 && tiers[2] < 3;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }

    public class SparkTower3xxDisplay : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override bool UseForTower(int[] tiers)
        {
            return tiers[0] >= 3 && tiers[0] < 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }

    public class SparkTowerx3xDisplay : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override bool UseForTower(int[] tiers)
        {
            return tiers[1] >= 3 && tiers[1] < 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }

    public class SparkTowerxx3Display : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override bool UseForTower(int[] tiers)
        {
            return tiers[2] >= 3 && tiers[2] < 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }

    public class SparkTower5xxDisplay : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override float Scale => 1.2f;

        public override bool UseForTower(int[] tiers)
        {
            return tiers[0] >= 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "SparkTower3xxDisplay");
        }
    }

    public class SparkTowerx5xDisplay : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override float Scale => 1.2f;

        public override bool UseForTower(int[] tiers)
        {
            return tiers[1] >= 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "SparkTowerx3xDisplay");
        }
    }

    public class SparkTowerxx5Display : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);

        public override float Scale => 1.2f;

        public override bool UseForTower(int[] tiers)
        {
            return tiers[2] >= 5;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "SparkTowerxx3Display");
        }
    }

    public class SparkTowerParagonDisplay : ModTowerDisplay<SparkTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.TackShooter);
        public override float Scale => 1.5f + ParagonDisplayIndex * 0.25f;
        public override string Name => nameof(SparkTowerParagonDisplay) + ParagonDisplayIndex;

        public override bool UseForTower(int[] tiers)
        {
            return IsParagon(tiers);
        }

        public SparkTowerParagonDisplay() { }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }
}

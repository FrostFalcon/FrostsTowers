using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppSystem.Collections.Generic;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2Cpp;

namespace FrostsTowers.SparkTower
{
    public class ChainLightning : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-001";

        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 400;

        public override string Description => "Lightning transfers to up to 3 targets.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            RetargetOnContactModel bounce = new RetargetOnContactModel("Retarget", 40, 999, "Close", 0, true);

            tower.GetAttackModel().weapons[0].projectile.AddBehavior(bounce);
            tower.GetAttackModel().weapons[0].projectile.pierce = 3;
        }
    }

    public class ImprovedChain : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-001";

        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 550;

        public override string Description => "Lightning hits up to 5 bloons and can travel farther between them.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetAttackModel().weapons[0].projectile.GetBehavior<RetargetOnContactModel>().distance = 60;
            tower.GetAttackModel().weapons[0].projectile.pierce = 5;
        }
    }

    public class StaticBurst : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-003";

        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 1600;

        public override string Description => "Lightning bolts will release bursts of energy on contact.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            ProjectileModel proj = Game.instance.model.GetTower("BombShooter", 3, 0, 0).GetDescendant<ProjectileModel>().GetBehavior<CreateProjectileOnContactModel>().projectile.Clone().Cast<ProjectileModel>();
            proj.ApplyDisplay<StaticBurstDisplay>();
            proj.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            proj.GetDamageModel().damage = 1;

            CreateProjectileOnExhaustPierceModel exp = new CreateProjectileOnExhaustPierceModel("Exp", proj, new SingleEmissionModel("em", null), 4, 3, 1, false, new Il2CppAssets.Scripts.Utils.PrefabReference(), 0, false);
            tower.GetAttackModel().weapons[0].projectile.AddBehavior(exp);

            tower.GetAttackModel().weapons[0].projectile.GetBehavior<RetargetOnContactModel>().delay = 0.02f;
        }
    }

    public class Discharge : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-003";

        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 5600;

        public override string Description => "Periodically releases a burst of energy, electrocuting all nearby bloons.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            ProjectileModel proj = tower.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.Duplicate();
            proj.display = new Il2CppAssets.Scripts.Utils.PrefabReference();
            proj.scale = 3;
            proj.GetDamageModel().damage = 2;
            proj.pierce = 6;
            proj.maxPierce = 6;
            proj.radius *= 3;

            ProjectileModel proj2 = tower.GetAttackModel().weapons[0].projectile.Duplicate();
            proj2.pierce = 2;
            proj.AddBehavior(new CreateProjectileOnContactModel("moreLightning", proj2, new SingleEmissionModel("e", null), false, false, false));

            tower.GetAttackModel().AddWeapon(new WeaponModel("Discharge", 1, tower.tiers[0] >= 1 ? 2 : 2.5f, proj, 0, 0, 0, 0, false, false, new SingleEmissionAtTowerModel("emission", null)));
            tower.GetAttackModel().weapons[1].AddBehavior(new EjectEffectModel("effect", new Il2CppAssets.Scripts.Utils.PrefabReference(), new EffectModel("model", new Il2CppAssets.Scripts.Utils.PrefabReference(), 3, 0.5f), 0.5f, false, false, false, false, false));
            tower.GetAttackModel().weapons[1].projectile.ApplyDisplay<StaticBurstDisplay>();
        }
    }

    public class Electrosphere : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-100";

        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 30000;

        public override string Description => "High powered electricity melts bloon clusters and defortifies moabs.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            //Increase stats
            tower.GetAttackModel().weapons[0].rate /= 2;
            tower.GetAttackModel().weapons[0].projectile.pierce += 4;
            tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            tower.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.pierce += 8;
            tower.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().count += 3;
            tower.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.GetDamageModel().damage += 1;
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce += 2;
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.pierce += 8;
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.GetDamageModel().damage += 2;
            tower.GetAttackModel().weapons[1].rate /= 2;

            DamageModifierForTagModel ceramDamage = new DamageModifierForTagModel("ceram", "Ceramic", 1, 8, false, false);

            tower.GetAttackModel().weapons[0].projectile.AddBehavior(ceramDamage);
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(ceramDamage);
            tower.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.AddBehavior(ceramDamage);

            RemoveBloonModifiersModel deFort = new RemoveBloonModifiersModel("deFortify", false, false, false, true, true, new Il2CppSystem.Collections.Generic.List<string>());
            deFort.bloonTagExcludeList.Add("Bfb");
            deFort.bloonTagExcludeList.Add("Zomg");
            deFort.bloonTagExcludeList.Add("Bad");
            deFort.bloonTagExcludeList.Add("Ddt");

            tower.GetAttackModel().weapons[0].projectile.AddBehavior(deFort);
        }
    }

    public class StaticBurstDisplay : ModDisplay
    {
        public override string BaseDisplay => "88399aeca4ae48a44aee5b08eb16cc61";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            node.SaveMeshTexture(0, "D:\\Downloads\\StaticBurst");
        }
    }
}

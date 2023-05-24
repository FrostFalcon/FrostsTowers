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
using MelonLoader;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace FrostsTowers.SparkTower
{
    public class FasterRecharge : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-100";

        public override int Path => TOP;
        public override int Tier => 1;
        public override int Cost => 300;

        public override string Description => "Increased attack speed.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().rate *= 2;
            tower.GetWeapon().rate /= 3;
        }
    }

    public class EngineBuster : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-200";

        public override int Path => TOP;
        public override int Tier => 2;
        public override int Cost => 500;

        public override string Description => "Attacks do increased damage to MOAB class bloons.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            DamageModifierForTagModel moab = new DamageModifierForTagModel("Moab", "Moabs", 1, 2, false, false);
            tower.GetWeapon().projectile.AddBehavior(moab);
        }
    }

    public class StaticShock : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-300";

        public override int Path => TOP;
        public override int Tier => 3;
        public override int Cost => 920;

        public override string Description => "Deals more damage and stuns bloons.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.GetDamageModel().damage++;

            SlowModel stun = new SlowModel("SparkStun", 0, 0.4f, "SparkTowerStun", 99999, "", true, false, new EffectModel("effectModel", new Il2CppAssets.Scripts.Utils.PrefabReference(), 1, -1), false, false, false, 0);
            stun.overlayType = "LaserShock";
            tower.GetWeapon().projectile.AddBehavior(stun);
            tower.GetWeapon().projectile.AddBehavior(new SlowModifierForTagModel("notMoabs", "Moabs", "SparkTowerStun", 1, false, true, 0, false));
            tower.GetWeapon().projectile.AddBehavior(new SlowModifierForTagModel("notPurples", "Purple", "SparkTowerStun", 1, false, true, 0, false));

            if (tower.GetWeapon().projectile.HasBehavior<RetargetOnContactModel>())
            {
                tower.GetWeapon().projectile.GetBehavior<RetargetOnContactModel>().expireIfNoTargetFound = false;
            }
        }
    }

    public class BoltStream : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-100";

        public override int Path => TOP;
        public override int Tier => 4;
        public override int Cost => 6200;

        public override string Description => "Basic attack is much stronger, attacking faster the longer it does damage. Static shock now affects moabs and ddts.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.range += 15;
            tower.GetAttackModel().range += 15;
            tower.GetWeapon().rate *= 0.75f;

            ProjectileModel oldProj = tower.GetWeapon().projectile.Duplicate();
            ProjectileModel newProj = Game.instance.model.GetTower("Druid", 2, 0, 0).GetWeapons()[1].projectile.Duplicate();

            newProj.RemoveBehavior<DamageModel>();
            newProj.GetBehavior<LightningModel>().splits = 1;
            newProj.GetBehavior<LightningModel>().splitRange = 40;
            newProj.RemoveBehavior<RemoveBloonModifiersModel>();
            newProj.AddBehavior(oldProj.GetDamageModel().Duplicate());
            newProj.AddBehavior(oldProj.GetBehavior<SlowModel>().Duplicate());
            newProj.AddBehavior(oldProj.GetBehavior<DamageModifierForTagModel>().Duplicate());

            newProj.pierce = 3;
            newProj.maxPierce = 3;
            newProj.GetDamageModel().damage += 2;
            newProj.GetBehavior<DamageModifierForTagModel>().damageAddative += 3;

            newProj.AddBehavior(new SlowModifierForTagModel("lessDdts", "Ddt", "SparkTowerStun", 1, false, false, 0.2f, false));
            newProj.AddBehavior(new SlowModifierForTagModel("notbfbs", "Bfb", "SparkTowerStun", 1, false, true, 0, false));
            newProj.AddBehavior(new SlowModifierForTagModel("notzomgs", "Zomg", "SparkTowerStun", 1, false, true, 0, false));
            newProj.AddBehavior(new SlowModifierForTagModel("notPurples", "Purple", "SparkTowerStun", 1, false, true, 0, false));

            //Crosspath
            if (tower.tiers[2] >= 1)
            {
                newProj.pierce += oldProj.pierce;
                newProj.maxPierce += oldProj.pierce;
            }

            tower.GetWeapon().projectile = newProj;

            tower.AddBehavior(new DamageBasedAttackSpeedModel("AttackSpeedRamp", 10, 50, 0.05f, 60));
        }
    }

    public class ThunderStrike : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-100";

        public override int Path => TOP;
        public override int Tier => 5;
        public override int Cost => 48000;

        public override string Description => "Adds a secondary attack that deals massive damage to the strongest bloon and stuns up to zomgs.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.pierce += 10;
            tower.GetWeapon().projectile.maxPierce += 10;
            tower.GetWeapon().projectile.GetDamageModel().damage += 8;
            tower.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 12;
            tower.GetWeapon().projectile.GetBehavior<LightningModel>().splitRange += 20;

            AttackModel attack = tower.GetAttackModel().Duplicate();
            attack.weapons[0].rate = 1.25f;
            attack.weapons[0].projectile.pierce = 1;
            attack.weapons[0].projectile.maxPierce = 1;
            attack.weapons[0].projectile.GetDamageModel().damage = 60;
            attack.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 240;
            attack.weapons[0].projectile.GetBehavior<SlowModel>().lifespan = 1;
            tower.GetBehavior<DamageBasedAttackSpeedModel>().damageThreshold = 500;
            tower.GetBehavior<DamageBasedAttackSpeedModel>().maxTimeInFramesWithoutDamage = 90;
            tower.GetBehavior<DamageBasedAttackSpeedModel>().increasePerThreshold = 0.1f;
            tower.GetBehavior<DamageBasedAttackSpeedModel>().maxStacks = 40;

            attack.targetProvider = new TargetStrongModel("strong", false, false);

            attack.weapons[0].projectile.RemoveBehavior<SlowModel>();
            attack.weapons[0].projectile.AddBehavior(new SlowModel("BigSparkStun", 0, 1f, "SparkTowerBigStun", 99999, "LaserShock", false, false, new EffectModel("effectModel", new Il2CppAssets.Scripts.Utils.PrefabReference(), 1, -1), false, false, false));
            attack.weapons[0].projectile.RemoveBehaviors<SlowModifierForTagModel>();
            attack.weapons[0].projectile.AddBehavior(new SlowModifierForTagModel("notPurples", "Purple", "SparkTowerBigStun", 1, false, true, 0, false));

            tower.AddBehavior(attack);
        }
    }
}

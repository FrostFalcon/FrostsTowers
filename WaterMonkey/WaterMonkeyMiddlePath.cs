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

//namespace FrostsTowers.WaterMonkey
//{
//    public class BiggerWaves : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-100";

//        public override int Path => MIDDLE;
//        public override int Tier => 1;
//        public override int Cost => 240;

//        public override string Description => "Waves have more range.";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            tower.range += 10;

//            tower.GetAttackModels()[0].weapons[0].projectile.radius += 8;

//            foreach (AttackModel a in tower.GetAttackModels())
//            {
//                a.range += 10;
//            }
//        }
//    }

//    public class TidalForce : ModUpgrade<WaterMonkey>
//    {
//        public override string Icon => "WaterMonkeyIcon-100";

//        public override int Path => MIDDLE;
//        public override int Tier => 2;
//        public override int Cost => 1000;

//        public override string Description => "All attacks knock back bloons on hit.";

//        public override void ApplyUpgrade(TowerModel tower)
//        {
//            KnockbackModel knockback = new KnockbackModel("knockback", 0, 0.8f, 1.6f, 0.4f, "WaveKnockback", null);
//            tower.GetAttackModels()[0].weapons[0].projectile.AddBehavior(knockback);

//            if (tower.tiers[0] >= 1)
//            {
//                KnockbackModel knockback2 = new KnockbackModel("bubbleKnockback", 0, 0.5f, 1.2f, 0.4f, "BubbleKnockback", null);
//                tower.GetAttackModels()[1].weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(knockback2);
//            }
//        }
//    }
//}

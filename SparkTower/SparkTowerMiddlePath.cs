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
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Utils;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Audio;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace FrostsTowers.SparkTower
{
    public class LongRangeConductors : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-010";

        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 100;

        public override string Description => "Increased attack range.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.range += 10;
            tower.GetAttackModel().range += 10;
        }
    }

    public class CamoSensors : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-020";

        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 320;

        public override string Description => "Allows detection of camo bloons.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.AddBehavior(new OverrideCamoDetectionModel("Camo", true));
        }
    }

    public class PowerNetwork : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-010";

        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 600;

        public override string Description => "Increases the range and pierce of nearby spark towers with this upgrade.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            TowerFilterModel filter1 = new FilterInBaseTowerIdModel("BaseTowerFilter", new Il2CppStringArray(new string[] { tower.baseId }));
            TowerFilterModel filter2 = new FilterInTowerUpgradeModel("UpgradeFilter", 0, 3, 0, true);
            TowerFilterModel t5Filter = new FilterInTowerUpgradeModel("T5Filter", 0, 5, 0, true);

            tower.AddBehavior(new RangeSupportModel("RangeBuff", false, 0.05f, 0, "PowerNetworkRangeBuff", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    filter1,
                    filter2
                }), false, "", ""));
            tower.AddBehavior(new PierceSupportModel("PierceBuff", false, 1, "PowerNetworkRangeBuff", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    filter1,
                    filter2
                }), false, "", ""));

            tower.AddBehavior(new DamageSupportModel("T5DamageBuff", false, 5, "ConvergenceCannonDamageBuff", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    filter1,
                    t5Filter
                }), false, false, 0));
        }
    }

    public class EmergencyPowerReserves : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-010";

        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 6000;

        public override string Description => "Activated Ability: Double the attack speed of all nearby spark towers for a short time.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            TowerFilterModel filter1 = new FilterInBaseTowerIdModel("BaseTowerFilter", new Il2CppStringArray(new string[] { tower.baseId }));

            ActivateRateSupportZoneModel az = Game.instance.model.towers.First(t => t.name == "IceMonkey-050").GetAbility().GetBehavior<ActivateRateSupportZoneModel>().Clone().Cast<ActivateRateSupportZoneModel>();
            az.name = "FireRateBuff";
            az.mutatorId = "EPRFireRateBuff";
            az.rateModifier = 0.5f;
            az.useTowerRange = true;
            az.isGlobal = false;
            az.canEffectThisTower = true;
            az.lifespan = 10;
            az.lifespanFrames = 599;
            az.displayModel = new DisplayModel("display", ModContent.CreatePrefabReference<EPRBuffDisplay>(), 0, Vector3.zero, 1, false, -1);
            az.filters = new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[] { filter1 });
            az.buffIconName = "";
            az.buffLocsName = "";

            SoundModel sound = new SoundModel("sound", Game.instance.model.GetTower("EngineerMonkey", 0, 4, 0).GetAbility().GetBehavior<CreateSoundOnAbilityModel>().sound.assetId);

            CreateSoundOnAbilityModel playSound = new CreateSoundOnAbilityModel("PlaySound", sound, null, null);

            AbilityModel ability = new AbilityModel("EPRAbility", "Emergency Power Reserves", "Double the attack speed of all nearby spark towers for a short time.", 1, -1,
                ModContent.GetSpriteReference(this.mod, "SparkTowerIcon-100"), 60, new Il2CppReferenceArray<Model>(new Model[] { az, playSound }),
                false, false, "", 0, 0, -1, false, false);

            tower.AddBehavior(ability);
        }
    }

    public class ConvergenceCannon : ModUpgrade<SparkTower>
    {
        public override string Icon => "SparkTowerIcon-100";

        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 24000;

        public override string Description => "Attacks channel power from the network to deal more damage for each connected tower.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetWeapon().projectile.GetDamageModel().damage = 10;
            tower.GetWeapon().projectile.pierce += 4;
            tower.GetWeapon().projectile.scale *= 3;
            tower.GetWeapon().rate /= 1.25f;
        }
    }

    class EPRBuffDisplay : ModDisplay
    {
        public override string BaseDisplay => "2b89f78293651a24b998f0d8d4a85460";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            throw new NotImplementedException();
        }
    }
}

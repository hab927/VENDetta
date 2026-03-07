using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

namespace Upgrades
{
    public class Upgrade {
        public string name;
        public string description;
        public float price;
        public Action upgradeEffect;

        public Upgrade(string name, string description, float price, Action effect) {
            this.name = name;
            this.description = description;
            this.price = price;
            this.upgradeEffect = effect;
        }
    }

    public static class UpgradeEffects {
        private static Run CurrentRun => Instance.run;
        private static UpgradePool Available => Instance.availableUpgrades;
        public static void BoostChips() {
            CurrentRun.products["ChipBag"].sat *= 1.5f; // 50% increase on satiation
        }
        public static void BoostWater() {
            CurrentRun.products["WaterBottle"].hyd *= 1.5f; // likewise for hydration on drinks
        }
        public static void LowerRequirement() {
            // multiplicatve reduction by 25%
            CurrentRun.satGoalMult *= 0.75f;
            CurrentRun.hydGoalMult *= 0.75f;
        }
        public static void ModifiersPlus() {
            CurrentRun.goodModChance = 0.70f;
        }

        // adding items to vending machine
        public static void AddCookiePack() {
            CurrentRun.snacks.Add("CookiePack");
            Available.data.Add(LockedUpgrades.data["CookieBoost"]);
        }
        public static void AddGranolaBar() {
            CurrentRun.snacks.Add("GranolaBar");
            Available.data.Add(LockedUpgrades.data["BlufBoost"]);
        }
        public static void AddCrocorade() {
            CurrentRun.drinks.Add("Crocorade");
            Available.data.Add(LockedUpgrades.data["CrocoradeBoost"]);
        }
        public static void AddSodaBottle() {
            CurrentRun.drinks.Add("SodaBottle");
            Available.data.Add(LockedUpgrades.data["ColaBoost"]);
        }


        public static void WorkEthic() {
            CurrentRun.income += 10.0f;
            CurrentRun.bonuses = true;
        }
        public static void IncreaseStock() {
            CurrentRun.stockMin = 2;
            CurrentRun.stockMax = 2;
        }
        public static void FresherStock() {
            CurrentRun.freshChance = 0.4f;
        }
        public static void ForeverFresh() {
            CurrentRun.expiredChance = 0.1f;
        }

        // locked stuff
        public static void BoostCrocorade() {
            CurrentRun.products["Crocorade"].sat *= 1.5f;
        }
        public static void BoostSodaBottle() {
            CurrentRun.products["SodaBottle"].sat *= 1.5f;
        }
        public static void BoostCookiePack() {
            CurrentRun.products["CookiePack"].sat *= 1.5f;
        }
        public static void BoostGranolaBar() {
            CurrentRun.products["GranolaBar"].sat *= 1.5f; 
        }
    }

    public class UpgradePool {

        public List<Upgrade> data;

        public UpgradePool() {
            data = new() {
                {
                    new Upgrade(
                        "Chip Booster",
                        "Grants 1.5x satiation for chips.",
                        6.00f,
                        UpgradeEffects.BoostChips
                    )
                },
                {
                    new Upgrade(
                        "Water Booster",
                        "Grants 1.5x hydration for water.",
                        6.00f,
                        UpgradeEffects.BoostWater
                    )
                },
                {
                    new Upgrade(
                        "Lower Thresholds",
                        "Lower the requirement for satiation and hydration by 10%.",
                        12.00f,
                        UpgradeEffects.LowerRequirement
                    )
                },
                {
                    new Upgrade(
                        "Add Cookie Pack",
                        "Adds the cookie pack ($1.75, 7.5 SAT) to the vending machine.",
                        8.0f,
                        UpgradeEffects.AddCookiePack
                    )
                },
                {
                    new Upgrade(
                        "Add Crocorade",
                        "Adds Crocorade ($2.00, 10.0 HYD) to the vending machine.",
                        10.00f,
                        UpgradeEffects.AddCrocorade
                    )
                },
                {
                    new Upgrade(
                        "Better Work Ethic",
                        "+$10 to payout, with a 25% chance to get another $10 as a bonus.",
                        9.00f,
                        UpgradeEffects.WorkEthic
                    )
                },
                {
                    new Upgrade(
                        "More Stock",
                        "Stocked items appear 2-5 times instead of 1-3.",
                        10.00f,
                        UpgradeEffects.IncreaseStock
                    )
                },
                {
                    new Upgrade(
                        "Fresher Stock",
                        "Double chance of fresh items.",
                        10.00f,
                        UpgradeEffects.FresherStock
                    )
                },
                {
                    new Upgrade(
                        "Expiration Examination",
                        "Half the chance of expired items.",
                        12.50f,
                        UpgradeEffects.ForeverFresh
                    )
                },
                {
                    new Upgrade(
                        "Modifiers+",
                        "Increase chance of good modifiers from 50% to 70%.",
                        14.00f,
                        UpgradeEffects.ModifiersPlus
                    )
                },
                {
                    new Upgrade(
                        "Add Joke-a Cola",
                        "Adds the Joke-a Cola ($2.50, 15.0 HYD) to the vending machine.",
                        15.00f,
                        UpgradeEffects.AddSodaBottle
                    )
                },
                {
                    new Upgrade(
                        "Add Bluf Bar",
                        "Adds the Bluf Bar ($2.50, 15.0 SAT) to the vending machine.",
                        15.00f,
                        UpgradeEffects.AddGranolaBar
                    )
                }
            };
        }
    }

    public static class LockedUpgrades {
        public static bool hasCrocorade = false;
        public static bool hasSodaBottle = false;
        public static bool hasCookiePack = false;
        public static bool hasFresher = false;

        public static Dictionary<string, Upgrade> data = new() {
            {
                "CrocoradeBoost",
                new Upgrade(
                    "Crocorade Booster",
                    "Grants 1.5x hydration for Crocorade.",
                    9.50f,
                    UpgradeEffects.BoostCrocorade
                )
            },
            {
                "ColaBoost",
                new Upgrade(
                    "Joke-a Cola Booster",
                    "Grants 1.5x hydration for the Joke-a Cola.",
                    13.50f,
                    UpgradeEffects.BoostSodaBottle
                )
            },
            {
                "CookieBoost",
                new Upgrade(
                    "Cookie Pack Booster",
                    "Grants 1.5x hydration for the Cookie Pack.",
                    9.50f,
                    UpgradeEffects.BoostCookiePack
                )
            },
            {
                "BlufBoost",
                new Upgrade(
                    "Bluf Bar Booster",
                    "Grants 1.5x hydration for the Bluf Bar.",
                    13.50f,
                    UpgradeEffects.BoostGranolaBar
                )
            },
        };
    }
}
 
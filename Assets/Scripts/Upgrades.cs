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

        // adding items to vending machine
        public static void AddCookiePack() {
            CurrentRun.snacks.Add("CookiePack");
        }
        public static void AddCrocorade() {
            CurrentRun.drinks.Add("Crocorade");
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
    }

    public static class UpgradePool {
        public static List<Upgrade> data = new() {
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
            }
        };
    }
}
 
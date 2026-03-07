using System;
using System.Collections.Generic;
using VendettaLib;
using vl = VendettaLib;

namespace Modifiers {

    public class LevelModifier {
        public string name;
        public string description;
        public ModifierType type;
        public Action effect;

        public LevelModifier(string name, string desc, ModifierType type, Action effect) {
            this.name = name;
            this.description = desc;
            this.type = type;
            this.effect = effect;
        }
    };

    public static class ModBuffs {
        private static Run CurrentRun => GameManager.Instance.run;

        public static void CoolDayBuff() {
            CurrentRun.hydrationGoal *= 0.8f;
        }
        public static void BigBonusBuff() {
            CurrentRun.income += 10.00f;
        }
        public static void BigDeliveryBuff() {
            CurrentRun.priceMult *= 0.8f;
        }
        public static void FullBuff() {
            CurrentRun.satiationGoal *= 0.8f;
        }
    };

    public static class ModDebuffs {
        private static Run CurrentRun => GameManager.Instance.run;

        public static void HotDayDebuff() {
            CurrentRun.hydrationGoal *= 1.2f;
        }
        public static void RecessionDebuff() {
            CurrentRun.income -= 10.00f;
        }
        public static void ShortageDebuff() {
            CurrentRun.priceMult *= 1.20f;
        }
        public static void HungryDebuff() {
            CurrentRun.satiationGoal *= 1.2f;
        }
    }

    public static class ModifierPool {
        public static readonly List<LevelModifier> data = new() {
            {
                new LevelModifier(
                    "Hot Day",
                    "Warmer than usual! x1.2 hydration requirement.",
                    vl.ModifierType.Debuff,
                    ModDebuffs.HotDayDebuff
                )
            },
            {
                new LevelModifier(
                    "Recession",
                    "Lower payout (-$10.00)! Plan accordingly.",
                    vl.ModifierType.Debuff,
                    ModDebuffs.RecessionDebuff
                )
            },
            {
                new LevelModifier(
                    "Shortage",
                    "Snacks and drinks 20% more expensive today!",
                    vl.ModifierType.Debuff,
                    ModDebuffs.ShortageDebuff
                )
            },
            {
                new LevelModifier(
                    "Hungry",
                    "You're hungrier than usual. 1.2x satiation goal!",
                    vl.ModifierType.Debuff,
                    ModDebuffs.HungryDebuff
                )
            },
            {
                new LevelModifier(
                    "Cool Day!",
                    "Cooler than usual! x0.8 hydration requirement.",
                    vl.ModifierType.Buff,
                    ModBuffs.CoolDayBuff
                )
            },
            {
                new LevelModifier(
                    "Big Bonus!",
                    "Higher payout (+$10.00)! Congratulations!",
                    vl.ModifierType.Buff,
                    ModBuffs.BigBonusBuff
                )
            },
            {
                new LevelModifier(
                    "Big Delivery!",
                    "Snacks and drinks 20% off today!",
                    vl.ModifierType.Buff,
                    ModBuffs.BigDeliveryBuff
                )
            },
            {
                new LevelModifier(
                    "Full!",
                    "You feel more full! 0.8x satiation goal!",
                    vl.ModifierType.Buff,
                    ModBuffs.FullBuff
                )
            }
        };
    }
}

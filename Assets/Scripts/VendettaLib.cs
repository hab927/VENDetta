using System.Collections.Generic;
using UnityEngine;

namespace VendettaLib {
    public enum Freshness {
        Expired,
        Stale,
        Fresh,
    }

    public enum ModifierType {
        Buff,
        Debuff
    }

    public class VendingMachineItem {
        public string name;
        public string blurb;
        public float price;
        public float sat;
        public float hyd;

        public VendingMachineItem(string name, string blurb, float price, float sat, float hyd) {
            this.name = name;
            this.blurb = blurb;
            this.price = price;
            this.sat = sat;
            this.hyd = hyd;
        }
    }

    public struct LevelProps {
        public float sGoal;
        public float hGoal;
        public int modifiers;

        public LevelProps(int s, int h, int m) {
            sGoal = s;
            hGoal = h;
            modifiers = m;
        }
    }

    public static class VendingLevels {
        public static List<LevelProps> levels = new() {
            new(10, 10, 1),
            new(20, 20, 1),
            new(35, 25, 2),
            new(50, 50, 2),
            new(75, 75, 2),
            new(100, 100, 3)
        };
    }

    public static class Helpers {
        // returns true if the rolled number is less than the percent chance
        public static bool Roll(float chance) {
            float roll = Random.value;
            if (roll <= chance) {
                return true;
            }
            else {
                return false;
            }
        }

        // for win screen
        public static string TimeAsString(float seconds) {
            int wholeSeconds = (int)seconds;
            string M = (wholeSeconds / 60).ToString();
            string S = (wholeSeconds % 60).ToString();
            
            return M + "m " + S + "s";
        }
    }
}

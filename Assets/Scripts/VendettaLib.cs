using System.Collections.Generic;
using UnityEngine;

namespace VendettaLib {
    public enum Freshness {
        Expired,
        Stale,
        Fresh,
    }

    public class VendingMachineItem {
        public string name;
        public string blurb;
        public float price;
        public float sat;
        public float hyd;

        // modifiers
        public bool fresh; // base: 25% more score
        public bool stale; // base: 25% less score

        public VendingMachineItem(string name, string blurb, float price, float sat, float hyd) {
            this.name = name;
            this.blurb = blurb;
            this.price = price;
            this.sat = sat;
            this.hyd = hyd;

            fresh = false;
            stale = false;
        }
    }

    public struct LevelProps {
        public float sGoal;
        public float hGoal;

        public LevelProps(int s, int h) {
            sGoal = s;
            hGoal = h;
        }
    }

    public static class VendingLevels {
        public static List<LevelProps> levels = new() {
            new(10, 10),
            new(20, 20),
            new(25, 25),
            new(35, 35),
            new(50, 50)
        };
    }

    public static class Helpers {
        // returns true if the rolled number is less than the percent chance
        public static bool Roll(float chance) {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll < chance) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}

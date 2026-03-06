using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using vl = VendettaLib;
using Upgrades;

public class Run {
    // basic run variables
    public float level;
    public float income;

    public float money;
    public float satiation;
    public float hydration;

    public int stockMin;
    public int stockMax;

    // fresh and stale variables
    public float freshMult;
    public float expiredMult;

    public float freshChance;
    public float expiredChance;

    // level goals
    public float satiationGoal;
    public float hydrationGoal;

    // multipliers to keep track of during the game
    public float satGoalMult;
    public float hydGoalMult;

    // list of upgrades
    public List<Upgrade> upgrades;

    // upgrade flags
    public bool bonuses;

    private static readonly Dictionary<string, vl.VendingMachineItem> DefaultProducts = new() {
        { "ChipBag", new vl.VendingMachineItem("Bag of Chips", "Good ol' chips!", 1.50f, 5.0f, 0.0f) },
        { "WaterBottle", new vl.VendingMachineItem("Bottle of Water", "Fresh and crisp!", 1.25f, 0.0f, 5.0f) },
        { "CookiePack", new vl.VendingMachineItem("Cookie Pack", "Sweet and tasty cookies!", 1.75f, 7.5f, 0.0f) },
        { "Crocorade", new vl.VendingMachineItem("Crocarade", "Electrolytes! (It's called this for legal reasons.)", 2.00f, 0.0f, 10.0f) }
    };

    public Dictionary<string, vl.VendingMachineItem> products = new(DefaultProducts);
    public List<string> snacks;
    public List<string> drinks;

    public Run() {
        products = new(DefaultProducts);

        snacks = new() { "ChipBag" };
        drinks = new() { "WaterBottle" };

        level = 0;
        income = 25.0f;
        money = 10.0f;
        satiation = 0.0f;
        hydration = 0.0f;

        stockMin = 1;
        stockMax = 3;

        // fresh and stale variables
        freshMult = 1.25f;
        expiredMult = 0.75f;

        freshChance = 0.2f; // 20% chance for stale and fresh
        expiredChance = 0.2f;

        // level goals
        satiationGoal = 10.0f;
        hydrationGoal = 10.0f;

        // multipliers
        satGoalMult = 1.0f;
        hydGoalMult = 1.0f;

        // upgrades list
        upgrades = new();

        // upgrades flags
        bonuses = false;
}

    public void AddMoney(float m) {
        money += m;
        UIManager.instance.UpdateMoneyDisplay();
    }
    public void SubtractMoney(float m) {
        money -= m;
        UIManager.instance.UpdateMoneyDisplay();
    }

    public void AddSatiation(float s) {
        satiation += s;
        UIManager.instance.UpdateSatiationDisplay();
    }
    public void SubtractSatiation(float s) {
        satiation -= s;
        UIManager.instance.UpdateSatiationDisplay();
    }

    public void AddHydration(float h) {
        hydration += h;
        UIManager.instance.UpdateHydrationDisplay();
    }
    public void SubtractHydration(float h) {
        hydration -= h;
        UIManager.instance.UpdateHydrationDisplay();
    }

    public void NextLevel() {
        level++;
        hydration = 0.0f;
        satiation = 0.0f;
        satiationGoal = satGoalMult * vl.VendingLevels.levels[(int)level].sGoal;
        hydrationGoal = hydGoalMult * vl.VendingLevels.levels[(int)level].hGoal;
        UIManager.instance.UpdateHydrationDisplay();
        UIManager.instance.UpdateSatiationDisplay();
        ItemPlacer.Instance.ResetItems();
    }

    public void AddUpgrade(Upgrade u) {
        upgrades.Add(u);
    }
}

using System.Collections.Generic;
using System.Linq;

using Upgrades;
using Modifiers;
using vl = VendettaLib;
using UnityEngine;

public class Run {
    // store base stats to keep track of permanent upgrades
    private Run baseRun;

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

    // probabilities
    public float freshChance;
    public float expiredChance;
    public float goodModChance;

    // level goals
    public float satiationGoal;
    public float hydrationGoal;

    // multipliers to keep track of during the game
    public float satGoalMult;
    public float hydGoalMult;
    public float priceMult;

    // list of upgrades
    public List<Upgrade> upgrades;

    // list of modifiers
    public List<LevelModifier> modifiers;

    // upgrade flags
    public bool bonuses;

    private static readonly Dictionary<string, vl.VendingMachineItem> DefaultProducts = new() {
        { "ChipBag", new vl.VendingMachineItem("Bag of Chips", "Good ol' chips!", 1.50f, 5.0f, 0.0f) },
        { "WaterBottle", new vl.VendingMachineItem("Bottle of Water", "Fresh and crisp!", 1.50f, 0.0f, 5.0f) },
        { "CookiePack", new vl.VendingMachineItem("Cookie Pack", "Sweet and tasty cookies!", 1.75f, 10.0f, 0.0f) },
        { "Crocorade", new vl.VendingMachineItem("Crocarade", "Electrolytes! (It's called this for legal reasons.)", 1.75f, 0.0f, 10.0f) },
        { "SodaBottle", new vl.VendingMachineItem("Joke-a Cola", "Fizzy, like TV static!", 2.50f, 0.0f, 15.0f) },
        { "GranolaBar", new vl.VendingMachineItem("Bluf Bar", "Packed full of protein & nutrients!", 2.50f, 15.0f, 0.0f) }
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

        // probabilities
        freshChance = 0.2f; // 20% chance for stale and fresh
        expiredChance = 0.2f;
        goodModChance = 0.5f;

        // level goals
        satiationGoal = 10.0f;
        hydrationGoal = 10.0f;

        // multipliers
        satGoalMult = 1.0f;
        hydGoalMult = 1.0f;
        priceMult = 1.0f;

        // upgrades list
        upgrades = new();

        // upgrades flags
        bonuses = false;

        // modifiers
        modifiers = new();
    }

    public void SaveToBase() {
        baseRun = (Run)this.MemberwiseClone();
    }


    public void AddMoney(float m) {
        money += m;
        money = Mathf.Round(money * 10f) * 0.1f;
        UIManager.instance.UpdateMoneyDisplay();
        SaveToBase();
    }
    public void SubtractMoney(float m) {
        money -= m;
        money = Mathf.Round(money * 10f) * 0.1f;
        UIManager.instance.UpdateMoneyDisplay();
        SaveToBase();
    }

    public void AddSatiation(float s) {
        satiation += s;
        satiation = Mathf.Round(satiation * 10f) * 0.1f;
        UIManager.instance.UpdateSatiationDisplay();
        SaveToBase();
    }
    public void SubtractSatiation(float s) {
        satiation -= s;
        satiation = Mathf.Round(satiation * 10f) * 0.1f;
        UIManager.instance.UpdateSatiationDisplay();
        SaveToBase();
    }

    public void AddHydration(float h) {
        hydration += h;
        hydration = Mathf.Round(hydration * 10f) * 0.1f;
        UIManager.instance.UpdateHydrationDisplay();
        SaveToBase();
    }
    public void SubtractHydration(float h) {
        hydration -= h;
        hydration = Mathf.Round(hydration * 10f) * 0.1f;
        UIManager.instance.UpdateHydrationDisplay();
        SaveToBase();
    }

    public void SetModifiers(int amt) {
        List<LevelModifier> buffs = ModifierPool.data.Where(m => m.type == vl.ModifierType.Buff).ToList();
        List<LevelModifier> debuffs = ModifierPool.data.Where(m => m.type == vl.ModifierType.Debuff).ToList();

        modifiers = new();

        for (int i = 0; i < amt; i++) {
            List<LevelModifier> choices = (vl.Helpers.Roll(goodModChance) ? buffs : debuffs);
            modifiers.Add(choices[Random.Range(0, choices.Count)]);
        }
    }

    public void ApplyModifiers() {
        foreach (LevelModifier modifier in modifiers) {
            modifier.effect();
        }
    }

    public void AddUpgrade(Upgrade u) {
        upgrades.Add(u);
        SaveToBase();
    }

    public void NextLevel() {
        level++;

        ResetToBaseRun();

        hydration = 0.0f;
        satiation = 0.0f;
        satiationGoal = satGoalMult * vl.VendingLevels.levels[(int)level].sGoal;
        hydrationGoal = hydGoalMult * vl.VendingLevels.levels[(int)level].hGoal;

        SetModifiers(vl.VendingLevels.levels[(int)level].modifiers);

        ApplyModifiers();

        UIManager.instance.UpdateHydrationDisplay();
        UIManager.instance.UpdateSatiationDisplay();
        UIManager.instance.UpdateModifierDisplay();
        ItemPlacer.Instance.ResetItems();
    }

    public void ResetToBaseRun() {
        if (baseRun == null) return;

        this.income = baseRun.income;
        this.hydrationGoal = baseRun.hydrationGoal;
        this.priceMult = baseRun.priceMult;
        this.satiationGoal = baseRun.satiationGoal;
    }
}

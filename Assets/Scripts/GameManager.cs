using JetBrains.Annotations;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Upgrades;
using VendettaLib;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Run run;
    public UpgradePool availableUpgrades;

    public UnityEvent moneyChanged;
    public UnityEvent satiationChanged;
    public UnityEvent hydrationChanged;

    public UnityEvent levelEnded;
    public UnityEvent levelStarted;

    public UnityEvent gameLost;
    public UnityEvent gameWon;

    // stats purely just for fun
    public float timeElapsed;
    public int eatenSnacks;
    public int drankDrinks;

    // flags
    public bool inShopScreen = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start()
    {
        timeElapsed = 0.0f;

        run = new Run();
        availableUpgrades = new UpgradePool();

        moneyChanged.AddListener(UIManager.instance.UpdateMoneyDisplay);
        satiationChanged.AddListener(UIManager.instance.UpdateSatiationDisplay);
        hydrationChanged.AddListener(UIManager.instance.UpdateHydrationDisplay);

        levelEnded.AddListener(UIManager.instance.EndLevelAnimation);
        levelEnded.AddListener(ShopManager.Instance.SetUpgradesInShop);
        levelEnded.AddListener(CheckForWin);

        // all functions that will be called at start of level
        levelStarted.AddListener(ItemPlacer.Instance.ResetItems);
        levelStarted.AddListener(UIManager.instance.UpdateDayDisplay);
        levelStarted.AddListener(UIManager.instance.UpdateHydrationDisplay);
        levelStarted.AddListener(UIManager.instance.UpdateSatiationDisplay);
        levelStarted.AddListener(UIManager.instance.UpdateModifierDisplay);
        levelStarted.AddListener(UIManager.instance.ClearItemDisplay);
        levelStarted.AddListener(UIManager.instance.UpdateKeypadScreen);

        gameLost.AddListener(UIManager.instance.ShowLoseScreen);
        gameWon.AddListener(UIManager.instance.ShowWinScreen);

        ItemPlacer.Instance.PlaceItems();

        run.satiationGoal = VendingLevels.levels[0].sGoal;
        run.hydrationGoal = VendingLevels.levels[0].hGoal;
        run.SetModifiers(VendingLevels.levels[0].modifiers);
        run.ApplyModifiers();

        levelStarted.Invoke();

        levelStarted.AddListener(UIManager.instance.StartLevelAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (run.satiation >= run.satiationGoal && run.hydration >= run.hydrationGoal && !inShopScreen) {
            inShopScreen = true;
            levelEnded.Invoke();
        }
    }

    public void CheckForLoss() {
        if (run.satiationGoal < run.satiation && run.hydrationGoal < run.hydration) {  // win by definition
            return;
        }
        foreach (string snack in run.snacks) {
            if (run.products[snack].price < run.money) { // check if we can afford any more snacks
                return;
            }
        }
        foreach (string drink in run.drinks) {
            if (run.products[drink].price < run.money) { // check if we can afford any more drinks
                return;
            }
        }
        SoundManager.instance.PlayLoseSound();
        gameLost.Invoke();
        return;
    }

    public void CheckForWin() {
        if (run.level == 5) {
            SoundManager.instance.PlayWinSound();
            gameWon.Invoke();
        }
    }

    public void NewRun() {
        run = new Run();
        availableUpgrades = new UpgradePool();

        run.satiationGoal = VendingLevels.levels[0].sGoal;
        run.hydrationGoal = VendingLevels.levels[0].hGoal;
        run.SetModifiers(VendingLevels.levels[0].modifiers);
        run.ApplyModifiers();
    }
}

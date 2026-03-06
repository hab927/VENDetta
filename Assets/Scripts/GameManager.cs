using JetBrains.Annotations;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;
using VendettaLib;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Run run;

    public UnityEvent moneyChanged;
    public UnityEvent satiationChanged;
    public UnityEvent hydrationChanged;

    public UnityEvent levelEnded;
    public UnityEvent levelStarted;

    public UnityEvent gameLost;

    // flags
    public bool inShopScreen = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        run = new Run();

        run.satiationGoal = VendingLevels.levels[0].sGoal;
        run.hydrationGoal = VendingLevels.levels[0].hGoal;

        moneyChanged.AddListener(UIManager.instance.UpdateMoneyDisplay);
        satiationChanged.AddListener(UIManager.instance.UpdateSatiationDisplay);
        hydrationChanged.AddListener(UIManager.instance.UpdateHydrationDisplay);

        levelEnded.AddListener(UIManager.instance.EndLevelAnimation);
        levelEnded.AddListener(ShopManager.Instance.SetUpgradesInShop);

        levelStarted.AddListener(UIManager.instance.StartLevelAnimation);
        levelStarted.AddListener(ItemPlacer.Instance.ResetItems);

        UIManager.instance.UpdateKeypadScreen();
        UIManager.instance.ClearItemDisplay();
        ItemPlacer.Instance.PlaceItems();
    }

    // Update is called once per frame
    void Update()
    {
        // hacks to test the UI
        //if (Input.GetKeyDown(KeyCode.H)) {
        //    run.hydration++;
        //    Debug.Log(run.hydration);
        //    hydrationChanged.Invoke();
        //}
        //if (Input.GetKeyDown(KeyCode.S)) {
        //    run.satiation++;
        //    Debug.Log(run.satiation);
        //    satiationChanged.Invoke();
        //}
        //if (Input.GetKeyDown(KeyCode.E)) {
        //    Debug.Log("ending level");
        //    levelEnded.Invoke();
        //}
        //if (Input.GetKeyDown(KeyCode.R)) {
        //    Debug.Log("starting level");
        //    levelStarted.Invoke();
        //}

        if (run.satiation >= run.satiationGoal && run.hydration >= run.hydrationGoal && !inShopScreen) {
            inShopScreen = true;
            levelEnded.Invoke();
        }
    }

    public void HasLost() {
        if (run.satiationGoal < run.satiation) {
            return;
        }
        else if (run.hydrationGoal < run.hydration) {
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
        gameLost.Invoke();
        return;
    }
}

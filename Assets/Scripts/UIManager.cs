using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using vl = VendettaLib;
using VendettaLib;
using Modifiers;
using static GameManager;

public class UIManager : MonoBehaviour
{
    public static Run CurrentRun => Instance.run; // alias for easier readability
    public static UIManager instance { get; private set; }

    public string keypadInput = "";

    public TMP_Text moneyTxt;
    public TMP_Text satiationTxt;
    public TMP_Text hydrationTxt;
    public TMP_Text screenTxt;
    public TMP_Text payoutTxt;

    public TMP_Text itemDisplay;
    public GameObject displayImg;

    public Slider satiationSlider;
    public Slider hydrationSlider;

    public UnityEvent displayChanged;
    public UnityEvent payoutShown;

    public List<GameObject> ItemsOnRight;
    public List<GameObject> ItemsOnLeft;
    public GameObject shop;

    public TMP_Text modifierDisplay;
    public TMP_Text dayDisplay;

    // shop buttons
    public Button upgradeButton1;
    public Button upgradeButton2;
    public Button upgradeButton3;

    public GameObject loseScreen;
    public GameObject winScreen;
    public TMP_Text winStats;

    // continue button
    public GameObject continueContainer;

    // quit to menu panel
    public GameObject exitContainer;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    private void Start() {
        displayChanged.AddListener(UpdateKeypadScreen);
        payoutShown.AddListener(
            () => {
                if (Instance.run.bonuses) {
                    if (vl.Helpers.Roll(0.25f)) Instance.run.AddMoney(10.0f);
                }
                Instance.run.AddMoney(Instance.run.income);
            }
        );
    }

    public void UpdateMoneyDisplay() {
        moneyTxt.text = "$" + CurrentRun.money.ToString("0.00");
    }
    public void UpdateSatiationDisplay() {
        satiationTxt.text = "Satiation: " + CurrentRun.satiation.ToString("0.0") + "/" + CurrentRun.satiationGoal.ToString("0.0");
        satiationSlider.value = CurrentRun.satiation;
        satiationSlider.maxValue = CurrentRun.satiationGoal;
    }
    public void UpdateHydrationDisplay() {
        hydrationTxt.text = "Hydration: " + CurrentRun.hydration.ToString("0.0") + "/" + CurrentRun.hydrationGoal.ToString("0.0");
        hydrationSlider.value = CurrentRun.hydration;
        hydrationSlider.maxValue = CurrentRun.hydrationGoal;
    }
    public void UpdateKeypadScreen() {
        string text = keypadInput;
        while (text.Length < 2) {
            text += "_";
        }
        screenTxt.text = text;
    }
    public void UpdateModifierDisplay() {
        string text = "";
        foreach (LevelModifier modifier in CurrentRun.modifiers) {
            if (modifier.type == ModifierType.Buff) {           // we got a buff
                text += "<color=#00FF00>" + modifier.name + "</color>: " + "<color=#999999>" + modifier.description + "</color>\n";
            }
            else {                                              // we got a debuff
                text += "<color=#FF0000>" + modifier.name + "</color>: " + "<color=#999999>" + modifier.description + "</color>\n";
            }
        }
        modifierDisplay.text = text;
    }
    public void UpdateDayDisplay() {
        string text = "DAY " + (CurrentRun.level + 1) + "/6";
        if (CurrentRun.level > 5) text = "DAY 6/6";
        dayDisplay.text = text;
    }

    public void ClearItemDisplay() {
        string text = "PLEASE\nSELECT\nAN\nITEM";
        displayImg.SetActive(false);
        itemDisplay.text = text;
    }

    public void UpdateItemDisplay(GameObject item) {
        string ID = item.name; // should match the key in the run variables' products
        string name = CurrentRun.products[ID].name;
        string desc = CurrentRun.products[ID].blurb;
        string price = (CurrentRun.products[ID].price * CurrentRun.priceMult).ToString("0.00");
        string text =   "ITEM " + keypadInput + ":\n" +
                        name + "\n" +
                        "$" + price + "\n" +
                        "<size=25>" + desc + "</size>\n" +
                        "HYD: " + CurrentRun.products[ID].hyd + "\n" +
                        "SAT: " + CurrentRun.products[ID].sat + "\n";

        displayImg.SetActive(true);
        Image img = displayImg.GetComponent<Image>();
        img.sprite = item.GetComponent<SpriteRenderer>().sprite;
        img.preserveAspect = true;
        itemDisplay.text = text;
    }

    public void ShowPayout() {
        SoundManager.instance.PlayCoinsFalling();
        payoutTxt.text = "$" + CurrentRun.income.ToString("0.00");
        payoutShown.Invoke();
    }

    public void ActivateShopButtons() {
        upgradeButton1.interactable = true;
        upgradeButton2.interactable = true;
        upgradeButton3.interactable = true;
    }

    // win & lose screens
    public void ShowLoseScreen() {
        loseScreen.SetActive(true);
        loseScreen.GetComponent<Animator>().Play("Fade In");
    }
    public void ShowWinScreen() {
        winScreen.SetActive(true);
        UpdateWinStatsText();
        winScreen.GetComponent<Animator>().Play("Fade In");
    }
    public void UpdateWinStatsText() {
        string text =   "Time spent: " + vl.Helpers.TimeAsString(GameManager.Instance.timeElapsed) + "\n" +
                        "Snacks eaten: " + GameManager.Instance.eatenSnacks.ToString() + "\n" +
                        "Drinks drank: " + GameManager.Instance.drankDrinks.ToString();
        winStats.text = text;
    }

    // animations
    public void MoveItemsAway() {
        SoundManager.instance.PlayWhoosh();
        foreach (GameObject obj in ItemsOnRight) {
            obj.GetComponent<Animator>().Play("Fly Out to Right");
        }
        foreach (GameObject obj in ItemsOnLeft) {
            obj.GetComponent<Animator>().Play("Fly Out to Left");
        }
    }
    public void BringItemsIn() {
        SoundManager.instance.PlayWhoosh();
        foreach (GameObject obj in ItemsOnRight) {
            obj.GetComponent<Animator>().SetTrigger("ShopEnd");
        }
        foreach (GameObject obj in ItemsOnLeft) {
            obj.GetComponent<Animator>().SetTrigger("ShopEnd");
        }
    }
    public void MoveShopIn() {
        SoundManager.instance.PlayComputerStartup();
        if (CurrentRun.level <= 4) {            // this will not move the shop in when you win
            shop.GetComponent<Animator>().Play("shop fly in");
        }
    }
    public void PushShopOut() {
        SoundManager.instance.PlayWhoosh();
        shop.GetComponent<Animator>().SetTrigger("Continue");
    }

    public void AnimateFlavorText(Transform origin, Freshness f) { // animate a flavor text at the given transform
        GameObject dropText = (GameObject)Resources.Load("Drop Text");
        GameObject empty = new();
        GameObject flavor = Instantiate(dropText);
        empty.transform.position = origin.transform.position;
        flavor.transform.SetParent(empty.transform, false);

        TMP_Text title = flavor.transform.Find("Title").gameObject.GetComponent<TMP_Text>();
        TMP_Text value = flavor.transform.Find("Value").gameObject.GetComponent<TMP_Text>();

        // set properties based on freshness
        switch (f) {
            case Freshness.Expired:
                title.faceColor = Color.red;
                value.faceColor = Color.red;
                title.text = "Expired!";
                value.text = "x" + CurrentRun.expiredMult.ToString("0.00");
                break;
            case Freshness.Stale:
                title.text = "Okay!";
                value.text = "x1.00";
                break;
            case Freshness.Fresh:
                title.faceColor = Color.green;
                value.faceColor = Color.green;
                title.text = "Fresh!!";
                value.text = "x" + CurrentRun.freshMult.ToString("0.00");
                break;
        }

        StartCoroutine(FlavorCR(empty));
    }
    public IEnumerator FlavorCR(GameObject p) {
        p.GetComponentInChildren<Animator>().Play("Fly Up and Fade");
        yield return new WaitForSeconds(0.75f);
        Destroy(p);
    }

    public void EndLevelAnimation() {
        StartCoroutine(EndLevelCR());
    }
    public void StartLevelAnimation() {
        if (CurrentRun.level <= 5) {
            StartCoroutine(StartLevelCR());
        }
    }

    private IEnumerator EndLevelCR() {
        continueContainer.SetActive(false);
        MoveItemsAway();
        yield return new WaitForSeconds(0.25f);
        MoveShopIn();
        yield return new WaitForSeconds(1.0f);
        ShowPayout();
        yield return new WaitForSeconds(1.0f);
        SoundManager.instance.PlayComputerGlitch();
        continueContainer.SetActive(true);
    }
    private IEnumerator StartLevelCR() {
        PushShopOut();
        yield return new WaitForSeconds(0.5f);
        BringItemsIn();
        payoutTxt.text = "";
        Instance.inShopScreen = false;
    }
}

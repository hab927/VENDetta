using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using vl = VendettaLib;
using static GameManager;
using VendettaLib;

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

    // shop buttons
    public Button upgradeButton1;
    public Button upgradeButton2;
    public Button upgradeButton3;

    public Canvas loseScreen;

    // continue button
    public GameObject continueContainer;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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
        satiationTxt.text = "Satiation: " + CurrentRun.satiation.ToString("F1") + "/" + CurrentRun.satiationGoal.ToString("F1");
        satiationSlider.value = CurrentRun.satiation;
        satiationSlider.maxValue = CurrentRun.satiationGoal;
    }
    public void UpdateHydrationDisplay() {
        hydrationTxt.text = "Hydration: " + CurrentRun.hydration.ToString("F1") + "/" + CurrentRun.hydrationGoal.ToString("F1");
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

    public void ClearItemDisplay() {
        string text = "PLEASE\nSELECT\nAN\nITEM";
        displayImg.SetActive(false);
        itemDisplay.text = text;
    }

    public void UpdateItemDisplay(GameObject item) {
        string ID = item.name; // should match the key in the run variables' products
        string name = CurrentRun.products[ID].name;
        string desc = CurrentRun.products[ID].blurb;
        string price = CurrentRun.products[ID].price.ToString("0.00");
        string text =   "ITEM " + keypadInput + ":\n" +
                        name + "\n" +
                        "$" + price + "\n" +
                        desc;

        displayImg.SetActive(true);
        Image img = displayImg.GetComponent<Image>();
        img.sprite = item.GetComponent<SpriteRenderer>().sprite;
        img.preserveAspect = true;
        itemDisplay.text = text;
    }

    public void ShowPayout() {
        payoutTxt.text = "$" + CurrentRun.income.ToString("0.00");
        payoutShown.Invoke();
    }

    public void ActivateShopButtons() {
        upgradeButton1.interactable = true;
        upgradeButton2.interactable = true;
        upgradeButton3.interactable = true;
    }

    // animations
    public void MoveItemsAway() {
        foreach (GameObject obj in ItemsOnRight) {
            obj.GetComponent<Animator>().Play("Fly Out to Right");
        }
        foreach (GameObject obj in ItemsOnLeft) {
            obj.GetComponent<Animator>().Play("Fly Out to Left");
        }
    }
    public void BringItemsIn() {
        foreach (GameObject obj in ItemsOnRight) {
            obj.GetComponent<Animator>().SetTrigger("ShopEnd");
        }
        foreach (GameObject obj in ItemsOnLeft) {
            obj.GetComponent<Animator>().SetTrigger("ShopEnd");
        }
    }
    public void MoveShopIn() {
        shop.GetComponent<Animator>().Play("shop fly in");
    }
    public void PushShopOut() {
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
                title.fontSize = 20;
                title.text = "EXPIRED!";
                value.text = "x" + CurrentRun.expiredMult.ToString("0.00");
                break;
            case Freshness.Stale:
                title.fontSize = 26;
                title.text = "STALE!";
                value.text = "x1.00";
                break;
            case Freshness.Fresh:
                title.fontSize = 26;
                title.faceColor = Color.green;
                value.faceColor = Color.green;
                title.text = "FRESH!";
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
        StartCoroutine(StartLevelCR());
    }

    private IEnumerator EndLevelCR() {
        continueContainer.SetActive(false);
        MoveItemsAway();
        yield return new WaitForSeconds(0.25f);
        MoveShopIn();
        yield return new WaitForSeconds(1.0f);
        ShowPayout();
        yield return new WaitForSeconds(1.0f);
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

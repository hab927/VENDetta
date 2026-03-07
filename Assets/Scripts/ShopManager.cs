using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    // text fields
    public TMP_Text upgrade1Name;
    public TMP_Text upgrade1Price;
    public TMP_Text upgrade1Description;

    public TMP_Text upgrade2Name;
    public TMP_Text upgrade2Price;
    public TMP_Text upgrade2Description;

    public TMP_Text upgrade3Name;
    public TMP_Text upgrade3Price;
    public TMP_Text upgrade3Description;

    // list of upgrade choices
    public List<Upgrade> choices;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void SetUpgradesInShop() {
        Run run = GameManager.Instance.run;
        UpgradePool pool = GameManager.Instance.availableUpgrades;
        List<Upgrade> notBoughtYet = pool.data.Where(u => !run.upgrades.Any(bought => bought.name == u.name)).ToList();
        choices = notBoughtYet.OrderBy(item => Guid.NewGuid()).Take(3).ToList();

        UIManager.instance.ActivateShopButtons();

        if (choices.Count >= 1) {
            upgrade1Name.text = choices[0].name;
            upgrade1Description.text = choices[0].description;
            upgrade1Price.text = "$" + choices[0].price.ToString("0.00");
            UIManager.instance.upgradeButton1.interactable = true;
        }
        if (choices.Count >= 2) {
            upgrade2Name.text = choices[1].name;
            upgrade2Description.text = choices[1].description;
            upgrade2Price.text = "$" + choices[1].price.ToString("0.00");
            UIManager.instance.upgradeButton2.interactable = true;
        }
        if (choices.Count >= 3) {
            upgrade3Name.text = choices[2].name;
            upgrade3Description.text = choices[2].description;
            upgrade3Price.text = "$" + choices[2].price.ToString("0.00");
            UIManager.instance.upgradeButton3.interactable = true;
        }
    }

    public void ResetShop() {
        upgrade1Name.text = "[EMPTY]";
        upgrade2Name.text = "[EMPTY]";
        upgrade3Name.text = "[EMPTY]";

        upgrade1Price.text = "";
        upgrade2Price.text = "";
        upgrade3Price.text = "";

        upgrade1Description.text = "";
        upgrade2Description.text = "";
        upgrade3Description.text = "";

        UIManager.instance.upgradeButton1.interactable = false;
        UIManager.instance.upgradeButton2.interactable = false;
        UIManager.instance.upgradeButton3.interactable = false;
    }
}

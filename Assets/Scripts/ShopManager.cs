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
        List<Upgrade> notBoughtYet = UpgradePool.data.Where(u => !run.upgrades.Any(bought => bought.name == u.name)).ToList();
        choices = notBoughtYet.OrderBy(item => Guid.NewGuid()).Take(3).ToList();

        UIManager.instance.ActivateShopButtons();

        upgrade1Name.text = choices[0].name;
        upgrade2Name.text = choices[1].name;
        upgrade3Name.text = choices[2].name;

        upgrade1Description.text = choices[0].description;
        upgrade2Description.text = choices[1].description;
        upgrade3Description.text = choices[2].description;

        upgrade1Price.text = "$" + choices[0].price.ToString("0.00");
        upgrade2Price.text = "$" + choices[1].price.ToString("0.00");
        upgrade3Price.text = "$" + choices[2].price.ToString("0.00");
    }
}

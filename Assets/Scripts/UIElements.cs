using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Upgrades;

public class UIElements : MonoBehaviour
{
    public void ContinueButton() {
        GameManager.Instance.run.NextLevel();
        GameManager.Instance.levelStarted.Invoke();
    }

    public void UpgradeButton(int upgradeNumber) {
        Run run = GameManager.Instance.run;
        if (ShopManager.Instance.choices.Count > 0) {
            Upgrade upgrade = ShopManager.Instance.choices[upgradeNumber - 1];

            // check if we can afford it
            if (run.money > upgrade.price) {
                run.SubtractMoney(upgrade.price);
                upgrade.upgradeEffect();
                GameObject clicked = EventSystem.current.currentSelectedGameObject;

                if (clicked != null) {
                    if (clicked.TryGetComponent<Button>(out var btn)) { // make it so that we can't purchase it multiple times
                        btn.interactable = false;
                    }
                }

                run.upgrades.Add(upgrade);
            }
        }
    }
}

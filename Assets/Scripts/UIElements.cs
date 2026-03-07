using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Upgrades;

public class UIElements : MonoBehaviour
{
    public void ContinueButton() {
        SoundManager.instance.PlayComputerGlitch();
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
                SoundManager.instance.PlayUpgradePurchased();
                upgrade.upgradeEffect();
                GameObject clicked = EventSystem.current.currentSelectedGameObject;

                if (clicked != null) {
                    if (clicked.TryGetComponent<Button>(out var btn)) { // make it so that we can't purchase it multiple times
                        btn.interactable = false;
                    }
                }

                run.AddUpgrade(upgrade);
            }
        }
    }

    public void OpenConfirmPopup() {
        UIManager.instance.exitContainer.SetActive(true);
        SoundManager.instance.PlayButtonPress();
    }

    public void CancelQuit() {
        UIManager.instance.exitContainer.SetActive(false);
        SoundManager.instance.PlayButtonPress();
    }
}

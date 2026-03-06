using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VendettaLib;

public class KeypadButtons : MonoBehaviour
{
    public TMP_Text txt;
    public GameObject impact;

    private void Start() {
        txt = GetComponentInChildren<TMP_Text>();
    }

    public void KeypadPressed() {
        UIManager.instance.keypadInput += txt.text;
        Debug.Log(UIManager.instance.keypadInput);
        if (UIManager.instance.keypadInput.Length >= 3) {
            UIManager.instance.keypadInput = txt.text;
        }
        UIManager.instance.UpdateKeypadScreen();

        if (UIManager.instance.keypadInput.Length == 2) {
            GameObject slot = GameObject.Find(UIManager.instance.keypadInput);
            GameObject selection = slot.transform.GetChild(0).gameObject;
            if (selection != null) {
                UIManager.instance.UpdateItemDisplay(selection);
            }
            else {
                UIManager.instance.ClearItemDisplay();
            }
        }
        else {
            UIManager.instance.ClearItemDisplay();
        }
    }

    public void KeypadClear() {
        UIManager.instance.keypadInput = "";
        UIManager.instance.UpdateKeypadScreen();
    }

    public void KeypadConfirm() {
        GameObject slot = GameObject.Find(UIManager.instance.keypadInput);
        GameObject selection = slot.transform.GetChild(0).gameObject;

        Run run = GameManager.Instance.run;

        if (run.money >= run.products[selection.name].price) {
            run.money -= run.products[selection.name].price;
            UIManager.instance.UpdateMoneyDisplay();

            Renderer r = selection.GetComponent<Renderer>();
            r.sortingOrder = 10000; // go to the front so it's not obscured by anything

            StartCoroutine(AnimateFall(selection));

            // update keypad at the time the object is destroyed
            UIManager.instance.keypadInput = "";
            UIManager.instance.UpdateKeypadScreen();
        }
    }

    private IEnumerator AnimateFall(GameObject selection) {
        Run run = GameManager.Instance.run;
        Animator anim = selection.GetComponent<Animator>();
        anim.Play("Fall");

        while (selection.transform.position.y > -6) {
            yield return null;
        }

        float roll = Random.Range(0.0f, 1.0f);
        Freshness f;
        float freshnessMult = 1.0f;

        if (roll <= run.freshChance) {
            f = Freshness.Fresh;
            freshnessMult = run.freshMult;
        }
        else if (roll <= run.freshChance + run.expiredChance) {
            f = Freshness.Expired;
            freshnessMult = run.expiredMult;
        }
        else {
            f = Freshness.Stale;
        }

            UIManager.instance.AnimateFlavorText(selection.transform, f);

        if (impact != null) {
            impact.GetComponent<Animator>().Play("Fade Out");
        }

        Destroy(selection);

        // update the food fields
        run.AddHydration(run.products[selection.name].hyd * freshnessMult);
        run.AddSatiation(run.products[selection.name].sat * freshnessMult);

        GameManager.Instance.HasLost();
    }
}

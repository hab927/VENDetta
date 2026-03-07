using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VendettaLib;

public class KeypadButtons : MonoBehaviour
{
    public TMP_Text txt;
    public GameObject impact;

    private void Start() {
        txt = GetComponentInChildren<TMP_Text>();

        AudioSource audSrc = this.AddComponent<AudioSource>();
        audSrc.clip = (AudioClip)Resources.Load("button-press");
    }

    private void PlaySound() {
        AudioSource audSrc = GetComponent<AudioSource>();
        audSrc.pitch = Random.Range(1.4f, 1.6f);
        audSrc.Play();
    }
    private void PlaySoundLower() {
        AudioSource audSrc = GetComponent<AudioSource>();
        audSrc.pitch = Random.Range(0.9f, 1.1f);
        audSrc.Play();
    }

    public void KeypadPressed() {
        PlaySound();

        UIManager.instance.keypadInput += txt.text;
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
        PlaySoundLower();

        UIManager.instance.keypadInput = "";
        UIManager.instance.UpdateKeypadScreen();
    }

    public void KeypadConfirm() {
        PlaySoundLower();

        GameObject slot = GameObject.Find(UIManager.instance.keypadInput);
        GameObject selection = slot.transform.GetChild(0).gameObject;

        Run run = GameManager.Instance.run;

        if (run.money >= run.products[selection.name].price) {
            run.money -= run.products[selection.name].price * run.priceMult;
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

        StartCoroutine(WaitForLoss());
    }

    private IEnumerator WaitForLoss() {
        yield return new WaitForSeconds(1.5f);
        if (!GameManager.Instance.inShopScreen) {
            GameManager.Instance.CheckForLoss();
        }
    }
}

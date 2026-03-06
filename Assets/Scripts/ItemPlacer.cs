using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    public static ItemPlacer Instance { get; private set; }

    public static List<char> rows = new() { 'A', 'B', 'C', 'D' }; 

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void ResetItems() {
        foreach (var row in rows) {
            for (int i = 1; i < 5; i++) {
                GameObject slot = GameObject.Find(row + i.ToString());
                for (int c = slot.transform.childCount - 1; c >= 0; c--) {
                    Destroy(slot.transform.GetChild(c).gameObject);
                }
            }
        }
        PlaceItems();
    }

    public void PlaceItems() {
        // start at Row A, move across columns 1-4 and then go to B, C, D
        Run run = GameManager.Instance.run;
        foreach (var row in rows) {
            for (int i = 1; i < 5; i++) {
            GameObject slot = GameObject.Find(row + i.ToString());
            GameObject item;
                GameObject product;
                if (row == 'A' || row == 'B') {
                    product = (GameObject)Resources.Load(run.snacks[Random.Range(0, run.snacks.Count)]);
                }
                else {
                    product = (GameObject)Resources.Load(run.drinks[Random.Range(0, run.drinks.Count)]);
                }
                int amount = Random.Range(run.stockMin, run.stockMax); // instantiate anywhere from 1-3 items
                for (int j = 0; j<amount; j++) {
                    item = Instantiate(product);
                    item.name = item.name.Replace("(Clone)", ""); // to make handling easier
                    item.GetComponent<SpriteRenderer>().sortingOrder = 1000 - j;
                    item.transform.position = slot.transform.position;
                    item.transform.parent = slot.transform;
                }
            }
        }
    }
}

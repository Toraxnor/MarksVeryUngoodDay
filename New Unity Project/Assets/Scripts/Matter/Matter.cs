using UnityEngine;
using System.Collections;

public class Matter : MonoBehaviour, Clickable {
    private int percent;
    private int averageQuantity;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void click() {
        Destroy(this.gameObject);
    }
}
using UnityEngine;
using System.Collections;

public class ClickDestroy : MonoBehaviour {

    void OnMouseUp() {
        Destroy(gameObject);
    }
}

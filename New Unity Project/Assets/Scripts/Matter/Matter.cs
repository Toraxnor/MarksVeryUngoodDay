using UnityEngine;

public abstract class Matter : MonoBehaviour, Clickable {
    public abstract double Percent { get; }
    public abstract int AverageQuantity { get; }

    public void click() {
        Destroy(this.gameObject);
    }
}

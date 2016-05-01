using UnityEngine;
using System.Collections;

public class Earth : Matter {
    public double Percent {
        get {
            return RessourceDensity.EarthPercent;
        }
    }

    public int AverageQuantity {
        get {
            return RessourceDensity.EarthAverageQuantity;
        }
    }
}

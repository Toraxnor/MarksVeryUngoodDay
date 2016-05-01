using UnityEngine;
using System.Collections;

public class Copper : Matter {
    public double Percent {
        get {
            return RessourceDensity.CopperPercent;
        }
    }

    public int AverageQuantity {
        get {
            return RessourceDensity.CopperAverageQuantity;
        }
    }
}

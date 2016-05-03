

public class Copper : Matter {
    public override double Percent {
        get {
            return MapSettings.CopperPercent;
        }
    }

    public override int AverageQuantity {
        get {
            return MapSettings.CopperAverageQuantity;
        }
    }
}

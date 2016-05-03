

public class Earth : Matter {
    public override double Percent {
        get {
            return MapSettings.EarthPercent;
        }
    }

    public override int AverageQuantity {
        get {
            return MapSettings.EarthAverageQuantity;
        }
    }
}

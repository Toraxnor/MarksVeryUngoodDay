using UnityEngine;
using System.Collections;

public static class WorldSettings {

    private static float _dayTime = 24;

    public static float DayTime
    {
        get { return WorldSettings._dayTime; }
    }
}

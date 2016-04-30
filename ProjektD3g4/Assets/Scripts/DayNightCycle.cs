using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    private static float _dayTime;

	// Use this for initialization
	void Start () {
        _dayTime = WorldSettings.DayTime;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(-Vector3.forward,_dayTime * Time.deltaTime);
	}
}

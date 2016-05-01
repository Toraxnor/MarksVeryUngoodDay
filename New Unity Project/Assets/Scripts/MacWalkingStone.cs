using UnityEngine;
using System.Collections;

public class MacWalkingStone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey("a"))
            this.transform.position += Vector3.left * Time.deltaTime;
        if (Input.GetKey("d"))
            this.transform.position += Vector3.right * Time.deltaTime;
        if (Input.GetKeyDown("w"))
            this.transform.position += Vector3.up * Time.deltaTime * 1000;
    }
}

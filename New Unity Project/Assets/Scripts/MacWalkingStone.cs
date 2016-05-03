using UnityEngine;
using System.Collections;

public class MacWalkingStone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey("a"))
            this.transform.position += 3 * Vector3.left * Time.deltaTime;
        if (Input.GetKey("d"))
            this.transform.position += 3 * Vector3.right * Time.deltaTime;
        if (Input.GetKeyDown("w") || Input.GetKeyDown("space"))
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 30, 0), ForceMode2D.Impulse);
    }
}

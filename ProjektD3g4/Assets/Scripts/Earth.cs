using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour, Clickable
{

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void click()
    {
       Destroy(this.gameObject);
    }
}

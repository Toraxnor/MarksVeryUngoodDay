using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour
{
    private Camera characterCamera;

	// Use this for initialization
	void Start ()
    {
        characterCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        checkClick();
	}

    private void checkClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(characterCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                print("Hit" + hit.collider.name);

                getClickable(hit.collider).click();
            }   
        }
    }


    //Helper Functions
    private Clickable getClickable(Collider2D collider)
    {
        return collider.gameObject.GetComponent(typeof(Clickable)) as Clickable;
    }
}

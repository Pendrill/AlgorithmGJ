using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will check on whether the player has clicked on a candidate.
/// if so, it will proceed.
/// </summary>
public class SelectCandidate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //once the left mouse button if pressed 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //we send a racayst in 2d space originating from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //we check if it hits anything
            if(hit)
            {
                //save a temp reference of the object that got hit
                GameObject tempHit = hit.transform.gameObject;
                Debug.Log(tempHit.name);
            }
        }
	}
}

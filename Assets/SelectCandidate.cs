using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will check on whether the player has clicked on a candidate.
/// if so, it will proceed.
/// </summary>
public class SelectCandidate : MonoBehaviour {

    public GameObject[] candidates = new GameObject[3];
    public int currentDisplayedCandidate = 2, lastDisplayedCandidate = 0;
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

    /// <summary>
    /// Button was clicked to initiate the movement of the candidates to the right.
    /// </summary>
    public void moveRight()
    {
        //keep an index reference of the candidate currently on screen that needs to be moved
        lastDisplayedCandidate = currentDisplayedCandidate;
        //check if this is the first candidate in the array
        if(currentDisplayedCandidate == 1)
        {
            //then loop around the array
            currentDisplayedCandidate = 3;
        }else
        {
            //otherwise just get the index before it
            currentDisplayedCandidate -= 1;
        }
        //set the state to move right
        

    }

    /// <summary>
    /// Button was clicked to initiate the movement of the candidates to the left.
    /// </summary>
    public void moveLeft()
    {
        //keep an index reference of the candidate currently on screen that needs to be moved
        lastDisplayedCandidate = currentDisplayedCandidate;
        //check if this is the last candidate in the array
        if (currentDisplayedCandidate == 3)
        {
            //then loop around the array
            currentDisplayedCandidate = 1;
        }else
        {
            //otherwise just get the index after it
            currentDisplayedCandidate += 1;
        }
        //set the state to move left
    }
    
}

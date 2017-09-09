using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

/// <summary>
/// This script will check on whether the player has clicked on a candidate.
/// if so, it will proceed.
/// </summary>
public class SelectCandidate : MonoBehaviour {

    public GameObject[] candidates = new GameObject[4];
    public int currentDisplayedCandidate = 2, lastDisplayedCandidate = 0;
    public enum GameState { wait, moveRight, moveLeft };
    public GameState currentState;
    float lastStateChange = 0.0f, time = 0.0f;
    public float leftPosition, centerPosition, rightPosition;
    public GameObject buttonRight, buttonLeft;
	// Use this for initialization
	void Start () {
        setCurrentState(GameState.wait);
	}
	
	// Update is called once per frame
	void Update () {
        //once the left mouse button if pressed 
        //Debug.Log(currentState);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //we send a racayst in 2d space originating from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //we check if it hits anything
            if(hit)
            {
                //save a temp reference of the object that got hit
                GameObject tempHit = hit.transform.gameObject;
                //Debug.Log(tempHit.name);
            }
        }

        switch (currentState)
        {
            case GameState.wait:
                //do the stuff in wait
                turnLeftRightOn();
                time = 0.0f;
                break;

            case GameState.moveLeft:
                //do the stuff in moveleft
                time += Time.deltaTime / 2;
                candidates[lastDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(centerPosition, leftPosition, time), candidates[lastDisplayedCandidate].transform.position.y);
                candidates[currentDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(rightPosition, centerPosition, time), candidates[currentDisplayedCandidate].transform.position.y);

                if(getStateElapsed() > 2.0f)
                {
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;

            case GameState.moveRight:
                //Debug.Log(getStateElapsed());
                //do the stuff in moveRight
                time += Time.deltaTime / 2;
                candidates[lastDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(centerPosition, rightPosition, time), candidates[lastDisplayedCandidate].transform.position.y);
                candidates[currentDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(leftPosition, centerPosition, time), candidates[currentDisplayedCandidate].transform.position.y);

                if (getStateElapsed() > 2.0f)
                {
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;
        }
	}

    /// <summary>
    /// Button was clicked to initiate the movement of the candidates to the right.
    /// </summary>
    public void moveRight()
    {
        turnLeftRightOff();
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
        setCurrentState(GameState.moveRight);
        

    }

    /// <summary>
    /// Button was clicked to initiate the movement of the candidates to the left.
    /// </summary>
    public void moveLeft()
    {
        turnLeftRightOff();
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
        setCurrentState(GameState.moveLeft);
    }

    /// <summary>
    /// sets the current state of the game manager
    /// </summary>
    /// <param name="state"></param>
    public void setCurrentState(GameState state)
    {
        currentState = state;
        lastStateChange = Time.time;
    }

    /// <summary>
    /// returns the amount of time that has passed since the last state change
    /// </summary>
    /// <returns></returns>
    float getStateElapsed()
    {
        return Time.time - lastStateChange;
    }

    void turnLeftRightOn()
    {
        buttonLeft.SetActive(true);
        buttonRight.SetActive(true);
    }
    void turnLeftRightOff()
    {
        buttonRight.SetActive(false);
        buttonLeft.SetActive(false);
    }
    
}

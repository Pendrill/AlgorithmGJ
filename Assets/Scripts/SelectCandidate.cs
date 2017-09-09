using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will check on whether the player has clicked on a candidate.
/// if so, it will proceed.
/// </summary>
public class SelectCandidate : MonoBehaviour {

    public GameObject[] candidates = new GameObject[4], candidatePrefabs;
    public int currentDisplayedCandidate = 2, lastDisplayedCandidate = 0;
    public enum GameState { wait, start, fadeOut, moveRight, moveLeft };
    public GameState currentState;
    float lastStateChange = 0.0f, time = 0.0f, alpha;
    public float leftPosition, centerPosition, rightPosition;
    public GameObject buttonRight, buttonLeft; 
    public Image blackFader;
    bool interactable = true;
	// Use this for initialization
	void Start () {
        turnLeftRightOff();
        setCurrentState(GameState.start);
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
            if(hit && interactable)
            {
                //save a temp reference of the object that got hit
                GameObject tempHit = hit.transform.gameObject;
                //Debug.Log(tempHit.name);
            }
        }

        switch (currentState)
        {
            case GameState.start:
                //do the initializing stuff
                for (int i = 1; i < candidates.Length; i++)
                {
                    GameObject tempCandidate = Instantiate(candidatePrefabs[Random.Range(0, candidatePrefabs.Length - 1)], new Vector3(1000,1,0), Quaternion.identity);
                    candidates[i] = tempCandidate;
                }
                candidates[2].transform.position = new Vector2(centerPosition, 1);

                //fade out of black

                setCurrentState(GameState.fadeOut);
                time = 0.0f;
                               
                break;

            case GameState.fadeOut:
                //do the fade out
                time += Time.deltaTime / 2;
                Color tmp = blackFader.color;
                alpha = Mathf.Lerp(1.0f, 0.0f, time / 2);
                tmp.a = alpha;
                blackFader.color = tmp;
                if (getStateElapsed() > 4.0f)
                {
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;
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
        Debug.Log("Clicked button");
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
        interactable = true;
    }
    void turnLeftRightOff()
    {
        Debug.Log("is this getting called");
        buttonRight.SetActive(false);
        buttonLeft.SetActive(false);
        interactable = false;
    }

    
}

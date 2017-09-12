using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is our Game Manager Script.
/// This will instatiate an random set of candidates
/// Let the player shuffle through them
/// Let the player selct one of them
/// </summary>
public class SelectCandidate : MonoBehaviour {

    public GameObject[] candidates = new GameObject[4], candidatePrefabs;
    public string[] M_Names_Array, F_Names_Array, lastName_Array;
    public TextAsset M_Names, F_Names, lastName;
    public int currentDisplayedCandidate = 2, lastDisplayedCandidate = 0;
    public enum GameState { wait, start, fadeOut, moveRight, moveLeft };
    public GameState currentState;
    float lastStateChange = 0.0f, time = 0.0f, alpha;
    public float leftPosition, centerPosition, rightPosition;
    public GameObject buttonRight, buttonLeft;
    public Image blackFader;
    bool interactable = true;

    // Use this for initialization
    void Start() {
        //we make sure the left and right buttons are off
        turnLeftRightOff();
        //set the current state to the starting state
        if (M_Names != null)
        {
            M_Names_Array = M_Names.text.Split(' ');
        }
        if (F_Names != null)
        {
            F_Names_Array = F_Names.text.Split(' ');
        }
        if (lastName != null)
        {
            lastName_Array = lastName.text.Split(' ');
        }
        setCurrentState(GameState.start);
    }

    // Update is called once per frame
    void Update() {
        //if the player wants to select a candidate
        //once the left mouse button if pressed 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //we send a racayst in 2d space originating from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //we check if it hits anything
            if (hit && interactable)
            {
                //save a temp reference of the object that got hit
                GameObject tempHit = hit.transform.gameObject;
                //Debug.Log(tempHit.name);
            }
        }

        switch (currentState)
        {
            case GameState.start:
                //Spawn a set of candidated
                for (int i = 1; i < candidates.Length; i++)
                {
                    GameObject tempCandidate = Instantiate(candidatePrefabs[Random.Range(0, candidatePrefabs.Length - 1)], new Vector3(1000, 1, 0), Quaternion.identity);
                    candidates[i] = tempCandidate;
                }
                //have the middle candidate be in the center of the screen
                candidates[2].transform.position = new Vector2(centerPosition, 1);

                //fade out of black
                setCurrentState(GameState.fadeOut);
                time = 0.0f;

                break;

            case GameState.fadeOut:
                //do the fade out
                //The alpha value of the image will lerp from 1 to 0
                time += Time.deltaTime / 2;
                Color tmp = blackFader.color;
                alpha = Mathf.Lerp(1.0f, 0.0f, time / 2);
                tmp.a = alpha;
                blackFader.color = tmp;
                //Wait 4 seconds
                if (getStateElapsed() > 4.0f)
                {
                    //Set the current state to wait
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;
            //Resets values and essentially is a state of limbo where nothing happens
            case GameState.wait:
                //do the stuff in wait
                turnLeftRightOn();
                time = 0.0f;
                break;

            //Move the candidates to the left
            case GameState.moveLeft:
                //do the stuff in moveleft
                time += Time.deltaTime / 2;
                //smoothstep the position of the candidate from the center to the left
                candidates[lastDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(centerPosition, leftPosition, time), candidates[lastDisplayedCandidate].transform.position.y);
                //smoothstep the position of the candidate from the right to the center
                candidates[currentDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(rightPosition, centerPosition, time), candidates[currentDisplayedCandidate].transform.position.y);

                //wait 2 seconds
                if (getStateElapsed() > 2.0f)
                {
                    //set gamestate to wait
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;
            //Move candidates to the right
            case GameState.moveRight:
                //Debug.Log(getStateElapsed());
                //do the stuff in moveRight
                time += Time.deltaTime / 2;
                //smoothstep the position of the candidate from the center to the right
                candidates[lastDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(centerPosition, rightPosition, time), candidates[lastDisplayedCandidate].transform.position.y);
                //smoothstep the position of the candidate from the left to the center
                candidates[currentDisplayedCandidate].transform.position = new Vector2(Mathf.SmoothStep(leftPosition, centerPosition, time), candidates[currentDisplayedCandidate].transform.position.y);

                //wait 2 seconds
                if (getStateElapsed() > 2.0f)
                {
                    //set the state to wait
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
        if (currentDisplayedCandidate == 1)
        {
            //then loop around the array
            currentDisplayedCandidate = 3;
        } else
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
        } else
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

    /// <summary>
    /// turns the buttons that enables the player to go through each candidate on
    /// </summary>
    void turnLeftRightOn()
    {
        buttonLeft.SetActive(true);
        buttonRight.SetActive(true);
        interactable = true;
    }
    /// <summary>
    /// turns the buttons that enables the player to go through each candidate off
    /// </summary>
    void turnLeftRightOff()
    {
        Debug.Log("is this getting called");
        buttonRight.SetActive(false);
        buttonLeft.SetActive(false);
        interactable = false;
    }
    /// <summary>
    /// generates and returns a full name for the candidate
    /// </summary>
    /// <returns></returns>
    public string generateName(bool gender)
    {
        string fullName = "";
        if (gender)
        {
            fullName = F_Names_Array[Random.Range(0, F_Names_Array.Length)];
            fullName += " " + lastName_Array[Random.Range(0, lastName_Array.Length)];
        } else
        {
            fullName = M_Names_Array[Random.Range(0, M_Names_Array.Length)];
            fullName += " " + lastName_Array[Random.Range(0, lastName_Array.Length)];
        }

        return fullName;
    }


    
}

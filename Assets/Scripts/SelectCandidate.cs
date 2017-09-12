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

    public Camera theCamera;
    public GameObject[] candidates = new GameObject[4], candidatePrefabs;
    public string[] M_Names_Array, F_Names_Array, lastName_Array, UniversitiesArray, SkillsArray, LanguagesArray, ExperienceArray;
    public TextAsset M_Names, F_Names, lastName, Universities, Skills, Languages, Experience;
    public Text startingText;
    public int currentDisplayedCandidate = 2, lastDisplayedCandidate = 0;
    public enum GameState { wait, start, fadeOut, moveRight, moveLeft, moveDown, moveUp, fadeIn };
    public GameState currentState;
    float lastStateChange = 0.0f, time = 0.0f, alpha;
    public float leftPosition, centerPosition, rightPosition, OriginalCameraPosition, DownCameraPosition;
    public GameObject buttonRight, buttonLeft, resumeText, hire, keepSearching;
    public Image blackFader;
    bool interactable = true;
    string startText;

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
        if (Languages != null)
        {
            LanguagesArray = Languages.text.Split('\n');
        }
        if (Universities != null)
        {
            UniversitiesArray = Universities.text.Split('\n');
        }
        if (Experience!= null)
        {
            ExperienceArray = Experience.text.Split('\n');
        }
        if (Skills != null)
        {
            SkillsArray = Skills.text.Split('\n');
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
                tempHit.GetComponent<Candidates>().setText();
                setCurrentState(GameState.moveDown);
                //Debug.Log(tempHit.name);
            }
        }

        switch (currentState)
        {
            case GameState.start:
                //Spawn a set of candidated

                startText = "Please hire the Candidate you think is most fit for the job";
                currentDisplayedCandidate = 2;
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
                startingText.text = startText;
                if (getStateElapsed() > 3.0f)
                {
                    startText = "";
                    time += Time.deltaTime / 2;
                    Color tmp = blackFader.color;
                    alpha = Mathf.Lerp(1.0f, 0.0f, time / 2);
                    tmp.a = alpha;
                    blackFader.color = tmp;
                }
                //Wait 4 seconds
                if (getStateElapsed() > 3.1f)
                {
                    //Set the current state to wait
                    resumeText.SetActive(false);
                }
                if (getStateElapsed() > 7.0f)
                {
                    //Set the current state to wait
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;

            case GameState.fadeIn:
                time += Time.deltaTime / 2;
                Color tmp2 = blackFader.color;
                alpha = Mathf.Lerp(0.0f, 1.0f, time / 2);
                tmp2.a = alpha;
                blackFader.color = tmp2;
                if (getStateElapsed() > 4.0f)
                {
                    //Set the current state to wait
                    for(int i = 1; i< candidates.Length; i++)
                    {
                        Destroy(candidates[i]);
                    }
                    theCamera.transform.position = new Vector3(0, 0, -10);
                    resumeText.SetActive(true);
                    setCurrentState(GameState.start);
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

            case GameState.moveDown:
                turnLeftRightOff();
                time += Time.deltaTime / 2;
                theCamera.transform.position = new Vector3(theCamera.transform.position.x, Mathf.SmoothStep(OriginalCameraPosition, DownCameraPosition, time), -10);
                if (getStateElapsed() > 2.0f)
                {
                    //Debug.Log("Does this get accessed");
                    hire.SetActive(true);
                    keepSearching.SetActive(true);
                    resumeText.SetActive(true);
                }
                break;

            case GameState.moveUp:
                time += Time.deltaTime / 2;
                theCamera.transform.position = new Vector3(theCamera.transform.position.x, Mathf.SmoothStep( DownCameraPosition, OriginalCameraPosition, time), -10);
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

    public string genenerateExperience(Candidates currentCandidate)
    {
        string experience = "";
        for(int i = 0; i <3; i++)
        {
            string tempExperience = ExperienceArray[Random.Range(0, ExperienceArray.Length)];
            //Debug.Log(tempSkill);
            string[] tempExperienceArray = tempExperience.Split(',');
            experience += tempExperienceArray[0] + "\n";
            currentCandidate.percentage += int.Parse(tempExperienceArray[1]);
        }

        return experience;
    }
    public string generateSkills( Candidates currentCandidate, int numberOfSkills)
    {
        string skill = "";
        for (int i = 0; i < numberOfSkills; i++)
        {
            string tempSkill = SkillsArray[Random.Range(0, SkillsArray.Length)];
            string[] tempSkillArray = tempSkill.Split(',');
            skill += tempSkillArray[0] + " ";
            currentCandidate.percentage += int.Parse(tempSkillArray[1]);
        }
        return skill;
    }

    public string generateLanguages(Candidates currentCandidate, int numberOfLanguages)
    {
        string languages = "";
        for (int i = 0; i < numberOfLanguages; i++)
        {
            string tempLanguage = LanguagesArray[Random.Range(0, LanguagesArray.Length)];
            string[] tempLanguageArray = tempLanguage.Split(',');
            languages += tempLanguageArray[0] + " ";
            currentCandidate.percentage += int.Parse(tempLanguageArray[1]);
        }
        return languages;
    }
    public string generateUniversity(Candidates currentCandidate)
    {
        string University = "";
        
        string tempUniversity = UniversitiesArray[Random.Range(0, UniversitiesArray.Length)];
        string[] tempUniversityArray = tempUniversity.Split(',');
        University += tempUniversityArray[0] + "\n";
        currentCandidate.percentage += int.Parse(tempUniversityArray[1]);
        
        return University;
    }
    public void moveUp()
    {
        
        setCurrentState(GameState.moveUp);
        resumeText.SetActive(false);
        hire.SetActive(false);
        keepSearching.SetActive(false);
        time = 0;
    }
    public void Hire()
    {
        setCurrentState(GameState.fadeIn);
        resumeText.SetActive(false);
        hire.SetActive(false);
        keepSearching.SetActive(false);
        time = 0;
    }


}

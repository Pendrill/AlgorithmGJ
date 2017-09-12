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

    //We keep a reference of the camera as to be able to move it 
    public Camera theCamera;
    //we keep a reference of the current candidates being displayed and all the candidate prefabs
    public GameObject[] candidates = new GameObject[4], candidatePrefabs;
    //We create a set of arry to be able to split all our text files. He have ones for first and last names for both men and women
    //University names, Skills, Languages known, and past work experience
    public string[] M_Names_Array, F_Names_Array, lastName_Array, UniversitiesArray, SkillsArray, LanguagesArray, ExperienceArray;
    //We keep a reference of the text files
    public TextAsset M_Names, F_Names, lastName, Universities, Skills, Languages, Experience;
    //We have a reference to the opening lines that are shown to the player at the begining of the game
    public Text startingText;
    //keep track of the index of the candidate that is being displayed and the index of the last candidate that was displayed
    //so we know when to loop around
    public int currentDisplayedCandidate = 2, lastDisplayedCandidate = 0;
    //we set up all our gamestates
    public enum GameState { wait, start, fadeOut, moveRight, moveLeft, moveDown, moveUp, fadeIn };
    //and we make sure to keep a reference of the current state
    public GameState currentState;
    //we keep a reference of how much time has passed since we last changed states
    //and just a general timer for lerping and smoothstep purposes
    //we also keep and alpha value as to fade things in and out
    float lastStateChange = 0.0f, time = 0.0f, alpha;
    //we keep left center and right postions to know where to place the candidates in the scene
    //we also keep camera positions for when it is looking at the candidates of the resume
    public float leftPosition, centerPosition, rightPosition, OriginalCameraPosition, DownCameraPosition;
    //we keep a reference of all the buttons that enable us to navigate through the game
    public GameObject buttonRight, buttonLeft, resumeText, hire, keepSearching;
    //we keep a reference to the image we will use to fade in and out
    public Image blackFader;
    //we check whether the player can interact with a candidate yet
    bool interactable = true;
    //we have a string for the opening lines of the game
    string startText;

    // Use this for initialization
    void Start() {
        //we make sure the left and right buttons are off
        turnLeftRightOff();
        
        //We split all the text files into their respective arrays
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
        //we then set the state to start
        setCurrentState(GameState.start);
    }

    // Update is called once per frame
    void Update() {

        //if the player presses escape, it quits out of the game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

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
                
                //we have the start text be displayed
                startText = "Please hire the Candidate you think is most fit for the job of Lead Gameplay Programmer";
                //we set the index to the center candidate
                currentDisplayedCandidate = 2;
                //Spawn a set of candidates
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
                if (getStateElapsed() > 4.0f)
                {
                    startText = "";
                    time += Time.deltaTime / 2;
                    Color tmp = blackFader.color;
                    alpha = Mathf.Lerp(1.0f, 0.0f, time / 2);
                    tmp.a = alpha;
                    blackFader.color = tmp;
                }
                //Wait 4 seconds
                if (getStateElapsed() > 1.1f)
                {
                    //we remove the resume text now that the instantiated candidates have a reference to them
                    resumeText.SetActive(false);
                }
                if (getStateElapsed() > 8.0f)
                {
                    //Set the current state to wait
                    setCurrentState(GameState.wait);
                    time = 0.0f;
                }
                break;

            case GameState.fadeIn:
                //do the fade in
                //The alpha value of the image will lerp from 0 to 1
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
                    //we reset the camera
                    theCamera.transform.position = new Vector3(0, 0, -10);
                    //we activate the text so the new candidates can get a reference to them
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

            //move the camera down to see the resume
            case GameState.moveDown:
                turnLeftRightOff();
                time += Time.deltaTime / 2;
                //we move the camera down
                theCamera.transform.position = new Vector3(theCamera.transform.position.x, Mathf.SmoothStep(OriginalCameraPosition, DownCameraPosition, time), -10);
                //after 2 seconds
                if (getStateElapsed() > 2.0f)
                {
                    //we activate the hire and search buttons
                    //Debug.Log("Does this get accessed");
                    hire.SetActive(true);
                    keepSearching.SetActive(true);
                    //and we activate the resume text
                    resumeText.SetActive(true);
                }
                break;

            //if the player decides to keep searching, we move the camera back up
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
        //Debug.Log("Clicked button");
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
        //Debug.Log("is this getting called");
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
        //depending on the gender, we generate a male or female name
        if (gender)
        {
            //we get the first name
            fullName = F_Names_Array[Random.Range(0, F_Names_Array.Length)];
            //and then we add the last name
            fullName += " " + lastName_Array[Random.Range(0, lastName_Array.Length)];
        } else
        {
            fullName = M_Names_Array[Random.Range(0, M_Names_Array.Length)];
            fullName += " " + lastName_Array[Random.Range(0, lastName_Array.Length)];
        }
        //we return the name to the candidate object
        return fullName;
    }

    /// <summary>
    /// generated the past work experience of the candidate
    /// </summary>
    /// <param name="currentCandidate"></param>
    /// <returns></returns>
    public string genenerateExperience(Candidates currentCandidate)
    {
        string experience = "";
        //selects three random pieces of work experience
        for(int i = 0; i <3; i++)
        {
            string tempExperience = ExperienceArray[Random.Range(0, ExperienceArray.Length)];
            //Debug.Log(tempSkill);
            string[] tempExperienceArray = tempExperience.Split(',');
            //it adds the title to the string 
            experience += tempExperienceArray[0] + "\n";
            //and then adds the specific number value associated to that position to the percentile value that references the compability with the job
            currentCandidate.percentage += int.Parse(tempExperienceArray[1]);
        }

        return experience;
    }
    /// <summary>
    /// generates the skills the specific candidate has
    /// </summary>
    /// <param name="currentCandidate"></param>
    /// <param name="numberOfSkills"></param>
    /// <returns></returns>
    public string generateSkills( Candidates currentCandidate, int numberOfSkills)
    {
        string skill = "";
        //the same process is repeated as above safe for this time the number of skills is defined by the surrent number the candidates percentage is up to 
        for (int i = 0; i < numberOfSkills; i++)
        {
            string tempSkill = SkillsArray[Random.Range(0, SkillsArray.Length)];
            string[] tempSkillArray = tempSkill.Split(',');
            skill += tempSkillArray[0] + " ";
            currentCandidate.percentage += int.Parse(tempSkillArray[1]);
        }
        return skill;
    }
    /// <summary>
    /// generates the languages the candidate knows
    /// </summary>
    /// <param name="currentCandidate"></param>
    /// <param name="numberOfLanguages"></param>
    /// <returns></returns>
    public string generateLanguages(Candidates currentCandidate, int numberOfLanguages)
    {
        string languages = "";
        //same as the generateSkill function
        for (int i = 0; i < numberOfLanguages; i++)
        {
            string tempLanguage = LanguagesArray[Random.Range(0, LanguagesArray.Length)];
            string[] tempLanguageArray = tempLanguage.Split(',');
            languages += tempLanguageArray[0] + " ";
            currentCandidate.percentage += int.Parse(tempLanguageArray[1]);
        }
        return languages;
    }
    /// <summary>
    /// generate the university the candidate attended.
    /// </summary>
    /// <param name="currentCandidate"></param>
    /// <returns></returns>
    public string generateUniversity(Candidates currentCandidate)
    {
        string University = "";
        //we load in the name of the university the same way we did the previous ones
        //except here we only need one, so we don't need the for loop
        string tempUniversity = UniversitiesArray[Random.Range(0, UniversitiesArray.Length)];
        string[] tempUniversityArray = tempUniversity.Split(',');
        University += tempUniversityArray[0] + "\n";
        currentCandidate.percentage += int.Parse(tempUniversityArray[1]);
        
        return University;
    }
    /// <summary>
    /// function activated by the search button; will move the camera back up
    /// </summary>
    public void moveUp()
    {
        
        setCurrentState(GameState.moveUp);
        //disable all the buttons
        resumeText.SetActive(false);
        hire.SetActive(false);
        keepSearching.SetActive(false);
        time = 0;
    }
    /// <summary>
    /// function activated by the hire button; will proceed to restart the game
    /// </summary>
    public void Hire()
    {
        setCurrentState(GameState.fadeIn);
        resumeText.SetActive(false);
        hire.SetActive(false);
        keepSearching.SetActive(false);
        time = 0;
    }


}

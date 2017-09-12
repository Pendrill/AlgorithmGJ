using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Candidates : MonoBehaviour {
    //keeps a reference of the game manager
    public SelectCandidate GameManager;
    //keeps a reference of the candidate's full name
    public string fullName;
    //keeps a reference of the percentil compability the candidate has with the job
    public int percentage;
    //keeps a refrence of all the info found on the resume of the player
    public string Universities, Skills, Experience, Languages ="English " ;
    //has a reference to the respective UI elements
    public GameObject UI_Name, UI_Uni, UI_Skills, UI_Experience, UI_Languages, UI_Compability;
    //checks whether male or female 
    public bool gender;

	// Use this for initialization
	void Start () {
        //We find the GameManager and all the UI elements in the scene when the candidate is instanciated
        GameManager = FindObjectOfType<SelectCandidate>();
        UI_Name = GameObject.FindGameObjectWithTag("Name");
        UI_Uni = GameObject.FindGameObjectWithTag("Uni");
        UI_Skills = GameObject.FindGameObjectWithTag("Skills");
        UI_Experience = GameObject.FindGameObjectWithTag("Experience");
        UI_Languages = GameObject.FindGameObjectWithTag("Languages");
        UI_Compability = GameObject.FindGameObjectWithTag("Compability");

        //we randomly generate a name from the list of names profided in teh text files
        fullName = GameManager.generateName(gender);
        
        //We randomly generate the candidates experience the same way
        Experience = GameManager.genenerateExperience(this);
        //and we do the same for Universities
        Universities = GameManager.generateUniversity(this);

        //Based on the current compatibity, we generate a certain number of skills
        if (percentage > 20)
        {
            Skills = GameManager.generateSkills(this, 3);
        }
        else if (percentage > 10)
        {
            Skills = GameManager.generateSkills(this, 2);
        }
        else
        {
            Skills = GameManager.generateSkills(this, 1);
        }

        //we then do the same for the number of languages known by the candidate
        if (percentage > 40)
        {
            Languages += GameManager.generateLanguages(this, 3);
        }
        else if (percentage > 20)
        {
            Languages += GameManager.generateLanguages(this, 2);
        }
        else
        {
            Languages += GameManager.generateLanguages(this, 1);
        }
        

    }
    /// <summary>
    /// Once the resume is being inspected we plug in all the specific candidate information into the UI objects
    /// </summary>
    public void setText()
    {
        UI_Name.GetComponent<Text>().text = "Name: " + fullName;
        UI_Experience.GetComponent<Text>().text = "Experience: " + Experience;
        UI_Languages.GetComponent<Text>().text = "Languages: " + Languages;
        UI_Uni.GetComponent<Text>().text = "Education: " + Universities;
        UI_Skills.GetComponent<Text>().text = "Skills: " + Skills;
        UI_Compability.GetComponent<Text>().text = "This Candidate is " + percentage + "% compatible with the job";


    }

}




//Universities 10%
//Skills 30%
//Experience 30%
//Languages 30%
//

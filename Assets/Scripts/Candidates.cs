using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Candidates : MonoBehaviour {
    public SelectCandidate GameManager;
    public string fullName;
    public int percentage;
    public string Universities, Skills, Experience, Languages ="English " ;
    public GameObject UI_Name, UI_Uni, UI_Skills, UI_Experience, UI_Languages, UI_Compability;
	// Use this for initialization
	void Start () {
        GameManager = FindObjectOfType<SelectCandidate>();
        UI_Name = GameObject.FindGameObjectWithTag("Name");
        UI_Uni = GameObject.FindGameObjectWithTag("Uni");
        UI_Skills = GameObject.FindGameObjectWithTag("Skills");
        UI_Experience = GameObject.FindGameObjectWithTag("Experience");
        UI_Languages = GameObject.FindGameObjectWithTag("Languages");
        UI_Compability = GameObject.FindGameObjectWithTag("Compability");

        if (Random.Range(0, 100) > 50)
        {
            fullName = GameManager.generateName(true);
        }else
        {
            fullName = GameManager.generateName(false);
        }
        Experience = GameManager.genenerateExperience(this);
        Universities = GameManager.generateUniversity(this);

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
    public void setText()
    {
        UI_Name.GetComponent<Text>().text = "Name: " + fullName;
        UI_Experience.GetComponent<Text>().text = "Experience: " + Experience;
        UI_Languages.GetComponent<Text>().text = "Languages: " + Languages;
        UI_Uni.GetComponent<Text>().text = "University: " + Universities;
        UI_Skills.GetComponent<Text>().text = "Skills: " + Skills;
        UI_Compability.GetComponent<Text>().text = "This Candidate is " + percentage + "% compatible with the job";


    }

}




//Universities 10%
//Skills 30%
//Experience 30%
//Languages 30%
//

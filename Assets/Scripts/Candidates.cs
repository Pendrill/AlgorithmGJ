using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candidates : MonoBehaviour {
    public SelectCandidate GameManager;
    public string fullName;
	// Use this for initialization
	void Start () {
        GameManager = FindObjectOfType<SelectCandidate>();
        if (Random.Range(0, 100) > 50)
        {
            fullName = GameManager.generateName(true);
        }else
        {
            fullName = GameManager.generateName(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

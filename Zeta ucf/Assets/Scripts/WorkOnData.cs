using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkOnData : MonoBehaviour {

    public GameObject textMesh;
      
	// Use this for initialization
	void Start () {
        textMesh = GameObject.Find("TextMeshPro Text");


    }

    // Update is called once per frame
    void Update () {
        textMesh.GetComponent<TextMeshProUGUI>().text = "OK";

    }
}

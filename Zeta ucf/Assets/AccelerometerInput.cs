using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccelerometerInput : MonoBehaviour
{

    public GameObject textMesh;
    public GameObject toggle;

    // Use this for initialization
    void Start()
    {
        textMesh = GameObject.Find("TextMeshPro Text");
        toggle = GameObject.Find("Toggle");
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.GetComponent<Toggle>().isOn)
        {
            Vector3 acceleration = Input.acceleration;
            textMesh.GetComponent<TextMeshProUGUI>().text = "Acceleration is\n\n" +
                                                            "X = " + acceleration.x + "\n" +
                                                            "Y = " + acceleration.y + "\n" +
                                                            "Z = " + acceleration.z;
        }
    }
}

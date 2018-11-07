using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{

    public void changetext()
    {
        if (GameObject.Find("CollectButton").GetComponentInChildren<Text>().text.Equals("Start"))
        {
            GameObject.Find("CollectButton").GetComponentInChildren<Text>().text = "Stop";
        }
        else
        {
            GameObject.Find("CollectButton").GetComponentInChildren<Text>().text = "Finished";
            
        }
    }

    public void startAnalysis()
    {
        GameObject.Find("AnalyzeButton").GetComponentInChildren<Text>().text = "Analyzing";
    }

    public void changescene(string scenename)
    {
        SceneManager.LoadScene(scenename, LoadSceneMode.Single);

    }
}

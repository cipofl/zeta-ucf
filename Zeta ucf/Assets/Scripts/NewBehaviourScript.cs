using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{

    public GameObject panel, panel2;

    private string getForUserEndPoint = "http://localhost:5000/api/LimpingTests/GetForUser/";
    private string getByIdEndPoint = "http://localhost:5000/api/LimpingTests/GetById/";
    private string createEndPoint = "http://localhost:5000/api/LimpingTests/Create";
    private string editEndPoint = "http://localhost:5000/api/LimpingTests/Edit/";
    private string deleteEndPoint = "http://localhost:5000/api/LimpingTests/Delete/";

    // Use this for initialization
    void Start()
    {
        panel.SetActive(true);
        panel2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Reload
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    // Quit
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadTakeTestScene()
    {
        SceneManager.LoadScene("Take Test Scene");
    }


    // API Methods

    // Get limping tests for user
    public IEnumerator GetForUser()
    {
        //get user id from input field
        string userId = GameObject.Find("InputField userId").GetComponent<InputField>().text;

        print("GetForUser()" + " " + userId);

        UnityWebRequest www = UnityWebRequest.Get(getForUserEndPoint + userId);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        //here deserialize and populate fields
    }

    // Get a limping test by given limping test id
    public IEnumerator GetById()
    {
        string limpingTestId = GameObject.Find("InputField limpingTestId").GetComponent<InputField>().text;

        print("GetById()" + " " + limpingTestId);

        UnityWebRequest www = UnityWebRequest.Get(getByIdEndPoint + limpingTestId);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        //here deserialize and populate fields
    }

    // Create a limping test
    public IEnumerator Create()
    {
        string appUserId = GameObject.Find("InputField userId").GetComponent<InputField>().text;
        string testData = GameObject.Find("InputField testData").GetComponent<InputField>().text;
        string testAnalysisEndValue = GameObject.Find("InputField testData").GetComponent<InputField>().text;
        string testAnalysisDescription = GameObject.Find("InputField testAnalysisDescription").GetComponent<InputField>().text;
        string testAnalysisLimpingSeverity = GameObject.Find("InputField testAnalysisLimpingSeverity").GetComponent<InputField>().text;

        print("Create()");

        //here serialize
        string postData = "";

        UnityWebRequest www = UnityWebRequest.Post(createEndPoint, postData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    // Edit a limping test by given limping test id
    public IEnumerator Edit()
    {
        string limpingTestId = GameObject.Find("InputField limpingTestId").GetComponent<InputField>().text;
        string testData = GameObject.Find("InputField testData").GetComponent<InputField>().text;
        string testAnalysisEndValue = GameObject.Find("InputField testData").GetComponent<InputField>().text;
        string testAnalysisDescription = GameObject.Find("InputField testAnalysisDescription").GetComponent<InputField>().text;
        string testAnalysisLimpingSeverity = GameObject.Find("InputField testAnalysisLimpingSeverity").GetComponent<InputField>().text;

        print("Edit()" + " " + limpingTestId);

        //here serialize
        string bodyData = "";

        UnityWebRequest www = UnityWebRequest.Put(editEndPoint, bodyData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    // Delete a limping test by given limping test id
    public IEnumerator Delete()
    {
        string limpingTestId = GameObject.Find("InputField limpingTestId").GetComponent<InputField>().text;

        print("Delete()" + " " + limpingTestId);

        UnityWebRequest www = UnityWebRequest.Delete(deleteEndPoint + limpingTestId);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}

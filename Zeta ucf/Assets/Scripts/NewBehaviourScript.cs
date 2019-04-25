using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Limping.Api.Models;

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
            yield break;
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        //here deserialize and populate fields
        LimpingTest limpingTest = JsonUtility.FromJson<LimpingTest>(www.downloadHandler.text);
        GameObject.Find("InputField userId").GetComponent<InputField>().text = limpingTest.AppUserId.ToString();
        GameObject.Find("InputField limpingTestId").GetComponent<InputField>().text = limpingTest.Id.ToString();
        GameObject.Find("InputField date").GetComponent<InputField>().text = limpingTest.Date.ToString();
        GameObject.Find("InputField testData").GetComponent<InputField>().text = limpingTest.TestData;
        GameObject.Find("InputField testAnalysisEndValue").GetComponent<InputField>().text = limpingTest.TestAnalysis.EndValue.ToString();
        GameObject.Find("InputField testDescription").GetComponent<InputField>().text = limpingTest.TestAnalysis.Description;
        GameObject.Find("Dropdown testAnalysisLimpingSeverity").GetComponent<Dropdown>().value = (int)limpingTest.TestAnalysis.LimpingSeverity;
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
            yield break;
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        //here deserialize and populate fields
        LimpingTest limpingTest = JsonUtility.FromJson<LimpingTest>(www.downloadHandler.text);

        GameObject.Find("InputField userId").GetComponent<InputField>().text = limpingTest.AppUserId.ToString();
        GameObject.Find("InputField limpingTestId").GetComponent<InputField>().text = limpingTest.Id.ToString();
        GameObject.Find("InputField date").GetComponent<InputField>().text = limpingTest.Date.ToString();
        GameObject.Find("InputField testData").GetComponent<InputField>().text = limpingTest.TestData;
        GameObject.Find("InputField testAnalysisEndValue").GetComponent<InputField>().text = limpingTest.TestAnalysis.EndValue.ToString();
        GameObject.Find("InputField testDescription").GetComponent<InputField>().text = limpingTest.TestAnalysis.Description;
        GameObject.Find("Dropdown testAnalysisLimpingSeverity").GetComponent<Dropdown>().value = (int)limpingTest.TestAnalysis.LimpingSeverity;
    }

    // Create a limping test
    public IEnumerator Create()
    {
        string appUserId = GameObject.Find("InputField userId").GetComponent<InputField>().text;
        string testData = GameObject.Find("InputField testData").GetComponent<InputField>().text;
        string testAnalysisEndValue = GameObject.Find("InputField testAnalysisEndValue").GetComponent<InputField>().text;
        string testAnalysisDescription = GameObject.Find("InputField testAnalysisDescription").GetComponent<InputField>().text;
        int testAnalysisLimpingSeverity = GameObject.Find("Dropdown testAnalysisLimpingSeverity").GetComponent<Dropdown>().value;

        print("Create()");

        //here serialize
        LimpingTest limpingTest = new LimpingTest
        {
            AppUserId = appUserId,
            TestData = testData
        };
        limpingTest.TestAnalysis = new TestAnalysis
        {
            EndValue = double.Parse(testAnalysisEndValue),
            Description = testAnalysisDescription,
            LimpingSeverity = (LimpingSeverityEnum)testAnalysisLimpingSeverity
        };

        string postData = JsonUtility.ToJson(limpingTest);

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
        string testAnalysisEndValue = GameObject.Find("InputField testAnalysisEndValue").GetComponent<InputField>().text;
        string testAnalysisDescription = GameObject.Find("InputField testAnalysisDescription").GetComponent<InputField>().text;
        int testAnalysisLimpingSeverity = GameObject.Find("Dropdown testAnalysisLimpingSeverity").GetComponent<Dropdown>().value;

        print("Edit()" + " " + limpingTestId);

        //here serialize
        LimpingTest limpingTest = new LimpingTest
        {
            TestData = testData
        };
        limpingTest.TestAnalysis = new TestAnalysis
        {
            EndValue = double.Parse(testAnalysisEndValue),
            Description = testAnalysisDescription,
            LimpingSeverity = (LimpingSeverityEnum)testAnalysisLimpingSeverity
        };

        string bodyData = JsonUtility.ToJson(limpingTest);

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

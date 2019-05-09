using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Limping.Api.Models;
using Newtonsoft.Json;
using System.Text;

public class NewBehaviourScript1 : MonoBehaviour
{
    private readonly string host = "http://192.168.99.100:8090";
    private string usersCreateHref;
    private string limpingTestsSingleHref;
    private string limpingTestsCreate;
    private User user;
    private MyTestAnalysis testAnalysis;
    private MyLimpingTest limpingTest;
    private string appUserid;
    private string limpingTestsEditHref;
    private string limpingTestsDeleteHref;
    private string limpingTestid;

    private void Start()
    {
        StartCoroutine(GetRoot());
    }

    //Gets all the links you can navigate to from the root
    public IEnumerator GetRoot()
    {
        UnityWebRequest www = UnityWebRequest.Get(host + "/api/root");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield break;
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        dynamic dynamic = JsonConvert.DeserializeObject(www.downloadHandler.text);
        print(dynamic);
        usersCreateHref = host + dynamic["_links"]["users-create"]["href"];
        print(usersCreateHref);
    }

    //Create a user and then return the dto
    public IEnumerator CreateUser()
    {
        string userName = GameObject.Find("user name input field").GetComponent<InputField>().text;
        string email = GameObject.Find("email input field").GetComponent<InputField>().text;

        user = new User
        {
            userName = userName,
            email = email
        };

        string postData = JsonUtility.ToJson(user);

        var request = new UnityWebRequest(usersCreateHref, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(postData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Status Code: " + request.responseCode);

        dynamic dynamic = JsonConvert.DeserializeObject(request.downloadHandler.text);
        print(dynamic);

        if (request.responseCode == 200)
        {
            GameObject.Find("Panel api").SetActive(false);
            GameObject.Find("Panel").transform.Find("Panel api (1)").gameObject.SetActive(true);
            limpingTestsSingleHref = host + dynamic["_links"]["limpingTests-single"]["href"];
            print(limpingTestsSingleHref);

            GameObject.Find("Panel api (1)/id").GetComponent<Text>().text = "Id: " + dynamic["id"].ToString();
            appUserid = dynamic["id"].ToString();
            GameObject.Find("Panel api (1)/username").GetComponent<Text>().text = "Username: " + dynamic["userName"].ToString();
            GameObject.Find("Panel api (1)/email").GetComponent<Text>().text = "Email: " + dynamic["email"].ToString();

        }
    }

    //Gets all the limping tests for a user
    public IEnumerator GetLimpingTestsForUser()
    {
        UnityWebRequest www = UnityWebRequest.Get(limpingTestsSingleHref);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield break;
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        dynamic dynamic = JsonConvert.DeserializeObject(www.downloadHandler.text);
        print(dynamic);
        limpingTestsCreate = host + dynamic["_links"]["limpingTests-create"]["href"];
        print(limpingTestsCreate);
    }

    //Create a limping test
    public IEnumerator CreateLimpingTest()
    {
        yield return StartCoroutine(GetLimpingTestsForUser());

        string testData = GameObject.Find("testData").GetComponent<InputField>().text;
        int endValue = int.Parse(GameObject.Find("endValue").GetComponent<InputField>().text);
        string description = GameObject.Find("description").GetComponent<InputField>().text;
        int limpingSeverity = int.Parse(GameObject.Find("limpingSeverity").GetComponent<InputField>().text);

        testAnalysis = new MyTestAnalysis
        {
            endValue = endValue,
            description = description,
            limpingSeverity = limpingSeverity
        };

        limpingTest = new MyLimpingTest
        {
            appUserId = appUserid,
            testData = testData,
            testAnalysis = testAnalysis
        };

        string postData = JsonUtility.ToJson(limpingTest);

        var request = new UnityWebRequest(limpingTestsCreate, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(postData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Status Code: " + request.responseCode);

        dynamic dynamic = JsonConvert.DeserializeObject(request.downloadHandler.text);
        print(dynamic);

        if (request.responseCode == 200)
        {
            GameObject.Find("Panel api (2)").SetActive(false);
            GameObject.Find("Panel").transform.Find("Panel api (3)").gameObject.SetActive(true);
            limpingTestsEditHref = host + dynamic["_links"]["limpingTests-edit"]["href"];
            limpingTestsDeleteHref = host + dynamic["_links"]["limpingTests-delete"]["href"];
            print(limpingTestsEditHref);

            GameObject.Find("Panel api (3)/id").GetComponent<Text>().text = "Id: " + dynamic["id"].ToString();
            limpingTestid = dynamic["id"].ToString();
            GameObject.Find("Panel api (3)/date").GetComponent<Text>().text = "Date: " + dynamic["date"].ToString();
            GameObject.Find("Panel api (3)/testdata").GetComponent<Text>().text = "Test data: " + dynamic["testData"].ToString();
            GameObject.Find("Panel api (3)/appuserid").GetComponent<Text>().text = "App user id: " + dynamic["appUserId"].ToString();
        }
    }

    //Deletes the limping test
    public IEnumerator DeleteLimpingTest()
    {
        UnityWebRequest www = UnityWebRequest.Delete(limpingTestsDeleteHref);
        yield return www.SendWebRequest();

        Debug.Log("Status Code: " + www.responseCode);

        if (www.responseCode == 200)
        {
            GameObject.Find("Panel api (3)").SetActive(false);
            GameObject.Find("Panel").transform.Find("Panel api (4)").gameObject.SetActive(true);
        }
    }

    //Edits the limping test
    public IEnumerator EditLimpingTest()
    {
        string testData = GameObject.Find("testData").GetComponent<InputField>().text;
        int endValue = int.Parse(GameObject.Find("endValue").GetComponent<InputField>().text);
        string description = GameObject.Find("description").GetComponent<InputField>().text;
        int limpingSeverity = int.Parse(GameObject.Find("limpingSeverity").GetComponent<InputField>().text);

        MyTestAnalysis testAnalysis = new MyTestAnalysis
        {
            endValue = endValue,
            description = description,
            limpingSeverity = limpingSeverity
        };

        MyLimpingTest2 limpingTest = new MyLimpingTest2
        {
            testData = testData,
            testAnalysis = testAnalysis
        };

        string postData = JsonUtility.ToJson(limpingTest);

        var request = new UnityWebRequest(limpingTestsEditHref, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(postData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Status Code: " + request.responseCode);

        dynamic dynamic = JsonConvert.DeserializeObject(request.downloadHandler.text);
        print(dynamic);

        if (request.responseCode == 200)
        {
            GameObject.Find("Panel api (5)").SetActive(false);
            GameObject.Find("Panel").transform.Find("Panel api (6)").gameObject.SetActive(true);
        }
    }
}

[System.Serializable]
public class User
{
    [SerializeField]
    public string userName;
    [SerializeField]
    public string email;
}

[System.Serializable]
public class MyLimpingTest
{
    [SerializeField]
    public string appUserId;
    [SerializeField]
    public string testData;
    [SerializeField]
    public MyTestAnalysis testAnalysis;
}

[System.Serializable]
public class MyTestAnalysis
{
    [SerializeField]
    public int endValue;
    [SerializeField]
    public string description;
    [SerializeField]
    public int limpingSeverity;
}

[System.Serializable]
public class MyLimpingTest2
{
    [SerializeField]
    public string testData;
    [SerializeField]
    public MyTestAnalysis testAnalysis;
}

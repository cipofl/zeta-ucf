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

        User user = new User
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
        print(JsonConvert.DeserializeObject(request.downloadHandler.text));

        if (request.responseCode == 200)
        {
            GameObject.Find("Panel api").SetActive(false);
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

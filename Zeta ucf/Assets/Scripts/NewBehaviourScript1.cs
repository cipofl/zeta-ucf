using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Limping.Api.Models;
using Newtonsoft.Json;

public class NewBehaviourScript1 : MonoBehaviour
{
    private readonly string host = "http://192.168.99.100:8090";
    private string usersCreateHref;

    private void Start()
    {
        StartCoroutine(GetRoot());
    }

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
}

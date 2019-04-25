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
    public void GetForUser()
    {
        GameObject inputField = GameObject.Find("InputField userId");
        string userId = inputField.GetComponent<InputField>().text;
        print("GetForUser()" + " " + userId);

        StartCoroutine(GetText(getForUserEndPoint + userId));
    }

    public void GetById()
    {
        GameObject inputField = GameObject.Find("InputField limpingTestId");
        string limpingTestId = inputField.GetComponent<InputField>().text;
        print("GetById()" + " " + limpingTestId);

        StartCoroutine(GetText(getByIdEndPoint + limpingTestId));
    }

    public void Create()
    {
        print("Create()");
    }

    public void Edit()
    {
        print("Edit()");
    }

    public void Delete()
    {
        print("Delete()");
    }

    IEnumerator GetText(string resource)
    {
        UnityWebRequest www = UnityWebRequest.Get(resource);
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

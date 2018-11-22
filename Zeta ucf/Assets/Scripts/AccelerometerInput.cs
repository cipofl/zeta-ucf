using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class AccelerometerInput : MonoBehaviour
{

    public TextMeshProUGUI textMesh;

    public GameObject collect_button;
    public GameObject analyze_button;

    public GameObject panelCollect;
    public GameObject panelAnalyze;
    public GameObject imageAcc;
    public GameObject content;

    public static List<Vector3> raw_data;
    public double[] total_Acc;
    public double[] y_fft;

    public bool collect;

    public float threshold;

    // Use this for initialization
    void Start()
    {

        textMesh = GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();

        collect_button = GameObject.Find("CollectButton");
        analyze_button = GameObject.Find("AnalyzeButton");
        analyze_button.GetComponent<Button>().interactable = false;

        panelCollect = GameObject.Find("Panel Collect");
        panelAnalyze = GameObject.Find("Panel Analyze");
        imageAcc = GameObject.Find("Image Acc");
        content = GameObject.Find("Content");
        panelAnalyze.SetActive(false);

        raw_data = new List<Vector3>();

    }

    void FixedUpdate()
    {

        if (collect)
        {
            Vector3 acceleration = Input.acceleration;
            raw_data.Add(acceleration);
            textMesh.text = "Acceleration is\n\n" +
                            "X = " + acceleration.x.ToString("F10") + "\n" +
                            "Y = " + acceleration.y.ToString("F10") + "\n" +
                            "Z = " + acceleration.z.ToString("F10");
        }

    }

    public void AnalyzeData()
    {

        print("AnalyzeData");
        total_Acc = new double[raw_data.Count];
        textMesh.text = "Calculating total acceleration on " + raw_data.Count.ToString() + " values";
        for (int i = 0; i < raw_data.Count; i++)
        {
            //calculate total acceleration of the 3 axis
            total_Acc[i] = Mathf.Sqrt(Mathf.Pow((raw_data.ElementAt(i).x), 2) + Mathf.Pow((raw_data.ElementAt(i).y), 2) + Mathf.Pow((raw_data.ElementAt(i).z), 2));
            //print("\t" + total_Acc[i]);
        }

        FFT2 fft2 = new FFT2();
        /**
        * Initialize class to perform FFT of specified size.
        *
        * @param   logN    Log2 of FFT length. e.g. for 512 pt FFT, logN = 9.
        */
        textMesh.text = "starting FFT";
        fft2.init((uint)Mathf.Log(total_Acc.Length));
        //create array of double for Im part-----> array should be compsed by 0

        y_fft = new double[total_Acc.Length];
        for (int i = 0; i < total_Acc.Length; i++)
        {
            y_fft[i] = 0;
        }

        //run the fft
        fft2.run(total_Acc, y_fft);

        StartCoroutine(ShowResult());

    }

    // Collect
    public void Collect()
    {
        print("AccelerometerInput.Collect()");
        if (!collect)
        {
            print("\tStart");
            collect_button.GetComponentInChildren<Text>().text = "Stop";
            collect = true;
        }
        else if (collect)
        {
            print("\tStop");
            //collect_button.GetComponentInChildren<Text>().text = "Finished";
            collect_button.GetComponent<Button>().interactable = false;
            analyze_button.GetComponent<Button>().interactable = true;
            collect = false;
        }
    }

    // Analyze
    public void Analyze()
    {
        print("AccelerometerInput.Analyze()");
        collect = false;
        //analyze_button.GetComponentInChildren<Text>().text = "Analyzing";
        analyze_button.GetComponent<Button>().interactable = false;
        AnalyzeData();
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

    // Show result
    public IEnumerator ShowResult()
    {

        print("ShowResult");
        panelCollect.SetActive(false);
        panelAnalyze.SetActive(true);

        foreach (double acc in total_Acc)
        {
            Instantiate(imageAcc, imageAcc.transform.parent);
        }
        yield return null;

        content.GetComponent<ContentSizeFitter>().enabled = false;
        yield return null;
        content.GetComponent<HorizontalLayoutGroup>().enabled = false;
        yield return null;

        for (int i = 0; i < total_Acc.Length; i++)
        {
            Transform child = content.transform.GetChild(i);
            child.localPosition = new Vector3(child.localPosition.x, (float)total_Acc[i] * 50, 0);

            if (total_Acc[i] > threshold)
            {
                child.GetComponent<Image>().color = Color.red;
            }
        }

        GameObject last = null;
        Transform[] transforms = content.GetComponentsInChildren<Transform>();
        for (int i = 1; i < transforms.Length; i++)
        {
            if (last)
            {
                CreateConnection(last.GetComponent<RectTransform>().anchoredPosition, transforms[i].GetComponent<RectTransform>().anchoredPosition);
            }
            last = transforms[i].gameObject;
        }

    }

    private void CreateConnection(Vector2 positionA, Vector2 positionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(content.transform, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (positionB - positionA).normalized;
        float distance = Vector2.Distance(positionA, positionB);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = positionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}

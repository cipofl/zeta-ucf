using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class AccelerometerInput : MonoBehaviour
{

    public GameObject textMesh;
    public GameObject collect_button;
    public GameObject analyze_button;
    public static List<Vector3> raw_data;
    public double[] total_Acc;
    public double[] y_fft;

    public bool collect;

    public GameObject panelCollect;
    public GameObject panelAnalyze;
    public GameObject imageAcc;
    public GameObject content;

    public float threshold;

    // Use this for initialization
    void Start()
    {
        textMesh = GameObject.Find("TextMeshPro Text");

        collect_button = GameObject.Find("CollectButton");
        analyze_button = GameObject.Find("AnalyzeButton");
        analyze_button.GetComponent<Button>().interactable = false;
        raw_data = new List<Vector3>();

        panelCollect = GameObject.Find("Panel Collect");
        panelAnalyze = GameObject.Find("Panel Analyze");
        imageAcc = GameObject.Find("Image Acc");
        content = GameObject.Find("Content");

        panelAnalyze.SetActive(false);
    }

    void FixedUpdate()
    {
        if (collect)
        {
            Vector3 acceleration = Input.acceleration;
            raw_data.Add(acceleration);
            textMesh.GetComponent<TextMeshProUGUI>().text = "Acceleration is\n\n" +
                                                            "X = " + acceleration.x.ToString("F10") + "\n" +
                                                            "Y = " + acceleration.y.ToString("F10") + "\n" +
                                                            "Z = " + acceleration.z.ToString("F10");
        }
    }

    public void AnalyzeData()
    {

        print("AnalyzeData");
        total_Acc = new double[raw_data.Count];
        textMesh.GetComponent<TextMeshProUGUI>().text = "Calculating total acceleration on " + raw_data.Count.ToString() + " values";
        for (int i = 0; i < raw_data.Count; i++)
        {
            //calculate total acceleration of the 3 axis
            total_Acc[i] = Mathf.Sqrt(Mathf.Pow((raw_data.ElementAt(i).x), 2) + Mathf.Pow((raw_data.ElementAt(i).y), 2) + Mathf.Pow((raw_data.ElementAt(i).z), 2));
            print("\t" + total_Acc[i]);
        }

        FFT2 fft2 = new FFT2();
        /**
        * Initialize class to perform FFT of specified size.
        *
        * @param   logN    Log2 of FFT length. e.g. for 512 pt FFT, logN = 9.
        */
        textMesh.GetComponent<TextMeshProUGUI>().text = "starting FFT";
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

    public IEnumerator ShowResult()
    {
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
            child.localPosition = new Vector3(child.localPosition.x, (float)total_Acc[i] * 20, 0);

            if (total_Acc[i] > threshold)
            {
                child.GetComponent<Image>().color = Color.red;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class AccelerometerInput : MonoBehaviour
{

    public GameObject textMesh;
    public GameObject toggle;
    public GameObject collect_button;
    public GameObject analyze_button;
    public static List<Vector3> raw_data;
    public double[] total_Acc;
    public double[] y_fft;

    // Use this for initialization
    void Start()
    {
        textMesh = GameObject.Find("TextMeshPro Text");

        collect_button = GameObject.Find("CollectButton");
        analyze_button = GameObject.Find("AnalyzeButton");
        analyze_button.GetComponent<Button>().enabled = false;
        raw_data = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collect_button.GetComponent<Button>().GetComponentInChildren<Text>().text.Equals("Stop"))
        {
            Vector3 acceleration = Input.acceleration;
            raw_data.Add(acceleration);
            textMesh.GetComponent<TextMeshProUGUI>().text = "Acceleration is\n\n" +
                                                            "X = " + acceleration.x.ToString("F10") + "\n" +
                                                            "Y = " + acceleration.y.ToString("F10") + "\n" +
                                                            "Z = " + acceleration.z.ToString("F10");
        }
        if (collect_button.GetComponent<Button>().GetComponentInChildren<Text>().text.Equals("Finished"))
        {
            collect_button.GetComponent<Button>().enabled = false;
            analyze_button.GetComponent<Button>().enabled = true;
        }

        if (analyze_button.GetComponent<Button>().GetComponentInChildren<Text>().text.Equals("Analyzing"))
        {
            analyze_button.GetComponent<Button>().enabled = false;
            AnalyzeData();
        }


    }

    public void AnalyzeData()
    {

        total_Acc = new double[raw_data.Count];
        textMesh.GetComponent<TextMeshProUGUI>().text = "Calculating total acceleration on " + raw_data.Count.ToString() + " values";
        for (int i = 0; i < raw_data.Count; i++)
        {
            //calculate total acceleration of the 3 axis
            total_Acc[i] = Mathf.Sqrt(Mathf.Pow((raw_data.ElementAt(i).x), 2) + Mathf.Pow((raw_data.ElementAt(i).y), 2) + Mathf.Pow((raw_data.ElementAt(i).z), 2));

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


        


    }


}


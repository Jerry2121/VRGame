using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScr : MonoBehaviour
{
    public float Timer = 10f;
    public TextMeshProUGUI GameClosingText;
    // Start is called before the first frame update
    void Start()
    {
        GameClosingText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        double b;
        b = System.Math.Round(Timer, 0);
        GameClosingText.text = "Gaming Closing in " + b + " Seconds...";
        if (Timer <= 0)
        {
            Application.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostOptionsCanvasComponent : MonoBehaviour
{
    public TextMeshProUGUI WorldNameText;
    public TextMeshProUGUI DifficultyText;
    public TextMeshProUGUI AllowHintsText;
    public TextMeshProUGUI AllowSavingText;
    public TextMeshProUGUI ThirtyMinuteTimerText;
    public TextMeshProUGUI TimeElapsedTimerText;

    // Start is called before the first frame update
    void Start()
    {
        WorldNameText.text = "World Name: " + PlayerPrefs.GetString("WorldName");

        //Difficulty is Easy
        if (PlayerPrefs.GetInt("Difficulty") == 0)
        {
            DifficultyText.text = "Difficulty: Easy";
        }
        //Difficulty is Normal
        else if (PlayerPrefs.GetInt("Difficulty") == 1)
        {
            DifficultyText.text = "Difficulty: Normal";
        }
        //Difficulty is Hard
        else if(PlayerPrefs.GetInt("Difficulty") == 2)
        {
            DifficultyText.text = "Difficulty: Hard";
        }

        //Hints are disabled
        if (PlayerPrefs.GetInt("AllowHints") == 0)
        {
            AllowHintsText.text = "Allow Hints: False";
        }
        //Hints are enabled
        else
        {
            AllowHintsText.text = "Allow Hints: True";
        }

        //Saving are disabled
        if (PlayerPrefs.GetInt("AllowSaving") == 0)
        {
            AllowSavingText.text = "Allow Saving: False";
        }
        //Saving are enabled
        else
        {
            AllowSavingText.text = "Allow Saving: True";
        }

        //30m Timer are disabled
        if (PlayerPrefs.GetInt("ThirtyMinuteTimer") == 0)
        {
            ThirtyMinuteTimerText.text = "30m Timer: False";
        }
        //30m Timer are enabled
        else
        {
            ThirtyMinuteTimerText.text = "30m Timer: True";
        }

        //Time Elapsed Timer are disabled
        if (PlayerPrefs.GetInt("TimeElapsed") == 0)
        {
            TimeElapsedTimerText.text = "Time Elapsed Timer: False";
        }
        //Time Elapsed Timer are enabled
        else
        {
            TimeElapsedTimerText.text = "Time Elapsed Timer: True";
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

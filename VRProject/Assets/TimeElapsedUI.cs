using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeElapsedUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("TimeElapsedTimer") + "s";
    }
}

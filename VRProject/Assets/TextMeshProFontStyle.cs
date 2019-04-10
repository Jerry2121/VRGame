using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMeshProFontStyle : MonoBehaviour
{
    private bool CursorHovering;
    // Start is called before the first frame update
    void Start()
    {
       // this.gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }

    // Update is called once per frame
    void Update()
    {
     if (CursorHovering)
        {

        }   
     else
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
    }
    public void UnderlineText()
    {
        CursorHovering = true;
        this.gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold | FontStyles.Underline;
        //this.gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
    }
    public void NotUnderlineText()
    {
        CursorHovering = false;
        this.gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }
    public void ClickColor()
    {
        CursorHovering = false;
        this.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32 (101, 0, 0, 255);
    }
    public void UnClickColor()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(161, 27, 27, 255);
    }
}

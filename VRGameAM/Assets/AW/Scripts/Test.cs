using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*Logger.Instance.Log("Test", LogType.Warning, true);
        Logger.Instance.Log("Test01", LogType.Error, false);
        Logger.Instance.Log("Test02", LogType.Log, true);
        */
        Logger.Instance.Init();

        Debug.Log("Log");
        Debug.LogWarning("Warning");
        Debug.LogError("Error");


        //Debug.Log(Logger.Instance.DisplayLoggedText());
    }

    // Update is called once per frame
    void Update()
    {

    }
}

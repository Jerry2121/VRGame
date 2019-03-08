using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using UnityEngine;

//public enum LogType { Normal, Warning, Error }

public sealed class Logger
{
    #region singleton
    private static readonly Lazy<Logger> lazy =
        new Lazy<Logger>(() => new Logger());

    public static Logger Instance { get { return lazy.Value; } }

    private Logger()
    {
    }
    #endregion

    bool debugToConsole;

    public void Init()
    {
        Application.logMessageReceivedThreaded += HandleLogThreaded;
        Application.quitting += WriteApplicationLog;
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR && DEVELOPMENT_BUILD
        DebugConsole.Instance.WriteLine("Checking Console");
        debugToConsole = true;
#endif
    }

    int messageDisplayLimit = 10;
    public int MessageDisplayLimit
    {
        set
        {
            if (value > 0)
                messageDisplayLimit = value;
        }
    }

    void HandleLogThreaded(string _message, string _stackTrace, LogType _type)
    {
        Log(_message, _type);
    }

    List<string> logHistory = new List<string>();

    public void Log(string _message, LogType logType = LogType.Log, bool debugLog = false)
    {
        string logTypeMessage = string.Empty;
        if (logType == LogType.Warning)
            logTypeMessage = "Warning - ";
        else if (logType == LogType.Error)
            logTypeMessage = "ERROR - ";


        logHistory.Add(string.Format("{0}[{1}] {2}", logTypeMessage, DateTime.Now, _message));
        if (debugLog && Debug.isDebugBuild)
        {
            Debug.Log(_message);
        }

        if(debugToConsole)
            DebugConsole.Instance.WriteLine(string.Format("{0}[{1}]: {2}", logTypeMessage, DateTime.Now, _message));
    }

    public string DisplayLoggedText()
    {
        List<string> logAsTextList = logHistory;

        int num = logAsTextList.Count;
        for (int i = 0; i < num - messageDisplayLimit; i++)
        {
            logAsTextList.Remove(logAsTextList[i]);
        }

        string logAsText = string.Join("\n", logAsTextList.ToArray());

        return logAsText;
    }

    void WriteApplicationLog()
    {
        string path = Application.persistentDataPath + @"/log" + ".txt";

        Debug.Log("Writing Log to text file at \n" + path);

        List<string> loggedText = logHistory;
        loggedText.Insert(0, string.Format("Log written at {0}, UTC time {1}", DateTime.Now, DateTime.UtcNow));

        string[] loggedTextArray = loggedText.ToArray();

        //string[] createText = (loggedTextArray);
        try
        {
            File.WriteAllLines(path, loggedTextArray);
        }
        catch (Exception ex)
        {
            Log(ex.Message, LogType.Error, true);
        }

    }

}

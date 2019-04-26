using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class DebugConsole
{
    [DllImport("kernel32")]
    static extern int AllocConsole();
    [DllImport("kernel32")]
    static extern int FreeConsole();
    [DllImport("kernel32")]
    static extern bool AttachConsole(uint dwProcessId);
    [DllImport("Kernel32.dll")]
    private static extern bool SetConsoleTitle(string title);
    [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    private const int ATTACH_PARENT_PROCESS = -1;
    StreamWriter m_StdOutWriter;
    private static TextWriter s_PreviousOutput;
    private static bool s_Initialized = false;

    #region singleton
    private static readonly Lazy<DebugConsole> s_Lazy =
        new Lazy<DebugConsole>(() => new DebugConsole());

    public static DebugConsole s_Instance { get { return s_Lazy.Value; } }

    private DebugConsole()
    {
        Init();
    }
    #endregion

    public void Init()
    {
        try
        {
            if (!AttachConsole(0xffffffff))
            {
                AllocConsole();
            }
            s_PreviousOutput = System.Console.Out;
            SetConsoleTitle("VRGame Debug");

            IntPtr stdHandle = GetStdHandle(-11);
            Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            System.Text.Encoding encoding = System.Text.Encoding.ASCII;
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            s_Initialized = true;
            Console.WriteLine("Console Started");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void WriteLine(string line)
    {
        if (s_Initialized)
        {
            try
            {
                //Instance._stdOutWriter.WriteLine(line);
                Console.WriteLine(line);
            }
            catch (NullReferenceException) { }
        }
    }

    public void CloseConsole()
    {
        Console.SetOut(s_PreviousOutput);
        FreeConsole();
    }

#if UNITY_EDITOR
    [MenuItem("VRGame/Debug/FreeConsole")]
    public static void EditorCloseConsole()
    {
        try
        {
            Console.SetOut(s_PreviousOutput);
            FreeConsole();
        }
        catch { }
    }
#endif

}

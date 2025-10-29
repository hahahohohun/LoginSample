using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DebugGUI : MonoBehaviour
{
    private static readonly Queue<string> logs = new Queue<string>();
    private const int maxLogs = 15; // 화면에 보일 최대 줄 수
    private Vector2 scrollPos;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        logs.Clear();
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logs.Enqueue(logString);
        while (logs.Count > maxLogs)
            logs.Dequeue();
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 10;
        style.normal.textColor = Color.green;
        style.wordWrap = true;

        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height / 3));
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        foreach (string log in logs)
        {
            GUILayout.Label(log, style);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void OnDestroy()
    {
        OnDisable();
    }
}
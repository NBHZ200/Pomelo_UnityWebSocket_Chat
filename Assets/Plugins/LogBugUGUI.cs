using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;

/// <summary>
/// 宣雨松写的甩出错误的脚本，可用来在运行时把错误显示到屏幕上
/// </summary>
public class LogBugUGUI : MonoBehaviour {

    public RectTransform contentDrag;
    public Text textDebug;

	static List<string> mLines = new List<string>();
	static List<string> mWriteTxt = new List<string>();
	private string outpath;
	void Start () {
		//Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
		outpath = Application.persistentDataPath + "/outLog.txt";
		//每次启动客户端删除之前保存的Log
		if (System.IO.File.Exists (outpath)) {
			File.Delete (outpath);
		}
		//监听Log
		Application.logMessageReceived += HandleLog;
	}
 
	void Update () 
	{
		//写入文件的操作必须在主线程中完成，因此在Update中写入文件。
		if(mWriteTxt.Count > 0)
		{
			string[] temp = mWriteTxt.ToArray();
			foreach(string t in temp)
			{
				using(StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
				{
					writer.WriteLine(t);
				}
				mWriteTxt.Remove(t);
			}
		}

        if (lineCount != mLines.Count)
        {
            textDebug.text = "";
            for (int i = 0, imax = mLines.Count; i < imax; ++i)
            {
                textDebug.text += mLines[i] + "\n";
            }
            contentDrag.sizeDelta = new Vector2(contentDrag.sizeDelta.x, textDebug.preferredHeight);
        }
        lineCount = mLines.Count;
    }
    int lineCount = 0;
	void HandleLog(string logString, string stackTrace, LogType type)
	{
		mWriteTxt.Add(logString);
		if (type == LogType.Error || type == LogType.Exception) 
		{
			Log(logString);
			Log(stackTrace);
		}
	}

    //保存错误信息，输出到手机屏幕上
    static public void Log (params object[] objs)
	{
		string text = "";
		for (int i = 0; i < objs.Length; ++i)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text += ", " + objs[i].ToString();
			}
		}
		if (Application.isPlaying)
		{
			/*if (mLines.Count > 20) 
			{
				mLines.RemoveAt(0);
			}*/
			mLines.Add(text);
 
		}
	}


 /*
	void OnGUI()
	{
		GUIStyle style = new GUIStyle();
		style.fontSize = 20;
		style.normal.textColor = new Color(0, 255f/255f, 244f/255f, 255f/255f);
		for (int i = 0, imax = mLines.Count; i < imax; ++i)
		{
			GUILayout.Label(mLines[i], style);
		}
	}*/
}

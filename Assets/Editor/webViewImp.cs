using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Demonstrate calling javascript from C# sdie.
/// </summary>
public class webViewImp : CustomWebViewEditorWindow
{
    //[MenuItem("Window/webView")]
    public static void Open(string prefix)
    {
		var w = CreateWebViewEditorWindow<webViewImp>("webViewImp", prefix, 500, 800, 500, 800);

        // Call javascript function within of the HTML file.
        EditorApplication.update = () =>
        {
            if (w != null)
            {
                //execute js inside html file  see index.html
                w.ExecuteJavascript("example", "changeText", Time.realtimeSinceStartup.ToString());

                //execute js as statment:
                //string s = "javascript:alert('hello world');";
            }
        };

        EditorApplication.delayCall += () =>
        {
            EditorApplication.delayCall += () =>
            {
                if (w != null)
                {
                    //  w.LoadURL("http://google.com");
                    //  w.LoadHTML("<html><body><script type=\"text/javascript\">alert('hello world');</script></body></html>");
                    //  w.LoadFile("file.html");
                }
            };
        };
    }

    public void Alert()
    {
        Debug.Log("shmoo");
    }
}
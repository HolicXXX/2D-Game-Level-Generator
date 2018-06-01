using UnityEngine;
using System;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CustomWebViewEditorWindow
{
    private object webViewEditorWindow = null;

    private static object webView;

    private static Type webViewEditorWindowType
    {
        get
        {
#if UNITY_5_4_OR_NEWER
            return (typeof(Editor).Assembly).GetType("UnityEditor.Web.WebViewEditorWindowTabs");
#else
            return Types.GetType("UnityEditor.Web.WebViewEditorWindow", "UnityEditor.dll");
#endif
        }
    }

    private static Type GetType(string typeName, string assemblyName)
    {
#if UNITY_5_4_OR_NEWER
        return Assembly.Load(assemblyName).GetType(typeName);
#else
        return Types.GetType(typeName, assemblyName);
#endif
    }

    [InitializeOnLoadMethod]
    private static void AddGlobalObjects()
    {
		// When Unity is not yet done loading, the Objects that you want to send message to does not exist.
		// however, since we are using the editor default callbacks (update, lateuodate, delayCall...) which are used like coroutine, we must make sure all the globals we use when calling js are ready
		// that is way we use  "InitializeOnLoadMethod" and static so it will be called first.
		AddGlobalObject(Type.GetType("webView"));
    }

    public static T CreateWebViewEditorWindow<T>(string title, string sourcesPath, int minWidth, int minHeight, int maxWidth, int maxHeight) where T : CustomWebViewEditorWindow, new()
    {
        // You need to use reflection to get the method to start with, then "construct" it by supplying type arguments with MakeGenericMethod:

        var createMethod = webViewEditorWindowType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).MakeGenericMethod(webViewEditorWindowType);

        //For a static method (such as create in UnityEditor.Web.WebViewEditorWindow),
        //pass null as the first argument to Invoke. That's nothing to do with generic methods - it's just normal reflection.
        var window = createMethod.Invoke(null, new object[] {
            title,
            sourcesPath,
            minWidth,
            minHeight,
            maxWidth,
            maxHeight
        });

        //saving the window object in webViewEditorWindow
        var customWebEditorWindow = new T { webViewEditorWindow = window };

        // attach a delegate which will execute after the editor is done updating inspectors
        EditorApplication.delayCall += () =>
        {
            EditorApplication.delayCall += () =>
            {
                //after all was updated I also take the inst of the window.
                webView = webViewEditorWindowType.GetField("m_WebView", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(customWebEditorWindow.webViewEditorWindow);
                //addgloabals is necessary for the js communication with the window
                AddGlobalObject(Type.GetType("webView"));
            };
        };

        return customWebEditorWindow;
    }

    private static void AddGlobalObject(Type type)
    {
        var jsproxyMgrType = GetType("UnityEditor.Web.JSProxyMgr", "UnityEditor.dll");
        // calling js init static function using reflection
        var instance = jsproxyMgrType.GetMethod("GetInstance").Invoke(null, new object[0]);

        if (jsproxyMgrType != null && instance != null)
        {
            // activate a list of types from which all the global types are ging to be called.
            // since we are using the "send message" mechanisem unity must know about the objects we are about to send to the js, before it is being sent.
            //jsproxyMgrType.GetMethod("AddGlobalObject").Invoke(instance, new object[] { type.Name, Activator.CreateInstance(type) });
        }
    }

    public void LoadURL(string path)
    {
        try
        {
            if (webViewEditorWindow != null && webView != null)
            {
                MethodInfo invokeLoadURLMethod = webView.GetType().GetMethod("LoadURL", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance);
                if (invokeLoadURLMethod != null)
                {
                    object[] param = new object[] { path };
                    invokeLoadURLMethod.Invoke(webView, param);
                }
            }
        }
        catch (TargetInvocationException ex)
        {
            // should be set as null to open the editor window again.
            webView = null;
            // force stop calling the delegate even after closing the window.
            EditorApplication.update = null;
            Debug.LogFormat("{0}", ex.Message);
            return;
        }
    }

    public void LoadFile(string fileFullPath)
    {
#if UNITY_STANDALONE_WIN

        fileFullPath.Replace("\\", "/");
#endif
        if (!fileFullPath.StartsWith("file:///{0}"))
            fileFullPath.Insert(0, "file:///{0}");

        LoadURL(fileFullPath);
    }

    public void LoadHTML(string htmlInput)
    {
        stringify html = new stringify(htmlInput);
        string input = "javascript:document.body.innerHTML=" + html.txt;
        if (webViewEditorWindow != null && webView != null)
        {
            LoadURL(input);
        }
    }

    public void ExecuteJavascript(string objectName, string funcName, params object[] args)
    {
#if UNITY_5_4_OR_NEWER
        try
        {
            if (webViewEditorWindow != null && webView != null)
            {
                MethodInfo invokeJSMethod = webView.GetType().GetMethod("ExecuteJavascript");
                if (invokeJSMethod != null)
                {
                    object[] param = new object[] { PrepareJSMethod(objectName, funcName, args) };
                    invokeJSMethod.Invoke(webView, param);
                }
            }
        }
        catch (TargetInvocationException ex)
        {
            // should be set as null to open the editor window again.
            webView = null;
            // force stop calling the delegate even after closing the window.
            EditorApplication.update = null;
            Debug.LogFormat("{0}", ex.Message);
            return;
        }
#else
        var invokeJSMethodMethod = webViewEditorWindowType.GetMethod ("InvokeJSMethod", BindingFlags.NonPublic | BindingFlags.Instance);
        if (invokeJSMethodMethod != null)
        {
            invokeJSMethodMethod.Invoke(webViewEditorWindow, new object[] { objectName, funcName, args });
        }
        else
            Debug.LogErrorFormat("No {0}.{1} is found.", objectName, funcName);
#endif
    }

    public static string PrepareJSMethod(string objectName, string name, params object[] args)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(objectName);
        stringBuilder.Append('.');
        stringBuilder.Append(name);
        stringBuilder.Append('(');
        bool flag = true;
        for (int i = 0; i < args.Length; i++)
        {
            object obj = args[i];
            if (!flag)
            {
                stringBuilder.Append(',');
            }
            bool flag2 = obj is string;
            if (flag2)
            {
                stringBuilder.Append('"');
            }
            stringBuilder.Append(obj);
            if (flag2)
            {
                stringBuilder.Append('"');
            }
            flag = false;
        }
        stringBuilder.Append(");");
        return stringBuilder.ToString();
    }

    [Serializable]
    private class stringify
    {
        //must only contain 1 serialized member
        public stringify(string inputString)
        {
            x = inputString;
            //the expected string format is:"{<string name or null>:\" \"} always > 6 - no null cases
            x = JsonUtility.ToJson(this);
            x = x.Substring(5, x.Length - 6);
        }

        [SerializeField]
        //must be 1 char name
        private string x;

        //getter to stringify member
        public string txt
        {
            get { return x; }
        }
    }
}


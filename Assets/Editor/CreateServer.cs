// /*
// 	Allen
// 	2018/5/29
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class CreateServer
{   
	private static HttpListener listener = new HttpListener();
	private const string prefix = "http://localhost:3000/";
 
	//[MenuItem("Level/Create Server")]
	private static void OpenServer()
	{
		listener.Prefixes.Clear();
        listener.Prefixes.Add(prefix);
		listener.Start();
		var t = ListenRequest();
		webViewImp.Open(prefix);
	}
    
	private static byte[] ReadBytes(string subpath)
	{
		if(subpath.StartsWith("/")) subpath = subpath.Substring(1, subpath.Length - 1);
		var path = Path.Combine(Application.dataPath, "../../../tools/level_test/0.0.23", subpath);
		Debug.Log($"{path}");
		var ret = File.ReadAllBytes(path);
		return ret;
	}

	private static async Task ListenRequest()
	{
		try
		{
			while (listener.IsListening)
			{
				var ctx = await listener.GetContextAsync();
				string route = ctx.Request.Url.AbsolutePath;
				var buf = new byte[0];
				if (route == "/")
				{
					buf = ReadBytes("index.html");
					ctx.Response.ContentType = "text/html";
				}
				else
				{
					buf = ReadBytes(route);
				}
				ctx.Response.ContentLength64 = buf.Length;
				await ctx.Response.OutputStream.WriteAsync(buf, 0, buf.Length);
				ctx.Response.OutputStream.Close();
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
	}

	//[MenuItem("Level/Close Server")]
    private static void CloseServer()
	{
		if (listener.IsListening)
        {
			listener.Stop();
            //listener.Close();
        }
	}
}

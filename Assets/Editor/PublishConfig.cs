using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

public static class PublishConfig
{
	private static void LoadAllObjects(Scene scene)
	{
		Debug.Log($"Load Scene: {scene.name}");
		var objs = scene.GetRootGameObjects();
		var border = objs.FirstOrDefault(o => o.name == "Border");
		var root = objs.FirstOrDefault(o => o.name == "Root");
		if (!root)
		{
			Debug.LogError("No Root GameObject");
			return;
		}

		var childrens = root.GetComponentsInChildren<DynamicObject>(true);
		Debug.Log($"Targets Count: {childrens.ToArray().Length}");
		var json = ParseSceneToJson(border, childrens);
		Debug.Log(json);
		WriteJsonToFile(json, scene.name);
	}

	private static string ParseSceneToJson(GameObject stage, IEnumerable<DynamicObject> objs)
	{
		Debug.Log("Parse Scene To Json");
		var cfg = new OutputScene();
		if (stage)
		{
			cfg.width = (stage.transform.localScale.x * 100 / DynamicObject.Factor).Truncate();
			cfg.height = (stage.transform.localScale.y * 100 / DynamicObject.Factor).Truncate();
		}
		else
		{
			cfg.width = (640f / DynamicObject.Factor).Truncate();
			cfg.height = (4000f / DynamicObject.Factor).Truncate();
		}
		cfg.objects = objs.Select(o => o.GetConfig(cfg.height)).ToArray();
		//return JsonUtility.ToJson(cfg, true);//not min
		return SerializeObjectToJson(cfg, typeof(OutputScene));//min & simple
	}

	private static void WriteJsonToFile(string json, string fileName)
	{
		Debug.Log("Write Scene to File");
		fileName = Path.Combine(Application.dataPath, "Config", fileName + ".json");
		File.WriteAllText(fileName, json, System.Text.Encoding.UTF8);
	}

	[MenuItem("Level/Publish All Scenes", false, -10)]
	private static void LoadAllScenes()
	{
		var scenes = AssetDatabase.FindAssets("", new string[] { "Assets/Scenes" }).Select(s => AssetDatabase.GUIDToAssetPath(s)).ToArray();
		EditorUtility.DisplayProgressBar("Publish All Scenes", $"Count:{0}/{scenes.Length}", 0);
		float c = 0;
		foreach (var s in scenes)
		{
			EditorSceneManager.OpenScene(s);
			//LoadCurrentScenes();
			var scene = SceneManager.GetActiveScene();
            LoadAllObjects(scene);
			float progress = ++c / scenes.Length;
			ShellHelper.ProcessCommand($"cp {scene.name}.json ../../../../tools/json2ts/json/{scene.name}.json", Path.Combine(Application.dataPath, "Config"));
			EditorUtility.DisplayProgressBar("Publish All Scenes", $"Count:{c}/{scenes.Length}, {scene.name} Done", progress);
		}
        AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
		var vs = "/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/share/dotnet";
        var list = vs.Split(new char[] { ':' }).ToList();
		var req = ShellHelper.ProcessCommand($"cd ../../../../tools/json2ts && node run.js", Path.Combine(Application.dataPath, "Config"), list);
        req.onLog += (arg1, arg2) =>
        {
            Debug.Log(arg2);
        };
        req.onError += () =>
        {
            Debug.LogError("Process Error");
        };
	}

	[MenuItem("Level/Publish Current Scene", false, -9)]
	private static void LoadCurrentScenes()
	{
		var scene = SceneManager.GetActiveScene();
		LoadAllObjects(scene);
		AssetDatabase.Refresh();
		var req = ShellHelper.ProcessCommand($"cp {scene.name}.json ../../../../tools/json2ts/json/{scene.name}.json", Path.Combine(Application.dataPath, "Config"));
        req.onLog += (arg1, arg2) =>
        {
            Debug.Log(arg2);
        };
        req.onError += () =>
        {
            Debug.LogError("Process Error");
        };
	}

	[MenuItem("Level/Test Current Scene", false, -8)]
	private static void TestCurrentScene()
	{
		var scene = SceneManager.GetActiveScene();
		Debug.Log($"Test Scene: {scene.name}");
		LoadAllObjects(scene);
		AssetDatabase.Refresh();
        
		var vs = "/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/share/dotnet";
		var list = vs.Split(new char[] { ':' }).ToList();
		var req = ShellHelper.ProcessCommand($"cp {scene.name}.json ../../../../tools/json2ts/json/level_test.json && cd ../../../../tools && sh level_test.sh", Path.Combine(Application.dataPath, "Config"), list);
		req.onLog += (arg1, arg2) =>
		{
			Debug.Log(arg2);
		};
		req.onError += () =>
		{
			Debug.LogError("Process Error");
		};
	}

	private static string SerializeObjectToJson(object obj, Type type)
	{
		using (MemoryStream m = new MemoryStream())
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
			ser.WriteObject(m, obj);
			m.Position = 0;
			using (StreamReader sr = new StreamReader(m))
			{
				return sr.ReadToEnd();
			}
		}
	}
}

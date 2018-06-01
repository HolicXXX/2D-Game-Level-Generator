// /*
// 	Allen
// 	2018/5/18
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class CreateDynamicPrefabs
{
	static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	[MenuItem("GameObject/CreatePrefabs/CreateBox", false, 0)]
    static void CreateBox()
	{
		if(CheckPrefabs())
			CreateNode(DynamicObjectType.box);
	}

	[MenuItem("GameObject/CreatePrefabs/CreateCircle", false, 1)]
    static void CreateCircle()
    {
		if (CheckPrefabs())
			CreateNode(DynamicObjectType.circle);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateTriangle", false, 2)]
    static void CreateTriangle()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.triangle);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateCross", false, 3)]
    static void CreateCross()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.cross);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateHexagon", false, 4)]
	static void CreateHexagon()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.hexagon);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateStar", false, 5)]
    static void CreateStar()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.star);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateConcave", false, 6)]
    static void CreateConcave()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.concave);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateGarland", false, 7)]
    static void CreateGarland()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.garland);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateRope", false, 8)]
    static void CreateRope()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.rope);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateDiamond", false, 9)]
    static void CreateDiamond()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.diamond);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateBalloon", false, 10)]
    static void CreateBalloon()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.balloon);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateWindmill", false, 11)]
    static void CreateWindmill()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.windmill);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateSnow", false, 12)]
    static void CreateSnow()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.snow);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateArrow", false, 13)]
    static void CreateArrow()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.arrow);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateTriangle2", false, 14)]
	static void CreateTriangle2()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.triangle2);
    }
    
	[MenuItem("GameObject/CreatePrefabs/CreateDandelion", false, 15)]
    static void CreateDandelion()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.dandelion);
    }

	[MenuItem("GameObject/CreatePrefabs/CreateBird", false, 16)]
    static void CreateBird()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.bird);
    }

	[MenuItem("GameObject/CreatePrefabs/CreatePendulum", false, 17)]
	static void CreatePendulum()
    {
        if (CheckPrefabs())
			CreateNode(DynamicObjectType.pendulum);
    }

	public static GameObject CreateNode(DynamicObjectType type, Transform t = null) {
		GameObject ret = null;
		t = t ?? Selection.activeTransform;      
		if (prefabs.TryGetValue(type.ToString(), out ret))
		{
			ret = Object.Instantiate(ret, t);
		}
		return ret;
	}

	[InitializeOnLoadMethod]
    static void InitPrefabs()
	{
        Debug.Log("InitPrefabs");
        var names = typeof(DynamicObjectType).GetEnumNames();
        if(prefabs.Count < names.Length) {
			var guids = AssetDatabase.FindAssets("", new string[] { "Assets/Prefabs" });
			var ps = guids.Select(AssetDatabase.GUIDToAssetPath).Select(p => AssetDatabase.LoadAssetAtPath(p, typeof(GameObject)));
            foreach (var p in ps)
            {
				prefabs[p.name] = p as GameObject;
            }
        }
	}

	static bool CheckPrefabs()
	{
		InitPrefabs();
		if(!Selection.activeGameObject || Selection.activeGameObject.name != "Root"){
			Debug.LogWarning("Must Select [Root] Node");
			return false;
		}
		return true;
	}

	[MenuItem("GameObject/CreatePrefabs", true, -10)]
    static bool Enabled()
	{
		return CheckPrefabs();
	}
}

// /*
// 	Allen
// 	2018/5/22
// 	allendk@foxmail.com
// */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class CreateHelperWindow : EditorWindow
{
	private enum GroupType
	{
		Box,
		Ball,
		Triangle,
		Hexagon,
		Star,
	}

	private enum GroupShape
	{
		//ball & triangle
		Diamond = 0,
		Vertical,
		Vertical_uneven,
		Horizontal,
		Horizontal_uneven,
		Ring,
	}

	private static GameObject root;

	private GroupType type;

	private Vector2 keyPoint = Vector2.zero;
	private float singleWidth = 0;
	private float singleHeight = 0;
	private int rowCount = 0;
	private int columnCount = 0;
	private float padding = 0;
	private float rotation = 0;
	private float singleRotation = 0;

	private float singleRadius = 0;
	private GroupShape groupShape = GroupShape.Diamond;
	private int nodeCount = 0;
	private float ringRadius = 0;

	[MenuItem("Level/Group Helper")]
	static void ShowWindow()
	{
		var w = GetWindow<CreateHelperWindow>(true, "CreateHelper", true);
		w.minSize = new Vector2(400, 600);
		w.maxSize = new Vector2(400, 600);
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginScrollView(Vector2.zero);
		EditorGUILayout.BeginVertical();
		EditorGUILayout.Space();
		type = (GroupType)EditorGUILayout.EnumPopup("Group Type:", type);
		groupShape = (GroupShape)EditorGUILayout.EnumPopup("Group Shape:", groupShape);
		keyPoint = EditorGUILayout.Vector2Field("Center Point:", keyPoint);
		if (type == GroupType.Box) singleWidth = EditorGUILayout.FloatField("Single Node Width:", singleWidth);
		if (type == GroupType.Box) singleHeight = EditorGUILayout.FloatField("Single Node Height:", singleHeight);
		if (type != GroupType.Box) singleRadius = EditorGUILayout.FloatField("Single Node Radius:", singleRadius);
		if (groupShape != GroupShape.Ring) rowCount = EditorGUILayout.IntField("Row Count:", rowCount);
		if (groupShape != GroupShape.Ring) columnCount = EditorGUILayout.IntField("Column Count:", columnCount);
		if (groupShape == GroupShape.Ring) nodeCount = EditorGUILayout.IntField("Node Count:", nodeCount);
		if (groupShape == GroupShape.Ring) ringRadius = EditorGUILayout.FloatField("Ring Radius:", ringRadius);
		EditorGUILayout.Space();
		if (groupShape != GroupShape.Ring) padding = EditorGUILayout.FloatField("Node Padding:", padding);
		rotation = EditorGUILayout.FloatField("Group Rotation:", rotation);
		singleRotation = EditorGUILayout.FloatField("Single Node Rotation:", singleRotation);
		EditorGUILayout.Space();

		GameObject obj = null;
		if (GUILayout.Button($"Create {type} Group", GUILayout.Height(40)))
		{
			obj = CreateGroupAction();
		}

		if (obj)
		{
			Selection.activeGameObject = obj;
			SceneView.FrameLastActiveSceneView();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}

	private void OnFocus()
	{
		var scene = SceneManager.GetActiveScene();
		root = scene.GetRootGameObjects().Where(o => o.name == "Root").SingleOrDefault();
		if (!root)
		{
			Debug.LogWarning("No [Root] Node in current scene.");
			this.Close();
			return;
		}
	}

#region CreateGroup

	public GameObject CreateGroupAction()
	{
		if (!root)
		{
			this.ShowNotification(new GUIContent("No [Root] Node in current scene."));
			return null;
		}
		if ((rowCount <= 0 && columnCount <= 0 && nodeCount == 0)
			|| (Mathf.Abs(singleWidth) < Mathf.Epsilon && Mathf.Abs(singleHeight) < Mathf.Epsilon && Mathf.Abs(singleRadius) < Mathf.Epsilon && Mathf.Abs(ringRadius) < Mathf.Epsilon))
		{
			this.ShowNotification(new GUIContent("Invalid Properties"));
			return null;
		}
		bool hor = groupShape == GroupShape.Horizontal_uneven || groupShape == GroupShape.Horizontal;
		var Obj = new GameObject($"Multi{type}Group");
		var t = Obj.transform;
		t.SetParent(root.transform);
		Vector2[] posArr = new Vector2[0];

		bool useWidthHeight = type == GroupType.Box;
		Vector2 wh = useWidthHeight ? new Vector2(singleWidth, singleHeight) : new Vector2(singleRadius * 2, singleRadius * 2);
		switch (groupShape)
		{
			case GroupShape.Diamond:
				posArr = GenerateDiamondPosition(Vector2.zero, wh.x, wh.y, padding, padding, columnCount);
				break;
			case GroupShape.Horizontal_uneven:
			case GroupShape.Vertical_uneven:
				posArr = GenerateUnevenPosition(Vector2.zero, wh.x, wh.y, padding, padding, rowCount, columnCount);
				break;
			case GroupShape.Horizontal:
			case GroupShape.Vertical:
				posArr = GenerateNormalPosition(Vector2.zero, wh.x, wh.y, padding, padding, rowCount, columnCount);
				break;
			case GroupShape.Ring:
				posArr = GenerateRingPosition(Vector2.zero, ringRadius, nodeCount);
				break;
		}

		foreach (var pos in posArr)
			CreateNode(type, t, useWidthHeight ? new Vector3(wh.x, wh.y, 1) : new Vector3(wh.x / 2, wh.y / 2, 1), new Vector3(pos.x, pos.y, 1), singleRotation);

		t.localPosition = new Vector3(keyPoint.x, keyPoint.y, 1);
		t.localEulerAngles = new Vector3(0, 0, hor ? rotation - 90 : rotation);
		return Obj;
	}

	private GameObject CreateNode(GroupType gtype, Transform parent, Vector3 scale, Vector3 pos, float angle)
	{
		DynamicObjectType otype = DynamicObjectType.circle;
		switch (gtype)
		{
			case GroupType.Box:
				otype = DynamicObjectType.box;
				break;
			case GroupType.Ball:
				otype = DynamicObjectType.circle;
				break;
			case GroupType.Triangle:
				otype = DynamicObjectType.triangle;
				break;
			case GroupType.Hexagon:
				otype = DynamicObjectType.hexagon;
				break;
			case GroupType.Star:
				otype = DynamicObjectType.star;
				break;
		}

		var ball = CreateDynamicPrefabs.CreateNode(otype, parent);
		if (ball)
		{
			var tran = ball.transform;
			tran.localScale = scale;
			tran.localPosition = pos;
			if (Math.Abs(angle) >= Mathf.Epsilon)
				tran.localEulerAngles = new Vector3(0, 0, angle);
		}
		else
			Debug.LogError($"Create {otype} Failed");
		return ball;
	}

#endregion

#region Position
	private Vector2[] GenerateDiamondPosition(Vector2 centerPos, float nodeWidth, float nodeHeight, float hGap, float vGap, int maxNodeNum, bool circleShape = true)
	{
		List<Vector2> list = new List<Vector2>();
		float deltaY = (nodeHeight + vGap) * (circleShape ? Mathf.Cos(Mathf.PI / 6) : 1);
		for (int r = 0; r < maxNodeNum * 2 - 1; ++r)
		{
			int rowNum = r <= maxNodeNum - 1 ? r + 1 : maxNodeNum - (r - (maxNodeNum - 1));
			float startX = -(nodeWidth + hGap) * rowNum / 2 + (nodeWidth + hGap) / 2;//-(rowNum / 2 - 1) * (nodeWidth + gap) + (rowNum % 2 == 0 ? -(nodeWidth + gap) / 2 : (nodeWidth + gap) / 2);
			float startY = (r <= maxNodeNum - 1 ? 1 : -1) * (maxNodeNum - rowNum) * deltaY;
			for (int c = 0; c < rowNum; ++c)
			{
				var pos = centerPos + new Vector2(startX + (nodeWidth + hGap) * c, startY);
				list.Add(pos);
			}
		}
		return list.ToArray();
	}

	private Vector2[] GenerateUnevenPosition(Vector2 centerPos, float nodeWidth, float nodeHeight, float hGap, float vGap, int rowNum, int colNum, bool circleShape = true)
	{
		List<Vector2> list = new List<Vector2>();
		float deltaY = (nodeHeight + vGap) * (circleShape ? Mathf.Cos(Mathf.PI / 6) : 1);
		for (int r = 0; r < rowNum; ++r)
		{
			int rowNodeCount = r % 2 == 0 ? colNum : colNum - 1;
			float startX = -(nodeWidth + hGap) * rowNodeCount / 2 + (nodeWidth + hGap) / 2;
			float startY = (rowNum / 2f - (r + 1)) * deltaY + deltaY / 2;
			for (int c = 0; c < rowNodeCount; ++c)
			{
				var pos = centerPos + new Vector2(startX + (nodeWidth + hGap) * c, startY);
				list.Add(pos);
			}
		}
		return list.ToArray();
	}

	private Vector2[] GenerateNormalPosition(Vector2 centerPos, float nodeWidth, float nodeHeight, float hGap, float vGap, int rowNum, int colNum)
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < rowNum * colNum; ++i)
		{
			int c = i % colNum;
			int r = i / rowNum;
			float delta = nodeWidth + hGap;
			float x = -delta * rowNum / 2 + delta / 2 + delta * c;
			delta = nodeHeight + vGap;
			float y = (rowNum / 2f - (r + 1)) * delta + delta / 2;
			var pos = centerPos + new Vector2(x, y);
			list.Add(pos);
		}

		return list.ToArray();
	}

	private Vector2[] GenerateRingPosition(Vector2 centerPos, float radius, int num)
	{
		List<Vector2> list = new List<Vector2>();
		float delta = Mathf.PI * 2 / num;
		for (int i = 0; i < num; ++i)
		{
			float d = delta * i;
			float x = Mathf.Cos(d) * radius;
			float y = Mathf.Sin(d) * radius;
			list.Add(centerPos + new Vector2(x, y));
		}

		return list.ToArray();
	}

#endregion
}

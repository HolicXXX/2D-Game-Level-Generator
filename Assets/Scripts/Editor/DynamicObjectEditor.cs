// /*
// 	Allen
// 	2018/5/18
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicObject))]
[CanEditMultipleObjects]
public class DynamicObjectEditor : Editor
{
	SerializedProperty shapeType;
    //SerializedProperty position;
    //SerializedProperty width_height;
    //SerializedProperty rotation;
	//SerializedProperty initialVelocity;
	//SerializedProperty initialAngularVelocity;
	SerializedProperty triggerDistance;
	SerializedProperty fireInterval;
	SerializedProperty triggerVelocity;
	SerializedProperty randomVelocityRange;
	SerializedProperty triggerAngularVelocity;
	SerializedProperty randomAngularVelocityRange;

	private bool canShowWH;

	private void OnEnable()
	{
		shapeType = serializedObject.FindProperty("shapeType");
		//position = serializedObject.FindProperty("position");
		//width_height = serializedObject.FindProperty("width_height");
		//rotation = serializedObject.FindProperty("rotation");
		//initialVelocity = serializedObject.FindProperty("initialVelocity");
		//initialAngularVelocity = serializedObject.FindProperty("initialAngularVelocity");
		triggerDistance = serializedObject.FindProperty("triggerDistance");
		fireInterval = serializedObject.FindProperty("fireInterval");
		triggerVelocity = serializedObject.FindProperty("triggerVelocity");
		randomVelocityRange = serializedObject.FindProperty("randomVelocityRange");
		triggerAngularVelocity = serializedObject.FindProperty("triggerAngularVelocity");
		randomAngularVelocityRange = serializedObject.FindProperty("randomAngularVelocityRange");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(shapeType);
		//EditorGUILayout.PropertyField(position);
		//EditorGUILayout.PropertyField(width_height);
		//EditorGUILayout.PropertyField(rotation);
		EditorGUILayout.PropertyField(triggerDistance);
		if (Mathf.Abs(triggerDistance.floatValue) > Mathf.Epsilon)
		{
			EditorGUILayout.PropertyField(fireInterval);
			EditorGUILayout.PropertyField(triggerVelocity);
			EditorGUILayout.PropertyField(randomVelocityRange);
			EditorGUILayout.PropertyField(triggerAngularVelocity);
			EditorGUILayout.PropertyField(randomAngularVelocityRange);
		}
		serializedObject.ApplyModifiedProperties();
		var obj = target as DynamicObject;
		if (obj.transform.parent)
		{
			var rb = obj.GetComponent<Rigidbody2D>();
			var render = obj.GetComponent<SpriteRenderer>();
			if (render && rb) render.color = rb.bodyType == RigidbodyType2D.Dynamic ? Color.white : Color.black;
		}
		obj.UpdateDisplay();
	}

	private void OnSceneGUI()
	{
		var obj = target as DynamicObject;
		float dis = obj.TriggerDistance;
		var pos = obj.transform.position;
		if (Mathf.Abs(dis) > Mathf.Epsilon)
		{
			Handles.color = Color.green;
			Handles.DrawDottedLine(new Vector3(pos.x - 10, pos.y - dis, pos.z), new Vector3(pos.x + 10, pos.y - dis, pos.z), 5f);
		}
	}
}

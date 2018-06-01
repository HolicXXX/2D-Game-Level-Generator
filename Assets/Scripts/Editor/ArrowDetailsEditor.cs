// /*
// 	Allen
// 	2018/5/23
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArrowDetails))]
[CanEditMultipleObjects]
public class ArrowDetailsEditor : Editor
{
	SerializedProperty headRadius;
	SerializedProperty lineLength;
	SerializedProperty thickness;

	private void OnEnable()
	{
		headRadius = serializedObject.FindProperty("headRadius");
		lineLength = serializedObject.FindProperty("lineLength");
		thickness = serializedObject.FindProperty("thickness");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(headRadius);
		EditorGUILayout.PropertyField(lineLength);
		EditorGUILayout.PropertyField(thickness);

		serializedObject.ApplyModifiedProperties();
        var obj = target as ArrowDetails;
		if (obj.transform.parent)
		{
			var rb = obj.GetComponent<Rigidbody2D>();
			var renders = obj.GetComponentsInChildren<SpriteRenderer>();
			foreach(var render in renders)
			{
				render.color = rb.bodyType == RigidbodyType2D.Dynamic ? Color.white : Color.black;
			}
		}
        obj.UpdateDisplay();
	}
}

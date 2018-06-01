// /*
// 	Allen
// 	2018/5/21
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RopeDetails))]
[CanEditMultipleObjects]
public class RopeDetailsEditor : Editor
{
	SerializedProperty lineRenderer;
	SerializedProperty ballRenderer;
	SerializedProperty hang;
	SerializedProperty ballCount;
	SerializedProperty line_thickness;
	SerializedProperty line_length;
	SerializedProperty ball_radius;

	private void OnEnable()
	{
		lineRenderer = serializedObject.FindProperty("lineRenderer");
		ballRenderer = serializedObject.FindProperty("ballRenderer");
		hang = serializedObject.FindProperty("hang");
		ballCount = serializedObject.FindProperty("ballCount");
		line_thickness = serializedObject.FindProperty("line_thickness");
		line_length = serializedObject.FindProperty("line_length");
		ball_radius = serializedObject.FindProperty("ball_radius");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(lineRenderer);
		EditorGUILayout.PropertyField(ballRenderer);
		EditorGUILayout.PropertyField(hang);
		EditorGUILayout.PropertyField(ballCount);
		EditorGUILayout.PropertyField(line_thickness);
		EditorGUILayout.PropertyField(line_length);
		EditorGUILayout.PropertyField(ball_radius);

		serializedObject.ApplyModifiedProperties();
		var obj = target as RopeDetails;
        if (obj.transform.parent)
        {
            var rb = obj.GetComponent<Rigidbody2D>();
            var renders = obj.GetComponentsInChildren<SpriteRenderer>();
            foreach (var render in renders)
            {
                render.color = rb.bodyType == RigidbodyType2D.Dynamic ? Color.white : Color.black;
            }
        }
		obj.UpdateDisplay();
	}
}

// /*
// 	Allen
// 	2018/5/21
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConcaveDetails))]
[CanEditMultipleObjects]
public class ConcaveDetailsEditor : Editor
{
	SerializedProperty thickness;
	SerializedProperty wall_height;
	SerializedProperty bottom_width;

    void OnEnable()
    {
		thickness = serializedObject.FindProperty("thickness");
		wall_height = serializedObject.FindProperty("wall_height");
		bottom_width = serializedObject.FindProperty("bottom_width");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
		EditorGUILayout.PropertyField(thickness);
		EditorGUILayout.PropertyField(wall_height);
		EditorGUILayout.PropertyField(bottom_width);

        serializedObject.ApplyModifiedProperties();
		var obj = target as ConcaveDetails;
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

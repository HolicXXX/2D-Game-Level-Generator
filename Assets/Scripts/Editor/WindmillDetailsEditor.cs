// /*
// 	Allen
// 	2018/5/23
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindmillDetails))]
[CanEditMultipleObjects]
public class WindmillDetailsEditor : Editor
{
	SerializedProperty lineWidth;
	SerializedProperty thickness;
	SerializedProperty radius;
	SerializedProperty gap;

	private void OnEnable()
	{
		lineWidth = serializedObject.FindProperty("lineWidth");
		thickness = serializedObject.FindProperty("thickness");
		radius = serializedObject.FindProperty("radius");
		gap = serializedObject.FindProperty("gap");
	}

	public override void OnInspectorGUI()
    {
        serializedObject.Update();
		EditorGUILayout.PropertyField(lineWidth);
		EditorGUILayout.PropertyField(thickness);
        EditorGUILayout.PropertyField(radius);
        EditorGUILayout.PropertyField(gap);

        serializedObject.ApplyModifiedProperties();
		var obj = target as WindmillDetails;
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

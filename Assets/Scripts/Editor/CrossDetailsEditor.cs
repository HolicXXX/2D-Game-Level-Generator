// /*
// 	Allen
// 	2018/5/18
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrossDetails))]
[CanEditMultipleObjects]
public class CrossDetailsEditor : Editor
{
	SerializedProperty lrWidth;
	SerializedProperty lrHeight;
    SerializedProperty tdWidth;
	SerializedProperty tdHeight;

    void OnEnable()
    {
		lrWidth = serializedObject.FindProperty("lrWidth");
		lrHeight = serializedObject.FindProperty("lrHeight");
		tdWidth = serializedObject.FindProperty("tdWidth");
		tdHeight = serializedObject.FindProperty("tdHeight");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
		EditorGUILayout.PropertyField(lrWidth);
		EditorGUILayout.PropertyField(lrHeight);
		EditorGUILayout.PropertyField(tdWidth);
		EditorGUILayout.PropertyField(tdHeight);

        serializedObject.ApplyModifiedProperties();
		var obj = target as CrossDetails;
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

// /*
// 	Allen
// 	2018/5/18
// 	allendk@foxmail.com
// */
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public class SetStageBorderWindow : EditorWindow
{
	[MenuItem("Level/Set Level Width&Height")]
    private static void SetLevelRect()
    {
        var scene = EditorSceneManager.GetActiveScene();
        var obj = scene.GetRootGameObjects().FirstOrDefault(o => o.name == "Border");
        if (obj == null)
        {
            Debug.LogError("No Border GameObject");
            return;
        }
		var window = EditorWindow.GetWindow<SetStageBorderWindow>(true, "SetStageBorder", true);
        window.border = obj;
        window.Width = obj.transform.localScale.x;
        window.Height = obj.transform.localScale.y;
    }

	public GameObject border = null;

    private float width = 6.4f;
    public float Width
    {
        get { return width; }
        set
        {
			if (border && System.Math.Abs(width - value) > Mathf.Epsilon)
            {
                width = value;
                var tr = border.transform;
                tr.localScale = new Vector3(width, tr.localScale.y, tr.localScale.z);
                tr.localPosition = new Vector3(width / 2, tr.localPosition.y, tr.localPosition.z);
            }
        }
    }

    private float height = 40f;
    public float Height
    {
        get { return height; }
        set
        {
			if (border && System.Math.Abs(height - value) > Mathf.Epsilon)
            {
                height = value;
                var tr = border.transform;
                tr.localScale = new Vector3(tr.localScale.x, height, tr.localScale.z);
                tr.localPosition = new Vector3(tr.localPosition.x, height / 2, tr.localPosition.z);
            }
        }
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Width: ");
        this.Width = EditorGUILayout.FloatField(this.width);
        EditorGUILayout.LabelField("Height: ");
        this.Height = EditorGUILayout.FloatField(this.height);
        EditorGUILayout.EndVertical();
    }
}

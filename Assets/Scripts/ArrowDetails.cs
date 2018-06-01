// /*
// 	Allen
// 	2018/5/23
// 	allendk@foxmail.com
// */
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class ArrowDetails : MonoBehaviour
{
	[SerializeField]
	float headRadius = 1;
	public float HeadRadius => headRadius;

	[SerializeField]
	float lineLength = 4;
	public float LineLenght => lineLength;

	[SerializeField]
	float thickness = 1;
	public float Thickness => thickness;

	Transform head;
	Transform line;

	private void Awake()
	{
		var ts = GetComponentsInChildren<Transform>();
		foreach(var t in ts)
		{
			if (t.name == "head")
				head = t;
			else if (t.name == "line")
				line = t;
		}
	}

    public void UpdateDisplay()
	{
		if (!transform.parent) return;
		head.localScale = new Vector3(headRadius, headRadius, 1);
		head.localPosition = new Vector3(0, -headRadius, 1);
		line.localScale = new Vector3(thickness, lineLength, 1);
		line.localPosition = new Vector3(0, -(headRadius / 2 * 3 + lineLength / 2), 1);
	}
}

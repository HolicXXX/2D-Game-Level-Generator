// /*
// 	Allen
// 	2018/5/23
// 	allendk@foxmail.com
// */
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class WindmillDetails : MonoBehaviour
{
	[SerializeField]
	float lineWidth = 2f;
	public float LineWidth => lineWidth;

	[SerializeField]
	float thickness = .2f;
	public float Thickness => thickness;

	[SerializeField]
	float radius = .2f;
	public float Radius => radius;

	[SerializeField]
	float gap = .2f;
	public float Gap => gap;

	Transform left;
	Transform top;
	Transform right;
	Transform down;
	Transform center;

	private void Awake()
	{
		var ts = GetComponentsInChildren<Transform>();
		foreach(var t in ts)
		{
			if (t.name == "left")
				left = t;
			else if (t.name == "top")
				top = t;
			else if (t.name == "right")
				right = t;
			else if (t.name == "down")
				down = t;
			else if (t.name == "center")
				center = t;
		}
	}

	public void UpdateDisplay()
	{
		if (!transform.parent) return;
		left.localScale = top.localScale = right.localScale = down.localScale = new Vector3(lineWidth, thickness, 1);
		center.localScale = new Vector3(radius, radius, 1);
		float outCircle = lineWidth / 2 + gap + radius;
		left.localPosition = new Vector3(-outCircle, 0, 1);
		top.localPosition = new Vector3(0, outCircle, 1);
		right.localPosition = new Vector3(outCircle, 0, 1);
		down.localPosition = new Vector3(0, -outCircle, 1);
		top.localEulerAngles = new Vector3(0, 0, -90);
		down.localEulerAngles = new Vector3(0, 0, 90);
	}
}

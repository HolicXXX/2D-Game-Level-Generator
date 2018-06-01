// /*
// 	Allen
// 	2018/5/18
// 	allendk@foxmail.com
// */
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class CrossDetails : MonoBehaviour
{
	[SerializeField]
	private float lrWidth = 1;
	public float LRWidth => lrWidth;
	[SerializeField]
	[Range(0.001f, 1f)]
	private float lrHeight = .2f;
	public float LRHeight => lrHeight;

	[SerializeField]
    [Range(0.001f, 1f)]
    private float tdWidth = .2f;
	public float TDWidth => tdWidth;
	[SerializeField]
	private float tdHeight = 1;
	public float TDHeight => tdHeight;

	private Transform lr;
	private Transform td;

	private void Awake()
	{
		var ts = GetComponentsInChildren<Transform>();
		foreach(var t in ts) {
			if (t.name == "lr")
				lr = t;
			else if (t.name == "td")
				td = t;
		}
	}

	public void UpdateDisplay()
	{
		if(lr)
			lr.localScale = new Vector3(lrWidth, lrHeight, 1);

		if(td)
			td.localScale = new Vector3(tdWidth, tdHeight, 1);
	}
}

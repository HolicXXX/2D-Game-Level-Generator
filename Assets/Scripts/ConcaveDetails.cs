// /*
// 	Allen
// 	2018/5/21
// 	allendk@foxmail.com
// */
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class ConcaveDetails : MonoBehaviour
{
	[SerializeField]
	private float thickness = 1;
	public float Thickness => thickness;

	[SerializeField]
	private float wall_height = 3;
	public float Wall_Height => wall_height;

	[SerializeField]
	private float bottom_width = 6;
	public float Bottom_Width => bottom_width;

	private Transform left_wall;
	private Transform right_wall;
	private Transform bottom;

	void Awake()
	{
		var ts = GetComponentsInChildren<Transform>();
        foreach (var t in ts)
        {
			if (t.name == "left_wall")
				left_wall = t;
			else if (t.name == "right_wall")
				right_wall = t;
			else if (t.name == "bottom")
				bottom = t;
        }
	}

	public void UpdateDisplay()
    {
		if(left_wall && right_wall && bottom) {
			left_wall.localScale = new Vector3(thickness, wall_height, 1);
            right_wall.localScale = new Vector3(thickness, wall_height, 1);
			bottom.localScale = new Vector3(bottom_width, thickness, 1);
			left_wall.localPosition = new Vector3(-(bottom_width / 2 - thickness / 2), 0, 1);         
			right_wall.localPosition = new Vector3(bottom_width / 2 - thickness / 2, 0, 1);
			bottom.localPosition = new Vector3(0, -wall_height / 2 + thickness / 2, 1);
		}
    }
}

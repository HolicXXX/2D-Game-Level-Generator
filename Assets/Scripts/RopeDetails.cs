// /*
// 	Allen
// 	2018/5/21
// 	allendk@foxmail.com
// */
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class RopeDetails : MonoBehaviour
{
	[SerializeField]
	private Sprite lineRenderer;

	[SerializeField]
	private Sprite ballRenderer;

	[SerializeField]
	private bool hang = true;
	public bool Hang => hang;

	[SerializeField]
	[Range(0, 100)]
	private int ballCount = 3;
	public int BallCount => ballCount;

	[SerializeField]
	private float line_thickness = 0.1f;
	public float LineThickness => line_thickness;

	[SerializeField]
	private float line_length = 1f;
	public float LineLength => line_length;

	[SerializeField]
	private float ball_radius = 0.5f;
	public float BallRadius => ball_radius;

	private List<Transform> lines;
	private List<Transform> balls;

	private void Awake()
	{
		lines = new List<Transform>(100);
		balls = new List<Transform>(100);
		var ts = GetComponentsInChildren<Transform>();
		foreach(var t in ts) {
			if (t.name.IndexOf("line", System.StringComparison.OrdinalIgnoreCase) >= 0)
				lines.Add(t);
			else if (t.name.IndexOf("ball", System.StringComparison.OrdinalIgnoreCase) >= 0)
				balls.Add(t);
		}
	}

	public void UpdateDisplay()
    {
		if (!transform.parent) return;
		if (balls.Count == ballCount && lines.Count == (hang ? ballCount : Mathf.Clamp(ballCount - 1, 0, ballCount)))
			return;
		var tran = transform;
		for (int i = 0; i < ballCount;++i) {
			Transform t;
            //line
			t = lines.ElementAtOrDefault(i);
            if (!t)
            {
                var g = new GameObject($"line_{i}");
                g.transform.SetParent(tran);
                var render = g.AddComponent<SpriteRenderer>();
                render.sprite = lineRenderer;
                t = g.transform;
                lines.Insert(i, t);
            }
            t.localScale = new Vector3(line_thickness, line_length, 1);
			if(hang) {
				t.localPosition = new Vector3(0, -(i * (line_length + ball_radius * 2) + line_length / 2), 1);
			}else if(i < ballCount - 1) {
				t.localPosition = new Vector3(0, -((i + 1) * (line_length + ball_radius * 2) - line_length / 2), 1);
			}
			//ball
			t = balls.ElementAtOrDefault(i);
			if(!t)
			{
				var g = new GameObject($"ball_{i}");
                g.transform.SetParent(tran);
                var render = g.AddComponent<SpriteRenderer>();
				render.sprite = ballRenderer;
                t = g.transform;
                balls.Insert(i, t);
			}
			t.localScale = new Vector3(ball_radius, ball_radius, 1);
			t.localPosition = new Vector3(0, -((line_length + ball_radius * 2) * i + ball_radius + (hang ? line_length : 0)), 1);
		}
		if(!hang) {
			int i = ballCount - 1;
			var t = lines.ElementAtOrDefault(i);
            if (t)
            {
                t.transform.SetParent(null);
                DestroyImmediate(t.gameObject);
                lines[i] = null;
            }
		}
		for (var i = ballCount; i < lines.Capacity;++i) {
			var t = lines.ElementAtOrDefault(i);
			if(t) {
				t.transform.SetParent(null);
				DestroyImmediate(t.gameObject);
				lines[i] = null;
			}

			t = balls.ElementAtOrDefault(i);
			if(t) {
				t.transform.SetParent(null);
				DestroyImmediate(t.gameObject);
				balls[i] = null;
			}
		}
    }
}

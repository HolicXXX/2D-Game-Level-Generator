// /*
// 	Allen
// 	2018/5/31
// 	allendk@foxmail.com
// */
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class PendulumDetails : MonoBehaviour
{
	[SerializeField]
	private Sprite lineRenderer;

	[SerializeField]
	private Sprite ballRenderer;

	[SerializeField]
    private float line_thickness = 0.1f;
    public float LineThickness => line_thickness;

    [SerializeField]
    private float line_length = 2f;
    public float LineLength => line_length;

    [SerializeField]
    private float ball_radius = 0.5f;
    public float BallRadius => ball_radius;

    private Transform line;
    private Transform ball;

	private void Awake()
    {
		var trans = GetComponentsInChildren<Transform>();
		foreach (var tran in trans)
		{
			if (tran.name == "line")
				line = tran;
			else if (tran.name == "ball")
				ball = tran;
		}
    }

    public void UpdateDisplay()
    {
        if (!transform.parent) return;
        
        var tran = transform;

		line.localScale = new Vector3(line_thickness, line_length, 1);
		line.localPosition = new Vector3(0, -line_length / 2, 0);
		ball.localScale = new Vector3(ball_radius, ball_radius, 1);
		ball.localPosition = new Vector3(0, -line_length, 0);
    }
}

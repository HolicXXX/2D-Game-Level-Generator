using System.Text;
using UnityEngine;
using System;

public enum DynamicObjectType
{
    box,
    circle,
    triangle,
    hexagon,
    cross,
    star,
    concave,
    garland,
    rope,
    diamond,
    balloon,
    windmill,
    snow,
    arrow,
    triangle2,
    dandelion,
    bird,
	pendulum,
}

[ExecuteInEditMode]
[DisallowMultipleComponent]
[Serializable]
public class DynamicObject : MonoBehaviour
{   
	public static int Factor = 50;

	[SerializeField]
	private DynamicObjectType shapeType;
	public DynamicObjectType ShapeType => shapeType;

	//[SerializeField]
	//private Vector2 position;
	//public Vector2 Position => position;

	//[SerializeField]
	//private Vector2 width_height;
	//public Vector2 Width_Height => width_height;

	//[SerializeField]
	//private float rotation;
	//public float Rotation => rotation;

	//[SerializeField]
	//private Vector2 initialVelocity;
	//public Vector2 InitialVelocity { get { return initialVelocity; } }

	//[SerializeField]
	//private float initialAngularVelocity;
	//public Vector2 InitialAngularVelocity { get { return InitialAngularVelocity; } }

	[SerializeField]
	private float triggerDistance;
	public float TriggerDistance => triggerDistance;

	[SerializeField]
	private float fireInterval;
	public float FireInterval => fireInterval;

	[SerializeField]
	private Vector2 triggerVelocity;
	public Vector2 TriggerVelocity => triggerVelocity;

	[SerializeField]
	private Vector2 randomVelocityRange;
	public Vector2 RandomVelocityRange => randomVelocityRange;

	[SerializeField]
	private float triggerAngularVelocity;
	public float TriggerAngularVelocity => triggerAngularVelocity;

	[SerializeField]
	private float randomAngularVelocityRange;
	public float RandomAngularVelocityRange => randomAngularVelocityRange;

	private Transform tran;
	private void Awake()
	{
		tran = transform;
		//position = new Vector2(tran.localPosition.x, tran.localPosition.y);
		//width_height = new Vector2(tran.localScale.x, tran.localScale.y);
		//rotation = tran.localEulerAngles.z;
		//tran.hideFlags = HideFlags.HideInInspector;
		var render = GetComponent<SpriteRenderer>();
		if (render) render.hideFlags = HideFlags.HideInInspector;
	}

    public void UpdateDisplay()
	{
		//tran.localPosition = new Vector3(position.x, position.y, 1);
		//tran.localScale = new Vector3(width_height.x, width_height.y);
		//tran.localEulerAngles = new Vector3(0, 0, rotation);
	}

	private float FixFloat(float value, int deci = 4)
	{
		return value.Truncate(deci);
	}
    
	public OutputConfig GetConfig(float borderHeight)
	{
		var rb = GetComponent<Rigidbody2D>();

		OutputConfig cfg = new OutputConfig() { type = shapeType.ToString(), physicType = rb.bodyType.ToString(), name = name };
        
		cfg.init = new InitConfig();
		cfg.init.position = new float[] { FixFloat(tran.position.x * 100 / Factor), FixFloat(borderHeight - tran.position.y * 100 / Factor) };
		switch (shapeType)
		{
            case DynamicObjectType.box:
                cfg.init.width = FixFloat(tran.lossyScale.x * 100 / Factor);
                cfg.init.height = FixFloat(tran.lossyScale.y * 100 / Factor);
                break;
            case DynamicObjectType.circle:
            case DynamicObjectType.triangle:
            case DynamicObjectType.hexagon:
            case DynamicObjectType.star:
                cfg.init.radius = FixFloat(tran.lossyScale.x * 100 / Factor);
                break;
            case DynamicObjectType.cross:
                {
                    var cross = GetComponent<CrossDetails>();
                    cfg.init.width = FixFloat(cross.LRWidth * 100 / Factor);
                    cfg.init.height = FixFloat(cross.TDHeight * 100 / Factor);
                    cfg.init.thickness = FixFloat(cross.LRHeight * 100 / Factor);
                    cfg.init.thickness2 = FixFloat(cross.TDWidth * 100 / Factor);
                }
                break;
            case DynamicObjectType.concave:
                {
                    var concave = GetComponent<ConcaveDetails>();
                    cfg.init.width = FixFloat(concave.Bottom_Width * 100 / Factor);
                    cfg.init.height = FixFloat(concave.Wall_Height * 100 / Factor);
                    cfg.init.thickness = FixFloat(concave.Thickness * 100 / Factor);
                }
                break;
            case DynamicObjectType.rope:
                {
                    var rope = GetComponent<RopeDetails>();
                    cfg.init.thickness = FixFloat(rope.LineThickness * 100 / Factor);
                    cfg.special = new SpecialConfig
                    {
                        ropeHang = rope.Hang,
                        lineLength = FixFloat(rope.LineLength * 100 / Factor),
                        ropeBallCount = rope.BallCount,
                        ballRadius = FixFloat(rope.BallRadius * 100 / Factor)
                    };
                }
                break;
            case DynamicObjectType.diamond:
                cfg.init.width = FixFloat(tran.lossyScale.x * 174 / Factor);
                cfg.init.height = FixFloat(tran.lossyScale.y * 300 / Factor);
                break;
            case DynamicObjectType.balloon:
                break;
            case DynamicObjectType.garland:
                cfg.init.radius = FixFloat(tran.lossyScale.x * 150 / Factor);
                break;
            case DynamicObjectType.windmill:
                {
                    var windmill = GetComponent<WindmillDetails>();
                    cfg.init.thickness = FixFloat(windmill.Thickness * 100 / Factor);
                    cfg.special = new SpecialConfig
                    {
                        gap = FixFloat(windmill.Gap * 100 / Factor),
                        lineLength = FixFloat(windmill.LineWidth * 100 / Factor),
                        ballRadius = FixFloat(windmill.Radius * 100 / Factor)
                    };
                }
                break;
			case DynamicObjectType.snow:
				cfg.init.radius = FixFloat(tran.lossyScale.x * 50 / Factor);
				break;
            case DynamicObjectType.arrow:
                {
                    var arrow = GetComponent<ArrowDetails>();
                    cfg.init.thickness = FixFloat(arrow.Thickness * 100 / Factor);
                    cfg.special = new SpecialConfig
                    {
                        lineLength = FixFloat(arrow.LineLenght * 100 / Factor),
                        ballRadius = FixFloat(arrow.HeadRadius * 100 / Factor)
                    };
                }
                break;
			case DynamicObjectType.triangle2:
				cfg.init.width = FixFloat(tran.lossyScale.x * 150 / Mathf.Cos(Mathf.PI / 6) / Factor);
				cfg.init.height = FixFloat(tran.lossyScale.y * 150 / Factor);
				break;
			case DynamicObjectType.dandelion:
				cfg.init.width = FixFloat(tran.lossyScale.x * 100 / Factor);
				cfg.init.height = FixFloat(tran.lossyScale.y * 200 / Factor);
				break;
			case DynamicObjectType.bird:
				cfg.init.width = FixFloat(tran.lossyScale.x * 200 / Factor);
				cfg.init.height = FixFloat(tran.lossyScale.y * 100 / Factor);
				break;
			case DynamicObjectType.pendulum:
				{
					var pendulum = GetComponent<PendulumDetails>();
                    cfg.init.thickness = FixFloat(pendulum.LineThickness * 100 / Factor);
                    cfg.special = new SpecialConfig
                    {
                        lineLength = FixFloat(pendulum.LineLength * 100 / Factor),
                        ballRadius = FixFloat(pendulum.BallRadius * 100 / Factor)
                    };
                }
				break;
        }
		if (Mathf.Abs(tran.rotation.eulerAngles.z) > Mathf.Epsilon)
			cfg.init.rotation = FixFloat(-tran.rotation.eulerAngles.z);
		cfg.init.mass = FixFloat(rb.mass);
		//cfg.init.velocity = new float[] { initialVelocity.x, initialVelocity.y };
		//if (Math.Abs(initialAngularVelocity) > Mathf.Epsilon)
			//cfg.init.angularVelocity = initialAngularVelocity;
  
		if (Mathf.Abs(triggerDistance) > Mathf.Epsilon)
		{
            cfg.trigger = new TriggerConfig();
			cfg.trigger.distance = FixFloat(triggerDistance * 100 / Factor);
			if (Mathf.Abs(fireInterval) > Mathf.Epsilon)
				cfg.trigger.interval = fireInterval;
			if(triggerVelocity != Vector2.zero)
			    cfg.trigger.velocity = new float[] { FixFloat(triggerVelocity.x), FixFloat(-triggerVelocity.y) };
			if (Mathf.Abs(triggerAngularVelocity) > Mathf.Epsilon)
				cfg.trigger.angularVelocity = FixFloat(triggerAngularVelocity);
			if(randomVelocityRange != Vector2.zero)
			    cfg.trigger.randomVelocityRange = new float[] { FixFloat(randomVelocityRange.x), FixFloat(-randomVelocityRange.y) };
			if (Mathf.Abs(randomAngularVelocityRange) > Mathf.Epsilon)
				cfg.trigger.randomAngularVelocityRange = FixFloat(randomAngularVelocityRange);
		}

		return cfg;
	}

	#region Print
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("Dynamic Object: " + gameObject.name + " dynamic information:\n");
		//stringBuilder.AppendLine("Initial Velocity: " + initialVelocity.ToString());
		stringBuilder.AppendLine("After Trigger Velocity: " + triggerVelocity.ToString());
		//stringBuilder.AppendLine("Initial Angular Velocity: " + initialVelocity);
		stringBuilder.AppendLine("After Trigger Velocity: " + triggerAngularVelocity);
		return stringBuilder.ToString();
	}

	public string PhysicInformationToString()
	{
		var rb = GetComponent<Rigidbody2D>();
		//var cld = GetComponent<Collider2D>();
		StringBuilder stringBuilder = new StringBuilder("Dynamic Object: " + gameObject.name + " physic information:\n");
		stringBuilder.AppendLine("Type: " + rb.bodyType);
		stringBuilder.AppendLine("gravityScale: " + rb.gravityScale);
		stringBuilder.AppendLine("collisionDetectionMode: " + rb.collisionDetectionMode);

		return stringBuilder.ToString();
	}

	public string BaseInfoToString()
	{
		var tr = transform;
		StringBuilder stringBuilder = new StringBuilder("Dynamic Object: " + gameObject.name + " base information:\n");
		stringBuilder.AppendLine("Position: " + tr.position);
		stringBuilder.AppendLine("Rotation: " + -tr.rotation.eulerAngles.z);
		stringBuilder.AppendLine("ScaleX: " + tr.lossyScale.x + "\t" + "ScaleY: " + tr.lossyScale.y);

		return stringBuilder.ToString();
	}
#endregion

}

// /*
// 	Allen
// 	2018/5/18
// 	allendk@foxmail.com
// */

using System.Runtime.Serialization;

[DataContract]
public class OutputScene
{
	[DataMember]
	public float width;
	[DataMember]
	public float height;
	[DataMember]
	public OutputConfig[] objects;
}

[DataContract]
public class OutputConfig
{
	[DataMember]
	public string type;
	[DataMember]
    public string physicType;
	[DataMember]
	public string name;
	[DataMember]
	public InitConfig init;
	[DataMember(EmitDefaultValue = false, IsRequired = false)]
	public TriggerConfig trigger;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public SpecialConfig special;
}

[DataContract]
public class InitConfig
{
	[DataMember]
	public float[] position;   
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float radius;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float width;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float height;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float thickness;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
    public float thickness2;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float rotation;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float mass;
    //public float[] velocity;
    //public float? angularVelocity;
}

[DataContract]
public class TriggerConfig
{
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float distance;
	[DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float interval;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float[] velocity;
	[DataMember(EmitDefaultValue = false, IsRequired = false)]
    public float[] randomVelocityRange;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float angularVelocity;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
    public float randomAngularVelocityRange;
}

[DataContract]
public class SpecialConfig
{
	[DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float gap;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public bool ropeHang;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float lineLength;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public int ropeBallCount;
    [DataMember(EmitDefaultValue = false, IsRequired = false)]
	public float ballRadius;
}

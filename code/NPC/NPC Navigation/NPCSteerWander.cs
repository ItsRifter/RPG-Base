using Sandbox;

public class NPCSteerWander : NPCSteering
{
	public float MinRadius { get; set; } = 50;
	public float MaxRadius { get; set; } = 500;

	public NPCSteerWander()
	{

	}

	public override void Tick( Vector3 position )
	{
		base.Tick( position );

		if ( Path.IsEmpty )
		{
			FindNewTarget( position );
		}
	}

	public virtual bool FindNewTarget( Vector3 center )
	{
		var t = NavMesh.GetPointWithinRadius( center, MinRadius, MaxRadius );
		if ( t.HasValue )
		{
			Target = t.Value;
		}

		return t.HasValue;
	}

}

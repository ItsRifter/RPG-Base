using System;
using Sandbox;

public class RPGCamera : CameraMode
{

	[ConVar.Replicated]
	public static bool thirdperson_collision { get; set; } = true;

	private Angles orbitAngles;

	private float minZoom = -90;
	private float maxZoom = 25;

	private float zoom = 0;

	public override void Update()
	{
		var pawn = Local.Pawn as AnimEntity;
		var client = Local.Client;

		if ( pawn == null )
			return;

		Position = pawn.Position;
		Vector3 targetPos;

		var center = pawn.Position + Vector3.Up * 64;

		Position = center;
		Rotation = Rotation.FromAxis( Vector3.Up, 4 ) * Input.Rotation;

		float distance = 130.0f + zoom * pawn.Scale;

		targetPos = Position + Input.Rotation.Right * (pawn.CollisionBounds.Maxs.x * pawn.Scale);
		targetPos += Input.Rotation.Forward * -distance;
		
		if ( thirdperson_collision )
		{
			var tr = Trace.Ray( Position, targetPos )
				.Ignore( pawn )
				.Radius( 8 )
				.Run();

			Position = tr.EndPosition;
		}
		else
		{
			Position = targetPos;
		}

		FieldOfView = 70;

		Viewer = null;
	}

	public override void BuildInput( InputBuilder input )
	{
		zoom -= input.MouseWheel * 5;

		zoom = Math.Clamp( zoom, minZoom, maxZoom );

		orbitAngles.yaw += input.AnalogLook.yaw;
		orbitAngles.pitch += input.AnalogLook.pitch;
		orbitAngles = orbitAngles.Normal;
		orbitAngles.pitch = orbitAngles.pitch.Clamp( -89, 89 );

		base.BuildInput( input );
	}
}

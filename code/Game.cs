
namespace Sandbox
{
	public class Cabinet : ModelEntity
	{
		public float Curvature => 0.9f;
		public float Depth => 0.0f;

		public Cabinet()
		{
		}

		public Cabinet( string modelName ) : base( modelName )
		{
		}

		public override void OnNewModel( Model model )
		{
			base.OnNewModel( model );

			UpdateAttributes();
		}

		[Event.Tick.Server]
		public void Tick()
		{
			Position = Vector3.Forward * 0;
			Rotation = Rotation.FromRoll( 60 );
		}

		[Event.Frame]
		public void OnFrame()
		{
			UpdateAttributes();
		}

		private void UpdateAttributes()
		{
			if ( SceneObject.IsValid() )
			{
				float scale = 10.0f.LerpTo( 0.25f, Curvature );
				float radius = 128.0f * scale;
				float offset = radius + Depth;
				Vector2 size = 128.0f;

				SceneObject.Attributes.Set( "radius", radius );
				SceneObject.Attributes.Set( "origin", Transform.Position + Transform.Rotation.Down * offset );
				SceneObject.Attributes.Set( "right", Transform.Rotation.Forward );
				SceneObject.Attributes.Set( "up", Transform.Rotation.Right );
				SceneObject.Attributes.Set( "screenres", new Vector4( size.x, size.y, scale, scale ) );
			}
		}
	}

	public partial class MyGame : Game
	{
		public MyGame()
		{
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var pawn = new Pawn
			{
				Transform = new Transform( Vector3.Right * 200, Rotation.From( 0, 0, 0 ) )
			};

			client.Pawn = pawn;
		}

		public override void PostLevelLoaded()
		{
			Map.Scene.AmbientLightColor = Color.White;

			_ = new PostProcessingEntity
			{
				PostProcessingFile = "postprocess/standard.vpost"
			};

			_ = new Cabinet( "models/screen.vmdl" )
			{

			};
		}
	}
}

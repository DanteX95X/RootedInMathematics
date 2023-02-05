using Godot;

namespace RootedInMathematics.Scripts.UI
{
	public class ButtonSounds : Button
	{
		private AudioStreamPlayer2D sceneAudio = null;
		
		public override void _Ready()
		{
			sceneAudio = GetNode<AudioStreamPlayer2D>("ButtonAudio");
		}

		public override void _Pressed()
		{
			sceneAudio.Play();
		}
	}
}

using Godot;
using System;

public class PlayerCharacter : Sprite
{
	private AnimationPlayer animationPlayer;
	
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Distortion");
	}

	public void PlayWinAnimation()
	{
		animationPlayer.Play("Shrinking");
	}
}

using Godot;
using System;

public class PlayerCharacter : Sprite
{
	public override void _Ready()
	{
		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Distortion");
	}
}

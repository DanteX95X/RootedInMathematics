using Godot;
using System;

public class PlayerCharacter : Sprite
{
	private AnimationPlayer animationPlayer;
	private AudioStreamPlayer2D winSound;
	private AudioStreamPlayer2D moveSound;
	
	public override void _Ready()
	{
		moveSound = GetNode<AudioStreamPlayer2D>("MoveSound");
		winSound = GetNode<AudioStreamPlayer2D>("WinSound");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Distortion");
	}

	public void OnWin()
	{
		animationPlayer.Play("Shrinking");
		winSound.Play();
	}

	public void OnMove()
	{
		moveSound.Play();
	}
}

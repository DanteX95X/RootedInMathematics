using Godot;
using System;

public class InGameMenu : CanvasLayer
{
	public void OnReplayButtonPressed()
	{
		GD.Print("Pressed");
		GetTree().ChangeScene("res://Scenes/Game.tscn");
	}

	public void OnResumeButtonPressed()
	{
		GD.Print("Resuming game");
		Visible = false;
	}

	public void OnMenuButtonPressed()
	{
		GD.Print("Back to menu");
	}

	public void DisableResumeButton()
	{
		GetNode<Button>("Control/Grid/ResumeButton").Visible = false;
	}
}

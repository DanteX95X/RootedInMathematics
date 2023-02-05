using Godot;
using System;

public class InGameMenu : CanvasLayer
{
	public void OnReplayButtonPressed()
	{
		GD.Print("Replay");
		GetTree().ReloadCurrentScene();
	}

	public void OnResumeButtonPressed()
	{
		GD.Print("Resuming game");
		Visible = false;
	}

	public void OnMenuButtonPressed()
	{
		GD.Print("Back to menu");
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}

	public void DisableResumeButton()
	{
		GetNode<Button>("Control/Grid/ResumeButton").Visible = false;
	}
}

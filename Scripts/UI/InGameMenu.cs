using Godot;
using System;

public class InGameMenu : CanvasLayer
{
	private const float timerLength = 0.2f;
	public async void OnReplayButtonPressed()
	{
		GD.Print("Replay");
		await ToSignal(GetTree().CreateTimer(timerLength), "timeout");
		GetTree().ReloadCurrentScene();
	}

	public void OnResumeButtonPressed()
	{
		GD.Print("Resuming game");
		Visible = false;
	}

	public async void OnMenuButtonPressed()
	{
		GD.Print("Back to menu");
		await ToSignal(GetTree().CreateTimer(timerLength), "timeout");
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}

	public void DisableResumeButton()
	{
		GetNode<Button>("Control/Grid/ResumeButton").Visible = false;
	}
}

using Godot;
using System;

public class MainMenu : CanvasLayer
{
	public void OnPlayButtonPressed()
	{
		GetTree().ChangeScene("res://Scenes/Game.tscn");
	}

	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}

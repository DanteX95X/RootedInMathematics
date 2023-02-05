using Godot;
using System;

public class ReplayButton : Button
{

	public void OnReplayButtonPressed()
	{
		GD.Print("Pressed");
		GetTree().ChangeScene("res://Scenes/Game.tscn");
	}
}

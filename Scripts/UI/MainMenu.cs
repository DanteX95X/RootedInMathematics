using Godot;

public class MainMenu : CanvasLayer
{
	public void OnEasyButtonPressed()
	{
		GetTree().ChangeScene("res://Scenes/GameEasy.tscn");
	}
	
	public void OnMediumButtonPressed()
	{
		GetTree().ChangeScene("res://Scenes/GameMedium.tscn");
	}
	
	public void OnHardButtonPressed()
	{
		GetTree().ChangeScene("res://Scenes/GameHard.tscn");
	}

	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}

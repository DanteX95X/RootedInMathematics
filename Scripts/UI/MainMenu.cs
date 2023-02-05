using Godot;

public class MainMenu : CanvasLayer
{
	private const float timerLength = 0.2f;
	
	public void OnEasyButtonPressed()
	{
		ChangeScene("res://Scenes/GameEasy.tscn");
	}
	
	public void OnMediumButtonPressed()
	{
		ChangeScene("res://Scenes/GameMedium.tscn");
	}
	
	public void OnHardButtonPressed()
	{
		ChangeScene("res://Scenes/GameHard.tscn");
	}

	public async void OnQuitButtonPressed()
	{
		await ToSignal(GetTree().CreateTimer(timerLength), "timeout");
		GetTree().Quit();
	}

	public async void ChangeScene(string scenePath)
	{
		await ToSignal(GetTree().CreateTimer(timerLength), "timeout");
		GetTree().ChangeScene(scenePath);
	}
}

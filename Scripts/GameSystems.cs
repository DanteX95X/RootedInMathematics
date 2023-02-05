using Godot;
using System;

public class GameSystems : Node
{
	private InputSystem inputSystem;
	private GameLogic gameLogic;

	private bool isInitialized = false;

	public GameLogic GameLogic => gameLogic;
	public InputSystem InputSystem => inputSystem;

	[Export]
	private NumberType numberType = NumberType.Integers;
	
	public override void _Ready()
	{
		inputSystem = GetNode<InputSystem>("InputSystem");
		gameLogic = GetNode<GameLogic>("GameLogic");
		gameLogic.numberType = numberType;

		inputSystem.OnSelectChildAction += ProcessInput;
	}

	public override void _Process(float delta)
	{
		if (!isInitialized)
		{
			isInitialized = true;
			gameLogic.InitializeGame();
		}
	}

	private void ProcessInput(int childIndex)
	{
		gameLogic.MoveToChild(childIndex);
	}
}

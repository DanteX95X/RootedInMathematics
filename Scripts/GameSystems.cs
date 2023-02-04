using Godot;
using System;

public class GameSystems : Node
{
	private InputSystem inputSystem;
	private GameLogic gameLogic;
	
	public override void _Ready()
	{
		inputSystem = GetNode<InputSystem>("InputSystem");
		gameLogic = GetNode<GameLogic>("GameLogic");

		inputSystem.OnSelectChildAction += ProcessInput;
	}

	private void ProcessInput(int childIndex)
	{
		gameLogic.MoveToChild(childIndex);
	}
}

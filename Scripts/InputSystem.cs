using Godot;
using System;
using System.Collections.Generic;

public class InputSystem : Node
{
	public event Action<int> OnSelectChildAction;

	private List<(string actionName, int childIndex)> actionBindings =
		new List<(string actionName, int childIndex)>() {("left", 0), ("down", 1), ("right", 2)};

	private float timeCounter = 0;
	
	public override void _Ready()
	{
	
	}
	
	public override void _Process(float delta)
	{
		timeCounter -= delta;
		if (timeCounter > 0)
		{
			return;
		}

		foreach (var (actionName, childIndex) in actionBindings)
		{
			if (Input.IsActionJustReleased(actionName))
			{
				OnSelectChildAction?.Invoke(childIndex);
				timeCounter = 1;
			}
		}
	}
}

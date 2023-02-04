using Godot;
using System;
using System.Collections.Generic;

public class InputSystem : Node
{
	public event Action<int> OnSelectChildAction;

	private List<(string actionName, int childIndex)> actionBindings = new() {("left", 0), ("down", 1), ("right", 2)};
	
	public override void _Ready()
	{
	
	}
	
	public override void _Process(float delta)
	{
		foreach (var (actionName, childIndex) in actionBindings)
		{
			if (Input.IsActionJustReleased(actionName))
			{
				OnSelectChildAction?.Invoke(childIndex);
			}
		}
	}
}

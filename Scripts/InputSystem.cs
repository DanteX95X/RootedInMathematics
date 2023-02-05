using Godot;
using System;
using System.Collections.Generic;

public class InputSystem : Node
{
	public event Action<int> OnSelectChildAction;
	public event Action OnPause;

	private List<(string actionName, int childIndex)> actionBindings =
		new List<(string actionName, int childIndex)>() {("left", 0), ("down", 1), ("right", 2)};

	private bool inputAllowed = true;
	
	public override void _Ready()
	{
	
	}
	
	public override void _Process(float delta)
	{
		if (Input.IsActionJustReleased("ui_cancel"))
		{
			OnPause?.Invoke();
		}
		
		if (inputAllowed)
		{
			foreach (var (actionName, childIndex) in actionBindings)
			{
				if (Input.IsActionJustReleased(actionName))
				{
					OnSelectChildAction?.Invoke(childIndex);
					inputAllowed = false;
				}
			}
		}
	}

	public void NotifyInput()
	{
		inputAllowed = true;
	}
}

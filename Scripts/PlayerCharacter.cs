using Godot;
using System;

public class PlayerCharacter : Sprite
{
	private Vector2 previousLookingPosition = Vector2.Up;

	private SceneTreeTween tween;
	
	
	public override void _Ready()
	{
	}

	public void RequestPositionChange(Vector2 newPosition)//, Vector2 lookingPosition, float angle)
	{
		tween = CreateTween();
		tween.TweenProperty(this, "position", newPosition, 1f);
		//tween.TweenMethod(this, "look_at", previousLookingPosition, lookingPosition, 0.2f);
		//tween.TweenProperty(this, "rotation", Rotation + angle, 0.2f);
		//tween.Play();
		//previousLookingPosition = lookingPosition;
	}

	public bool IsMoving()
	{
		if (tween != null)
		{
			return tween.IsRunning();
		}

		return false;
	}
	
	
}

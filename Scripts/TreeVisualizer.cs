using System.Collections.Generic;
using Godot;
using System.Globalization;
using RootedInMathematics.Scripts;

public class TreeVisualizer : Node2D
{
	[Export]
	private PackedScene treeNodeScene = null;

	private GameSystems gameSystems;
	private Node2D playerCharacter;
	private Camera2D camera;
	private Vector2 cachedPosition = Vector2.Zero;
	private Dictionary<TreeNode, Node2D> modelViewMapping = new Dictionary<TreeNode, Node2D>();

	public override void _Ready()
	{
		GD.Print(GetPath());
		gameSystems = GetNode<GameSystems>("/root/Root/GameSystems");
		gameSystems.GameLogic.OnMoveToNode += OnMovedToNode;

		playerCharacter = GetNode<Node2D>("PlayerCharacter");
		camera = playerCharacter.GetNode<Camera2D>("Camera");
	}

	private void OnMovedToNode(TreeNode destination, TreeNode source)
	{
		Vector2 lookingPosition = Vector2.Up;
		if (!modelViewMapping.TryGetValue(destination, out Node2D destinationView))
		{
			destinationView = (Node2D) treeNodeScene.Instance();
			AddChild(destinationView);
			var textNode = destinationView.GetNode<Label>("Container/Text");
			if (destination.NodeValue != null)
			{
				textNode.Text = destination.NodeValue.value.ToString(CultureInfo.CurrentCulture);
			}
			
			if (source == null || !modelViewMapping.TryGetValue(source, out Node2D sourceView))
			{
				destinationView.Position = Vector2.Zero;
			}
			else
			{
				sourceView.Visible = false;
				int childIndex = source.Children.FindIndex(data => data.node == destination);
				
				Vector2 displacement = Vector2.Down * 200;
				if (source.Parent != null && modelViewMapping.TryGetValue(source.Parent, out Node2D sourceParentView))
				{
					displacement = sourceView.Position - sourceParentView.Position;
				}

				Vector2 currentDisplacement = displacement.Rotated(-1 * (childIndex - 1));
				destinationView.Position = sourceView.Position + currentDisplacement;

				lookingPosition = sourceView.Position;
			}
			
			modelViewMapping.Add(destination, destinationView);
			// destinationView.LookAt(lookingPosition);
			// destinationView.Rotate(1.5708f);
		}

		destinationView.Visible = true;
		playerCharacter.Position = destinationView.Position;
		playerCharacter.LookAt(lookingPosition);
		destinationView.Rotation = playerCharacter.Rotation;
		destinationView.Rotate(1.5708f);
		
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
	}
}

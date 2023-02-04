using System.Collections.Generic;
using Godot;
using System.Linq;
using RootedInMathematics.Scripts;

public class TreeVisualizer : Node2D
{
	[Export]
	private PackedScene treeNodeScene = null;

	[Export]
	private PackedScene edgeScene = null;

	[Export]
	private PackedScene edgeLabelScene = null;

	private GameSystems gameSystems;
	private Node2D playerCharacter;
	private Camera2D camera;
	private Vector2 cachedPosition = Vector2.Zero;
	private Dictionary<TreeNode, Node2D> modelViewMapping = new Dictionary<TreeNode, Node2D>();
	private const float distance = 500;
	private const float rightAngle = 1.5708f;

	private Queue<(TreeNode destination, TreeNode source)> visualizationQueue =
		new Queue<(TreeNode destination, TreeNode source)>();

	private Queue<Node2D> visibleNodes = new Queue<Node2D>();

	public override void _Ready()
	{
		GD.Print(GetPath());
		gameSystems = GetNode<GameSystems>("/root/Root/GameSystems");
		gameSystems.GameLogic.OnMoveToNode += QueueUpMovement;

		playerCharacter = GetNode<Node2D>("PlayerCharacter");
		camera = playerCharacter.GetNode<Camera2D>("Camera");
	}

	public override void _Process(float delta)
	{
		if (visualizationQueue.Any())
		{
			var data = visualizationQueue.Dequeue();
			OnMovedToNode(data.destination, data.source);
		}
	}
	
	private void QueueUpMovement(TreeNode nextNode, TreeNode previousNode)
	{
		visualizationQueue.Enqueue((nextNode, previousNode));
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
				textNode.Text = destination.NodeValue.value.ToString();
			}
			
			if (source == null || !modelViewMapping.TryGetValue(source, out Node2D sourceView))
			{
				destinationView.Position = Vector2.Zero;
			}
			else
			{
				//sourceView.Visible = false;
				lookingPosition = sourceView.Position;
				int childIndex = source.Children.FindIndex(data => data.node == destination);

				Vector2 displacement = Vector2.Down * distance;
				if (source.Parent != null &&
					modelViewMapping.TryGetValue(source.Parent, out Node2D sourceParentView))
				{
					displacement = sourceView.Position - sourceParentView.Position;
				}

				Vector2 currentDisplacement = displacement.Rotated(-1 * (childIndex - 1));
				destinationView.Position = sourceView.Position + currentDisplacement;
			}
			
			SpawnEdges(destinationView, destination);
			modelViewMapping.Add(destination, destinationView);
			//destinationView.Visible = true;
			playerCharacter.Position = destinationView.Position;
			playerCharacter.LookAt(lookingPosition);
			destinationView.Rotation = playerCharacter.Rotation;
			destinationView.Rotate(rightAngle);
		}
		else
		{
			Node2D sourceView = modelViewMapping[source];
			// sourceView.Visible = false;
			// destinationView.Visible = true;
			playerCharacter.Position = destinationView.Position;
			playerCharacter.Rotation = destinationView.Rotation;
			playerCharacter.Rotate(-rightAngle);
		}
		
		while (visibleNodes.Count > 3)
		{
			var disappearingView = visibleNodes.Dequeue();
			disappearingView.Visible = false;
		}
		
		if (destination.Parent != null)
		{
			var parentView = modelViewMapping[destination.Parent];
			if (!visibleNodes.Contains(parentView))
			{
				visibleNodes.Enqueue(parentView);
			}
		}
		visibleNodes.Enqueue(destinationView);
		
		foreach (var node in visibleNodes)
		{
			node.Visible = true;
		}
	}

	private void SpawnEdges(Node2D destinationView, TreeNode destination)
	{
		var normalizedDisplacement = Vector2.Down;
		for (int i = 0; i < destination.Children.Count; ++i)
		{
			var newDisplacement = normalizedDisplacement.Rotated(-1 * (i - 1)) * (distance / 2);
			Node2D edge = (Node2D) edgeScene.Instance();
			destinationView.AddChild(edge);
			edge.Position = Vector2.Zero;
			edge.Translate(newDisplacement);

			var sprite = (Sprite) destinationView;
			edge.Scale = new Vector2(1, distance * 4/ sprite.GetRect().Size.y);
			edge.LookAt(destinationView.Position);
			edge.Rotate(rightAngle);

			var edgeLabel = (Node2D) edgeLabelScene.Instance();
			destinationView.AddChild(edgeLabel);
			edgeLabel.Position = Vector2.Zero;
			edgeLabel.Translate(newDisplacement);
			var text = edgeLabel.GetNode<Label>("Container/Text");
			text.Text = destination.Children[i].edgeValue.value.ToString();
		}
	}
}

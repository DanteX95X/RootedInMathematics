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

	[Export]
	private PackedScene backOnTrackNodeScene = null;

	[Export]
	private PackedScene winningNodeScene = null;

	private GameSystems gameSystems;
	private PlayerCharacter playerCharacter;
	private Dictionary<TreeNode, Node2D> modelViewMapping = new Dictionary<TreeNode, Node2D>();
	private const float distance = 500;
	private const float rightAngle = 1.5708f;

	private Queue<(TreeNode destination, TreeNode source)> visualizationQueue =
		new Queue<(TreeNode destination, TreeNode source)>();

	private Queue<Node2D> visibleNodes = new Queue<Node2D>();
	private const int allowedVisibles = 2;

	private SceneTreeTween playerTween = null;

	public override void _Ready()
	{
		GD.Print(GetPath());
		gameSystems = GetNode<GameSystems>("/root/Root/GameSystems");
		gameSystems.GameLogic.OnMoveToNode += QueueUpMovement;
		gameSystems.GameLogic.OnWin += OnWin;

		playerCharacter = GetNode<PlayerCharacter>("PlayerCharacter");
	}

	private void OnWin()
	{
		playerCharacter.PlayWinAnimation();
		var winMenu = GetNode<CanvasLayer>("WinMenu");
		winMenu.Visible = true;
	}

	public override void _Process(float delta)
	{
		if (visualizationQueue.Any() && (playerTween == null || !playerTween.IsRunning()))
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
			if (gameSystems.GameLogic.IsWon)
			{
				destinationView = (Node2D) winningNodeScene.Instance();
			}
			else if (destination.NodeValue != null)
			{
				destinationView = (Node2D) treeNodeScene.Instance();
			}
			else
			{
				destinationView = (Node2D) backOnTrackNodeScene.Instance();
			}

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
			var cachedPosition = playerCharacter.Position;
			var cachedRotation = playerCharacter.Rotation;
			playerCharacter.Position = destinationView.Position;
			playerCharacter.LookAt(lookingPosition);
			destinationView.Rotation = playerCharacter.Rotation;
			destinationView.Rotate(rightAngle);
			playerCharacter.Position = cachedPosition;
			var targetRotation = playerCharacter.Rotation;
			playerCharacter.Rotation = cachedRotation;

			playerTween = playerCharacter.CreateTween();
			playerTween.TweenProperty(playerCharacter, "rotation", targetRotation, 0.2f);
			playerTween.TweenProperty(playerCharacter, "position", destinationView.Position, 1.0f);
		}
		else
		{
			Node2D sourceView = modelViewMapping[source];
			var cachedPosition = playerCharacter.Position;
			var cachedRotation = playerCharacter.Rotation;
			playerCharacter.Position = destinationView.Position;
			playerCharacter.LookAt(sourceView.Position);
			var midwayRotation = playerCharacter.Rotation;
			playerCharacter.Rotation = destinationView.Rotation;
			playerCharacter.Rotate(-rightAngle);
			playerCharacter.Position = cachedPosition;
			var targetRotation = playerCharacter.Rotation;
			playerCharacter.Rotation = cachedRotation;
			
			playerTween = playerCharacter.CreateTween();
			playerTween.TweenProperty(playerCharacter, "rotation", midwayRotation, 0.2f);
			playerTween.TweenProperty(playerCharacter, "position", destinationView.Position, 1.0f);
			if (source.NodeValue != null)
			{
				playerTween.TweenProperty(playerCharacter, "rotation", targetRotation, 0.2f);
			}
		}
		
		UpdateVisibleNodes(destination, destinationView);
	}

	private void UpdateVisibleNodes(TreeNode destination, Node2D destinationView)
	{
		foreach (var node in visibleNodes)
		{
			node.GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
		}
		
		PruneRedundantVisibleNodes();

		if (destination.Parent != null)
		{
			var parentView = modelViewMapping[destination.Parent];
			if (!visibleNodes.Contains(parentView))
			{
				visibleNodes.Enqueue(parentView);
			}
		}

		PruneRedundantVisibleNodes();
		visibleNodes.Enqueue(destinationView);

		foreach (var node in visibleNodes)
		{
			node.Visible = true;
			node.GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("FadeOut");
		}
	}

	private void PruneRedundantVisibleNodes()
	{
		while (visibleNodes.Count > allowedVisibles)
		{
			var disappearingView = visibleNodes.Dequeue();
			disappearingView.Visible = false;
		}
	}

	private void SpawnEdges(Node2D destinationView, TreeNode destination)
	{
		var normalizedDisplacement = Vector2.Down;
		for (int i = 0; i < destination.Children.Count; ++i)
		{
			var newDisplacement = normalizedDisplacement.Rotated(-1 * (i - 1));// * (distance / 2);
			Node2D edge = (Node2D) edgeScene.Instance();
			destinationView.AddChild(edge);
			edge.Position = Vector2.Zero;
			edge.Translate(newDisplacement);

			var sprite = (Sprite) destinationView;
			edge.LookAt(destinationView.Position);
			edge.Rotate(rightAngle);

			var edgeLabel = (Node2D) edgeLabelScene.Instance();
			destinationView.AddChild(edgeLabel);
			edgeLabel.Position = Vector2.Zero;
			edgeLabel.Translate(newDisplacement * distance*0.4f);
			var text = edgeLabel.GetNode<Label>("Container/Text");
			text.Text = destination.Children[i].edgeValue.value.ToString();

			var tween = edge.CreateTween();
			var targetScale = new Vector2(0.1f, 0.1f);//distance * 4/ sprite.GetRect().Size.y);
			tween.TweenProperty(edge, "scale", targetScale, 0.5f);
		}
	}
}

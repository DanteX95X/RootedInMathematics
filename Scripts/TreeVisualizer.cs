using Godot;
using System;
using System.Globalization;
using RootedInMathematics.Scripts;

public class TreeVisualizer : Node
{
	[Export]
	private PackedScene treeNodeScene = null;

	private GameSystems gameSystems;

	private Node2D playerCharacter;
	
	private Vector2 cachedPosition = Vector2.Zero;
	
	public override void _Ready()
	{
		GD.Print(GetPath());
		gameSystems = GetNode<GameSystems>("/root/Root/GameSystems");
		gameSystems.GameLogic.OnNodeActivated += GameLogicOnOnNodeActivated;

		playerCharacter = GetNode<Node2D>("PlayerCharacter");
	}

	private void GameLogicOnOnNodeActivated(TreeNode currentNode)
	{
		var nodeView = (Node2D)treeNodeScene.Instance();
		AddChild(nodeView);
		nodeView.Position = cachedPosition;
		cachedPosition += new Vector2(0, 200);
		var textNode = nodeView.GetNode<Label>("Container/Text");
		textNode.Text = currentNode.NodeValue.value.ToString(CultureInfo.CurrentCulture);

		playerCharacter.Position = nodeView.Position;
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
	}
}

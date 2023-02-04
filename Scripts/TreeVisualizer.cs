using Godot;
using System;
using System.Globalization;
using RootedInMathematics.Scripts;

public class TreeVisualizer : Node
{
	[Export]
	private PackedScene treeNodeScene = null;

	private GameSystems gameSystems;
	
	public override void _Ready()
	{
		GD.Print(GetPath());
		gameSystems = GetNode<GameSystems>("/root/Root/GameSystems");
		gameSystems.GameLogic.OnNodeActivated += GameLogicOnOnNodeActivated;
		GameLogicOnOnNodeActivated(null);
	}

	private void GameLogicOnOnNodeActivated(TreeNode node)
	{
		var nodeView = (Node2D)treeNodeScene.Instance();
		AddChild(nodeView);
		nodeView.Position = new Vector2(0, 0);
		var textNode = nodeView.GetNode<Label>("Container/Text");
		if (node != null)
		{
			textNode.Text = node.NodeValue.value.ToString(CultureInfo.CurrentCulture);
		}
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
	}
}

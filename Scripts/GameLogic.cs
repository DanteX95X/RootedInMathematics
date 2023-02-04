using System;
using Godot;
using RootedInMathematics.Scripts;

public class GameLogic : Node
{
	private TreeNode root;
	private TreeNode currentNode;
	private Random random;

	public override void _Ready()
	{
		root = new TreeNode(null);
		random = new Random();
		currentNode = root;
		currentNode.OnActivate += ReactOnActivate;
		root.GenerateNode(random);
	}

	public override void _Process(float delta)
	{
	}

	public void ReactOnActivate(TreeNode node)
	{
		GD.Print($"node activated {node.NodeValue.value} from GameLogic");
	}

	public void MoveToChild(int childIndex)
	{
		if (currentNode.IsAnswerCorrect(childIndex))
		{
			GD.Print("Answer correct");
		}
		else
		{
			GD.Print("Answer incorrect");
		}
		
		GD.Print($"Moving to child {childIndex}");
		currentNode = currentNode.Children[childIndex].node;
		currentNode.GenerateNode(random);
	}
}

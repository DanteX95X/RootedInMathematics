using Godot;
using RootedInMathematics.Scripts;

public class GameLogic : Node
{
	private TreeNode root;
	private TreeNode currentNode;

	public override void _Ready()
	{
		root = new TreeNode(null, new Numeric(5f));
		currentNode = root;
		root.GenerateNode();
	}

	public override void _Process(float delta)
	{
	}
}

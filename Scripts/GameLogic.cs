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
		currentNode.OnActivate += ReactOnActivate;
		
		
		root.GenerateNode();
	}

	public override void _Process(float delta)
	{
	}

	public void ReactOnActivate(TreeNode node)
	{
		GD.Print($"node activated {node.NodeValue.value} from GameLogic");
	}
}

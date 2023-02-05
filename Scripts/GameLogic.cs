using System;
using System.Data;
using Godot;
using RootedInMathematics.Scripts;

public class GameLogic : Node
{
	private TreeNode root;
	private TreeNode currentNode;
	private Random random;

	[Export(PropertyHint.Range, "1,10,1")]
	private int winningDepth = 3;

	private int mistakesCounter = 0;
	
	public bool IsWon { get; private set; } = false;
	
	public event Action<TreeNode, TreeNode> OnMoveToNode;
	public event Action OnWin;

	public override void _Ready()
	{
		root = new TreeNode(null, 0);
		random = new Random(3);
		currentNode = root;
		currentNode.OnActivate += ReactOnActivate;
	}

	public void InitializeGame()
	{
		ActivateCurrentNode();
		OnMoveToNode?.Invoke(currentNode, null);
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
	}

	public void ReactOnActivate(TreeNode node)
	{
		GD.Print($"node activated {node.NodeValue.value} from GameLogic");
	}

	public void MoveToChild(int childIndex)
	{
		var previousNode = currentNode;
		previousNode.OnActivate -= ReactOnActivate;
		GD.Print($"Moving to child {childIndex}");
		currentNode = currentNode.Children[childIndex].node;
		currentNode.OnActivate += ReactOnActivate;

		if (previousNode.IsAnswerCorrect(childIndex))
		{
			GD.Print("Answer correct");
			if (mistakesCounter == 0)
			{
				if (!CheckWinCondition())
				{
					ActivateCurrentNode();
				}
				OnMoveToNode?.Invoke(currentNode, previousNode);
			}
			else
			{
				GD.Print("Up the tree");
				OnMoveToNode?.Invoke(currentNode, previousNode);
				TraverseUpTheTree();
				--mistakesCounter;
			}
		}
		else
		{
			GD.Print("Answer incorrect");
			++mistakesCounter;
			ActivateCurrentNode();
			OnMoveToNode?.Invoke(currentNode, previousNode);
		}
	}

	void ActivateCurrentNode()
	{
		if (currentNode.NodeValue == null)
		{
			currentNode.GenerateNode(random);
		}
		currentNode.ActivateNode();
	}

	private bool CheckWinCondition()
	{
		if (currentNode.Depth >= winningDepth)
		{
			GD.Print("You won");
			OnWin?.Invoke();
			IsWon = true;

			return true;
		}

		return false;
	}

	private void TraverseUpTheTree()
	{
		var parent = currentNode.Parent;
		if (parent == null)
		{
			throw new ConstraintException("Parent is null when traversing up the tree");
		}
		
		OnMoveToNode?.Invoke(parent, currentNode);

		var parentOfParent = parent.Parent;
		if (parentOfParent == null)
		{
			throw new ConstraintException("Parent of parent is null when traversing up the tree");
		}

		OnMoveToNode?.Invoke(parentOfParent, parent);
		currentNode = parentOfParent;
		currentNode.ActivateNode();
	}
}

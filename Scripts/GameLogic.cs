using System;
using System.Data;
using Godot;
using RootedInMathematics.Scripts;

public class GameLogic : Node
{
	private TreeNode root;
	private TreeNode currentNode;
	private Random random;

	private int mistakesCounter = 0;

	public override void _Ready()
	{
		root = new TreeNode(null);
		random = new Random(3);
		currentNode = root;
		currentNode.OnActivate += ReactOnActivate;
		ActivateCurrentNode();
	}

	public void ReactOnActivate(TreeNode node)
	{
		GD.Print($"node activated {node.NodeValue.value} from GameLogic");
	}

	public void MoveToChild(int childIndex)
	{
		var previousNode = currentNode;
		GD.Print($"Moving to child {childIndex}");
		currentNode = currentNode.Children[childIndex].node;
		
		if (previousNode.IsAnswerCorrect(childIndex))
		{
			GD.Print("Answer correct");
			if (mistakesCounter == 0)
			{
				ActivateCurrentNode();
				CheckWinCondition();
			}
			else
			{
				TraverseUpTheTree();
				--mistakesCounter;
			}
		}
		else
		{
			GD.Print("Answer incorrect");
			++mistakesCounter;
			ActivateCurrentNode();
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

	private void CheckWinCondition()
	{
		GD.Print("You won");
	}

	private void TraverseUpTheTree()
	{
		var parent = currentNode.Parent;
		if (parent == null)
		{
			throw new ConstraintException("Parent is null when traversing up the tree");
		}

		var parentOfParent = parent.Parent;
		if (parentOfParent == null)
		{
			throw new ConstraintException("Parent of parent is null when traversing up the tree");
		}

		currentNode = parentOfParent;
		currentNode.ActivateNode();
	}
}

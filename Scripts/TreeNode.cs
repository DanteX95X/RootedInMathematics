using System.Collections.Generic;
using Godot;

namespace RootedInMathematics.Scripts
{
	public class TreeNode
	{
		private readonly TreeNode parent;
		private List<(TreeNode child, Numeric edgeValue)> children = new();
		private Numeric nodeValue;

		public TreeNode Parent => parent;
		public Numeric NodeValue => nodeValue;
		public List<(TreeNode child, Numeric edgeValue)> Children => children;

		public TreeNode(TreeNode parent, Numeric nodeValue)
		{
			this.parent = parent;
			this.nodeValue = nodeValue;
		}

		public void GenerateNode()
		{
			GD.Print($"node generated with value {nodeValue.value}");
			//TODO: generate children, node value, edge values
		}

		public void ActivateNode()
		{
			//TODO: set node as current;
		}
	}
}

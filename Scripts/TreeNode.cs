using System;
using System.Collections.Generic;
using Godot;

namespace RootedInMathematics.Scripts
{
	public class TreeNode
	{
		private readonly TreeNode parent;
		private List<(TreeNode node, Numeric edgeValue)> children = new();
		private Numeric nodeValue;
		private int correctChildIndex;

		public event Action<TreeNode> OnActivate; 

		public TreeNode Parent => parent;
		public Numeric NodeValue => nodeValue;
		public List<(TreeNode node, Numeric edgeValue)> Children => children;

		public TreeNode(TreeNode parent)
		{
			this.parent = parent;
		}

		public void GenerateNode(Random random)
		{
			SortedSet<float> generatedPossibilities = new();
			for (int i = 0; i < 3; ++i)
			{
				float childValue = 0;
				do
				{
					childValue = random.Next(1, 11);
				}
				while (generatedPossibilities.Contains(childValue));

				generatedPossibilities.Add(childValue);
				
				Numeric edgeValue = new(childValue);
				children.Add((new TreeNode(this), edgeValue));
			}
			correctChildIndex = random.Next(children.Count);
			Numeric value = children[correctChildIndex].edgeValue;
			nodeValue = value.ToPower(2);

			GD.Print($"node generated with value {nodeValue.value}");
		}

		public void ActivateNode()
		{
			GD.Print($"node activated with value {nodeValue.value}");
			string info = "Children: ";
			foreach (var child in children)
			{
				info += child.edgeValue.value.ToString() + ",";
			}
			GD.Print(info);
			OnActivate?.Invoke(this);
			//TODO: set node as current;
		}

		public bool IsAnswerCorrect(int index)
		{
			return correctChildIndex == index;
		}
	}
}

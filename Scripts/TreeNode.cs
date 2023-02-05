using System;
using System.Collections.Generic;
using System.Numerics;
using Godot;
using Microsoft.VisualBasic;

namespace RootedInMathematics.Scripts
{
	public class TreeNode
	{
		private readonly TreeNode parent;
		private readonly int depth;
		private readonly List<(TreeNode node, Numeric edgeValue)> children = new List<(TreeNode node, Numeric edgeValue)>();
		private Numeric nodeValue;
		private int correctChildIndex;
		private const int upperLimit = 101;

		public event Action<TreeNode> OnActivate; 

		public TreeNode Parent => parent;
		public Numeric NodeValue => nodeValue;
		public int Depth => depth;
		public List<(TreeNode node, Numeric edgeValue)> Children => children;

		public TreeNode(TreeNode parent, int depth)
		{
			this.parent = parent;
			this.depth = depth;
		}

		public void GenerateNode(Random random, NumberType numberType)
		{
			SortedSet<Complex> generatedPossibilities = new SortedSet<Complex>(Comparer<Complex>.Create((first, second) =>
			{
				if (first.Real.CompareTo(second.Real) == 0)
				{
					return first.Imaginary.CompareTo(second.Imaginary);
				}

				return first.Real.CompareTo(second.Real);
			}));
			for (int i = 0; i < 3; ++i)
			{
				Numeric childValue = new Numeric(0, 0);
				do
				{
					if (numberType == NumberType.Integers)
					{
						childValue = new Numeric(random.Next(1, upperLimit), 0);
					}
					else if (numberType == NumberType.Real)
					{
						childValue = new Numeric(Math.Round(random.NextDouble() + random.Next(1, upperLimit), 2), 0);
					}
					else if(numberType == NumberType.Complex)
					{
						childValue = new Numeric(random.Next(1, upperLimit), random.Next(1, upperLimit));
					}
				}
				while (generatedPossibilities.Contains(childValue.ToPower(2).complex));

				generatedPossibilities.Add(childValue.ToPower(2).complex);
				
				Numeric edgeValue = childValue;
				children.Add((new TreeNode(this, depth + 1), edgeValue));
			}
			correctChildIndex = random.Next(children.Count);
			Numeric value = children[correctChildIndex].edgeValue;
			nodeValue = value.ToPower(2);

			GD.Print($"node generated with value {nodeValue.complex}");
		}

		public void ActivateNode()
		{
			GD.Print($"node activated with value {nodeValue.complex}");
			string info = "Children: ";
			foreach (var child in children)
			{
				info += child.edgeValue.complex.ToString() + ",";
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

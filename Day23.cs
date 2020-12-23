using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day23
	{
		const string _inputFilename = @"Inputs\Day23.txt";

		const string _example = "389125467";

		[Test]
		public void SilverTest()
		{
			Assert.AreEqual("67384529", RunSilver(_example));
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual("72496583", RunSilver(FileHelpers.GetSingle(_inputFilename)));
		}

		[Test]
		public void GoldTest()
		{
			Assert.AreEqual(149245887792, RunGold(_example));
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(41785843847, RunGold(FileHelpers.GetSingle(_inputFilename)));
		}

		static string RunSilver(string input)
		{
			var nodeSet = new NodeSet(input, input.Length);
			Run(nodeSet, 100);

			var firstNode = nodeSet[1].Next;

			return string.Join(string.Empty, firstNode.AsEnumerable.Take(8));
		}

		static long RunGold(string input)
		{
			var nodeSet = new NodeSet(input, 1_000_000);
			Run(nodeSet, 10_000_000);

			var nodeOne = nodeSet[1];
			return (long) nodeOne.Next.Value * (long) nodeOne.Next.Next.Value;
		}

		static void Run(NodeSet nodeSet, int moves)
		{
			var currentNode = nodeSet.FirstNode;
			for (int i = 0; i < moves; i++)
			{
				var extractFirst = currentNode.Next;
				var extractLast = extractFirst.Next.Next;

				currentNode.Next = extractLast.Next;
				extractLast.Next.Previous = currentNode;

				extractLast.Next = null;
				extractFirst.Previous = null;
				
				var insertAfter = nodeSet.FindNode(currentNode.Value - 1, extractFirst);
				extractFirst.Previous = insertAfter;
				extractLast.Next = insertAfter.Next;
				insertAfter.Next.Previous = extractLast;
				insertAfter.Next = extractFirst;

				currentNode = currentNode.Next;
			}
		}
	}

	class Node
	{
		public int Value { get; }
		public Node? Next { get; set; }
		public Node? Previous { get; set; }

		public Node(int value)
		{
			Value = value;
		}

		public IEnumerable<int> AsEnumerable
		{
			get
			{
				for (var node = this; node != null; node = node.Next)
				{
					yield return node.Value;
				}
			}
		}
	}

	class NodeSet
	{
		readonly IReadOnlyList<Node> _nodes;

		public Node FirstNode { get; }

		public Node this[int value] => _nodes[value];

		public NodeSet(string input, int totalNodes)
		{
			var nodes = new Node[totalNodes + 1];

			Node? firstNode = null;
			Node? lastNode = null;
			int max = int.MinValue;
			foreach (var c in input)
			{
				int value = int.Parse(c.ToString());
				max = Math.Max(max, value);
				var newNode = new Node(value);
				nodes[value] = newNode;

				if (firstNode == null)
				{
					firstNode = lastNode = newNode;
				}
				else
				{
					newNode.Previous = lastNode;
					lastNode.Next = newNode;
					lastNode = newNode;
				}
			}

			for (int i = max + 1; i <= totalNodes; i++)
			{
				var newNode = new Node(i);
				nodes[i] = newNode;

				newNode.Previous = lastNode;
				lastNode.Next = newNode;
				lastNode = newNode;
			}

			lastNode.Next = firstNode;
			firstNode.Previous = lastNode;

			FirstNode = firstNode;
			_nodes = nodes;
		}

		public Node FindNode(int target, Node excludeNodes)
		{
			for (;;)
			{
				if (target == 0)
				{
					target = _nodes.Count - 1;
				}

				for (var node = excludeNodes;; node = node.Next)
				{
					if (node == null)
					{
						return _nodes[target];
					}

					if (node.Value == target)
					{
						break;
					}
				}

				target--;
			}
		}
	}
}
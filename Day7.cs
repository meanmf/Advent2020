using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day7
	{
		const string _inputFilename = @"Inputs\Day7.txt";

		const string _example = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";

		const string _example2 = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";

		[Test]
		public void SilverTest()
		{
			var allBags = ParseBags(FileHelpers.ReadAllLinesFromString(_example));

			var foundBags = new HashSet<string>();
			FindSilver("shiny gold", allBags, foundBags);
			
			Assert.AreEqual(4, foundBags.Count);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var allBags = ParseBags(FileHelpers.EnumerateLines(_inputFilename));

			var foundBags = new HashSet<string>();
			FindSilver("shiny gold", allBags, foundBags);
			
			Assert.AreEqual(259, foundBags.Count);
		}

		static void FindSilver(string findBag, IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> allBags, ISet<string> foundBags)
		{
			foreach (var child in allBags.Where(bag => bag.Value.ContainsKey(findBag)))
			{
				foundBags.Add(child.Key);
				FindSilver(child.Key, allBags, foundBags);
			}
		}

		[Test]
		public void GoldTest()
		{
			var allBags = ParseBags(FileHelpers.ReadAllLinesFromString(_example));
			var count = FindGold("shiny gold", allBags);
			Assert.AreEqual(32, count);

			allBags = ParseBags(FileHelpers.ReadAllLinesFromString(_example2));
			count = FindGold("shiny gold", allBags);
			Assert.AreEqual(126, count);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var allBags = ParseBags(FileHelpers.EnumerateLines(_inputFilename));
			var count = FindGold("shiny gold", allBags);
			Assert.AreEqual(45018, count);
		}

		static int FindGold(string target, IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> bags)
		{
			var bag = bags[target];
			var total = 0;
			foreach (var child in bag)
			{
				total += child.Value;

				total += (child.Value * FindGold(child.Key, bags));
			}

			return total;
		}

		static IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> ParseBags(IEnumerable<string> input)
		{
			var bags = new Dictionary<string, IReadOnlyDictionary<string, int>>();

			foreach (var line in input)
			{
				var thisBag = new Dictionary<string, int>();

				var tokens = line.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

				var container = tokens[0] + " " + tokens[1];
				if (tokens[4] != "no")
				{
					for (int i = 4; i < tokens.Length; i += 4)
					{
						var amount = int.Parse(tokens[i]);
						var contents = tokens[i + 1] + " " + tokens[i + 2];

						thisBag[contents] = amount;
					}
				}

				bags.Add(container, thisBag);
			}

			return bags;
		}
	}
}
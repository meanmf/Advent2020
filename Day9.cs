using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day9
	{
		const string _inputFilename = @"Inputs\Day9.txt";

		const string _example =
			@"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";

		[Test]
		public void SilverTest()
		{
			var result = FindSilver(FileHelpers.ReadAllLinesFromString(_example), 5);
			Assert.AreEqual(127, result);
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = FindSilver(FileHelpers.EnumerateLines(_inputFilename), 25);
			Assert.AreEqual(3199139634, result);
		}

		static long FindSilver(IEnumerable<string> input, int range)
		{
			int index = range;

			var elements = input.Select(long.Parse).ToArray();
			
			while (index < elements.Length - 1)
			{
				bool found = false;
				for (int i = index - range; i < index && !found; i++)
				{
					for (int j = i + 1; j < index && !found; j++)
					{
						if (elements[i] + elements[j] == elements[index])
						{
							found = true;
						}
					}
				}

				if (!found)
				{
					return elements[index];
				}

				index++;
			}

			throw new InvalidOperationException("Not found");
		}

		[Test]
		public void GoldTest()
		{
			var find = FindSilver(FileHelpers.ReadAllLinesFromString(_example), 5);
			var result = FindGold(FileHelpers.ReadAllLinesFromString(_example), find);
			
			Assert.AreEqual(62, result);

		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var find = FindSilver(FileHelpers.EnumerateLines(_inputFilename), 25);
			var result = FindGold(FileHelpers.EnumerateLines(_inputFilename), find);
			
			Assert.AreEqual(438559930, result);
		}

		static long FindGold(IEnumerable<string> input, long target)
		{
			var elements = input.Select(long.Parse).ToArray();
			
			for (int start = 0; start < elements.Length; start++)
			{
				long total = elements[start];
				int next = start;
				while (total < target)
				{
					total += elements[++next];
				}

				if (total == target)
				{
					var max = elements.Skip(start).Take(next - start).Max();
					var min = elements.Skip(start).Take(next - start).Min();

					return (min + max);
				}
			}

			throw new InvalidOperationException("Not found");
		}
	}
}

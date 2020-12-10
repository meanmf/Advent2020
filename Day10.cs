using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day10
	{
		const string _inputFilename = @"Inputs\Day10.txt";

		#region examples
		const string _example1 = @"16
10
15
5
1
11
7
19
6
12
4";
		const string _example2 = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";
		#endregion

		[Test]
		public void SilverTest()
		{
			Assert.AreEqual(35, RunSilver(FileHelpers.ReadAllLinesFromString(_example1)));
			Assert.AreEqual(220, RunSilver(FileHelpers.ReadAllLinesFromString(_example2)));
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);
			
			Assert.AreEqual(2376, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
		}

		static int RunSilver(IEnumerable<string> input)
		{
			var inputs = input.Select(int.Parse).ToList();
			inputs.Add(inputs.Max() + 3);
			inputs.Sort();

			var diffs = new int[4];
			int last = 0;
			foreach (int i in inputs)
			{
				diffs[i - last]++;
				last = i;
			}

			return (diffs[1] * diffs[3]);
		}

		[Test]
		public void GoldTest()
		{
			Assert.AreEqual(8, RunGold(FileHelpers.ReadAllLinesFromString(_example1)));
			Assert.AreEqual(19208, RunGold(FileHelpers.ReadAllLinesFromString(_example2)));
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(129586085429248, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
		}

		static long RunGold(IEnumerable<string> input)
		{
			var inputs = input.Select(long.Parse).ToList();
			inputs.Add(inputs.Max() + 3);
			inputs.Add(0);
			inputs.Sort();

			var counts = new long[inputs.Count];
			counts[0] = 1;

			for (int i = 0; i < inputs.Count; i++)
			{
				for (int j = i + 1; j < inputs.Count && inputs[j] <= inputs[i] + 3; j++)
				{
					counts[j] += counts[i];
				}
			}

			return counts.Last();
	}
	}
}

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day6
	{
		const string _inputFilename = @"Inputs\Day6.txt";

		const string _example = @"abc

a
b
c

ab
ac

a
a
a
a

b";

		[Test]
		public void SilverTest()
		{
			var answers = ParseAnswers(FileHelpers.ReadAllLinesFromString(_example));

			var total = answers.Sum(group => group.answers.Count);
			Assert.AreEqual(11, total);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var answers = ParseAnswers(FileHelpers.EnumerateLines(_inputFilename));

			var total = answers.Sum(group => group.answers.Count);
			Assert.AreEqual(7110, total);
		}

		[Test]
		public void GoldTest()
		{
			var answers = ParseAnswers(FileHelpers.ReadAllLinesFromString(_example));

			var total = answers.Sum(group => group.answers.Count(g => g.Value == group.groupSize));
			Assert.AreEqual(6, total);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var answers = ParseAnswers(FileHelpers.EnumerateLines(_inputFilename));

			var total = answers.Sum(group => group.answers.Count(g => g.Value == group.groupSize));
			Assert.AreEqual(3628, total);
		}

		static IEnumerable<(int groupSize, IReadOnlyDictionary<char, int> answers)> ParseAnswers(IEnumerable<string> input)
		{
			var answers = new List<(int, IReadOnlyDictionary<char, int>)>();

			var groupAnswers = new Dictionary<char, int>();
			int groupCount = 0;
			foreach (var line in input)
			{
				if (string.IsNullOrEmpty(line))
				{
					answers.Add((groupCount, groupAnswers));
					groupAnswers = new Dictionary<char, int>();
					groupCount = 0;
				}
				else
				{
					groupCount++;
					foreach (var c in line)
					{
						groupAnswers.TryGetValue(c, out var count);
						groupAnswers[c] = count + 1;
					}
				}
			}

			answers.Add((groupCount, groupAnswers));
			return answers;
		}
	}
}
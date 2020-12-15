using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day15
	{
		const string _inputFilename = @"Inputs\Day15.txt";

		const int _silverTurns = 2020;
		const int _goldTurns = 30_000_000;

		record Example(string Input, int SilverAnswer, int GoldAnswer);
		readonly Example[] _examples =
		{
			new("0,3,6", 436, 175594),
			new("1,3,2", 1, 2578),
			new("2,1,3", 10, 3544142),
			new("1,2,3", 27, 261214),
			new("2,3,1", 78, 6895259),
			new("3,2,1", 438, 18),
			new("3,1,2", 1836, 362),
		};
		
		[Test]
		public void SilverTest()
		{
			foreach (var example in _examples)
			{
				Assert.AreEqual(example.SilverAnswer, Run(example.Input, _silverTurns));
			}
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(447, Run(FileHelpers.GetSingle(_inputFilename), _silverTurns));
		}
		
		[Test]
		public void GoldTest()
		{
			foreach (var example in _examples)
			{
				Assert.AreEqual(example.GoldAnswer, Run(example.Input, _goldTurns));
			}
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(11721679, Run(FileHelpers.GetSingle(_inputFilename), _goldTurns));
		}

		static int Run(string input, int turnCount)
		{
			var inputs = input.Split(",").Select(int.Parse);
			var state = new Dictionary<int, int>();

			int turn = 1;
			foreach (int i in inputs)
			{
				state[i] = turn;
				turn++;
			}

			int nextSpoken = 0;
			for (; turn < turnCount; turn++)
			{

				int nextNext;
				if (state.TryGetValue(nextSpoken, out int previouslySeen))
				{
					nextNext = turn - previouslySeen;
				}
				else
				{
					nextNext = 0;
				}

				state[nextSpoken] = turn;
				nextSpoken = nextNext;
			}

			return nextSpoken;
		}
	}
}

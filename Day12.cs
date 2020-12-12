using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Advent2020
{
	class Day12
	{
		const string _inputFilename = @"Inputs\Day12.txt";

		const string _example =
			@"F10
N3
F7
R90
F11";

		[Test]
		public void SilverTest()
		{
			Assert.AreEqual(25, RunSilver(FileHelpers.ReadAllLinesFromString(_example)));
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(1221, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
		}

		[Test]
		public void GoldTest()
		{
			Assert.AreEqual(286, RunGold(FileHelpers.ReadAllLinesFromString(_example)));
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(59435, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
		}

		static int RunSilver(IEnumerable<string> input)
		{
			return Run(input, 1, 0, false);
		}

		static int RunGold(IEnumerable<string> input)
		{
			return Run(input, 10, -1, true);
		}

		static readonly IReadOnlyDictionary<char, (int x, int y)> _moves = new Dictionary<char, (int, int)>()
		{
			['N'] = (0, -1),
			['S'] = (0, 1),
			['E'] = (1, 0),
			['W'] = (-1, 0)
		};
		
		static int Run(IEnumerable<string> input, int dx, int dy, bool moveWaypoint)
		{
			int x = 0;
			int y = 0;

			foreach (var action in input)
			{
				var val = int.Parse(action.Substring(1));
				switch (action[0])
				{
					case 'N':
					case 'S':
					case 'E':
					case 'W':
						var (moveX, moveY) = _moves[action[0]];
						if (moveWaypoint)
						{
							dx += moveX * val;
							dy += moveY * val;
						}
						else
						{
							x += moveX * val;
							y += moveY * val;
						}

						break;
					case 'L':
						for (int i = 0; i < val / 90; i++)
						{
							(dx, dy) = TurnLeft(dx, dy);
						}

						break;
					case 'R':
						for (int i = 0; i < val / 90; i++)
						{
							(dx, dy) = TurnRight(dx, dy);
						}

						break;
					case 'F':
						x += dx * val;
						y += dy * val;
						break;
					default:
						throw new InvalidOperationException("Unknown command: " + action[0]);
				}
			}

			return Math.Abs(x) + Math.Abs(y);
		}

		static (int, int) TurnRight(int dx, int dy)
		{
			return (-dy, dx);
		}

		static (int, int) TurnLeft(int dx, int dy)
		{
			return (dy, -dx);
		}
	}
}

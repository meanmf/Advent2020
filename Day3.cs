using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day3
	{
		const string _inputFilename = @"Inputs\Day3.txt";

		#region Example Input
		const string _example = @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
		#endregion

		[Test]
		public void SilverTest()
		{
			var map = FileHelpers.ReadAllLinesFromString(_example).ToArray();
			var trees = CountTrees(map, 3, 1);

			Assert.AreEqual(7, trees);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var map = FileHelpers.EnumerateLines(_inputFilename).ToArray();
			var trees = CountTrees(map, 3, 1);

			Assert.AreEqual(211, trees);
		}

		[Test]
		public void GoldTest()
		{
			var map = FileHelpers.ReadAllLinesFromString(_example).ToArray();

			var a = CountTrees(map, 1, 1);
			var b = CountTrees(map, 3, 1);
			var c = CountTrees(map, 5, 1);
			var d = CountTrees(map, 7, 1);
			var e = CountTrees(map, 1, 2);

			Assert.AreEqual(336, a * b * c * d * e);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var map = FileHelpers.EnumerateLines(_inputFilename).ToArray();

			var a = CountTrees(map, 1, 1);
			var b = CountTrees(map, 3, 1);
			var c = CountTrees(map, 5, 1);
			var d = CountTrees(map, 7, 1);
			var e = CountTrees(map, 1, 2);

			Assert.AreEqual(3_584_591_857, a * b * c * d * e);
		}

		static long CountTrees(IReadOnlyList<string> map, int deltaX, int deltaY)
		{
			int x = 0;
			int y = 0;
			int trees = 0;
			int width = map[0].Length;

			while (y < map.Count)
			{
				if (map[y][x % width] != '.')
				{
					trees++;
				}

				x += deltaX;
				y += deltaY;
			}

			return trees;
		}
	}
}

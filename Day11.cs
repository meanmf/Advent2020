using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day11
	{
		const string _inputFilename = @"Inputs\Day11.txt";

		const string _example =
			@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

		[Test]
		public void SilverTest()
		{
			var result = Run(FileHelpers.ReadAllLinesFromString(_example), maxLook: 1, maxNeighbors: 4);
			Assert.AreEqual(37, result);
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = Run(FileHelpers.EnumerateLines(_inputFilename), maxLook: 1, maxNeighbors: 4);
			Assert.AreEqual(2164, result);
		}
		
		[Test]
		public void GoldTest()
		{
			var result = Run(FileHelpers.ReadAllLinesFromString(_example), maxLook: int.MaxValue, maxNeighbors: 5);
			Assert.AreEqual(26, result);
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = Run(FileHelpers.EnumerateLines(_inputFilename), maxLook: int.MaxValue, maxNeighbors: 5);
			Assert.AreEqual(1974, result);
		}

		static int Run(IEnumerable<string> input, int maxLook, int maxNeighbors)
		{
			var grid = input.ToArray();

			int width = grid[0].Length;
			int height = grid.Length;

			for (;;)
			{
				var newGrid = new string[height];

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						int neighbors =
							Look(grid, x, y, -1, 0, maxLook) +
							Look(grid, x, y, -1, -1, maxLook) +
							Look(grid, x, y, 0, -1, maxLook) +
							Look(grid, x, y, 1, -1, maxLook) +
							Look(grid, x, y, 1, 0, maxLook) +
							Look(grid, x, y, 1, 1, maxLook)+
							Look(grid, x, y, 0, 1, maxLook) +
							Look(grid, x, y, -1, 1, maxLook);
						
						if (grid[y][x] == '#' && neighbors >= maxNeighbors)
						{
							newGrid[y] += 'L';
						}
						else if (grid[y][x] == 'L' && neighbors == 0)
						{
							newGrid[y] += '#';
						}
						else
						{
							newGrid[y] += grid[y][x];
						}
					}
				}

				if (grid.SequenceEqual(newGrid))
				{
					return grid.Sum(row => row.Sum(c => c == '#' ? 1 : 0));
				}

				grid = newGrid;
			}
		}

		static int Look(string[] grid, int startX, int startY, int deltaX, int deltaY, int maxLook)
		{
			int x = startX;
			int y = startY;

			for (int i = 0; i < maxLook; i++)
			{
				x += deltaX;
				y += deltaY;

				if (x < 0 || x == grid[0].Length) return 0;
				if (y < 0 || y == grid.Length) return 0;

				if (grid[y][x] == '#')
				{
					return 1;
				}

				if (grid[y][x] == 'L')
				{
					return 0;
				}
			}

			return 0;
		}
	}
}

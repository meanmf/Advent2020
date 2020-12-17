using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Advent2020
{
	class Day17
	{
		const string _inputFilename = @"Inputs\Day17.txt";

		const string _example =
			@".#.
..#
###";

		[Test]
		public void SilverTest()
		{
			var result = RunGold(FileHelpers.ReadAllLinesFromString(_example), isHyper: false);
			Assert.AreEqual(112, result);
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunGold(FileHelpers.EnumerateLines(_inputFilename), isHyper: false);
			Assert.AreEqual(319, result);
		}
		
		[Test]
		public void GoldTest()
		{
			var result = RunGold(FileHelpers.ReadAllLinesFromString(_example), isHyper: true);
			Assert.AreEqual(848, result);
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunGold(FileHelpers.EnumerateLines(_inputFilename), isHyper: true);
			Assert.AreEqual(2324, result);
		}

		record HyperCube(int X, int Y, int Z, int W);

		static int RunGold(IEnumerable<string> input, bool isHyper)
		{
			HashSet<HyperCube> actives = new();

			int lineCount = 0;
			foreach (var line in input)
			{
				for (int i = 0; i < line.Length; i++)
				{
					if (line[i] == '#')
					{
						actives.Add(new HyperCube(i, lineCount, 0, 0));
					}
				}
				lineCount++;
			}

			for (int cycle = 0; cycle < 6; cycle++)
			{
				var newActives = new HashSet<HyperCube>();
				var (minX, minY, minZ, minW, maxX, maxY, maxZ, maxW) = FindBoundaries(actives, isHyper);
				
				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						for (int z = minZ; z <= maxZ; z++)
						{
							for (int w = minW; w <= maxW; w++)
							{
								var neighborCount = CountNeighbors(actives, x, y, z, w);

								var thisCube = new HyperCube(x, y, z, w);
								if (actives.Contains(thisCube))
								{
									if (neighborCount == 2 || neighborCount == 3)
									{
										newActives.Add(thisCube);
									}
								}
								else
								{
									if (neighborCount == 3)
									{
										newActives.Add(thisCube);
									}
								}
							}
						}
					}
				}

				actives = newActives;
			}

			return actives.Count;
		}
		
		static int CountNeighbors(IReadOnlySet<HyperCube> actives, int x, int y, int z, int w)
		{
			int count = 0;
			for (int xx = x - 1; xx <= x + 1; xx++)
			{
				for (int yy = y - 1; yy <= y + 1; yy++)
				{
					for (int zz = z - 1; zz <= z + 1; zz++)
					{
						for (int ww = w - 1; ww <= w + 1; ww++)
						{
							if (x == xx & y == yy && z == zz && w == ww) continue;

							if (actives.Contains(new HyperCube(xx, yy, zz, ww)))
							{
								count++;
							}
						}
					}
				}
			}

			return count;
		}

		static (int minX, int minY, int minZ, int minW, int maxX, int maxY, int maxZ, int maxW) FindBoundaries(
			IEnumerable<HyperCube> actives, bool isHyper)
		{
			int minX, maxX, minY, maxY, minZ, maxZ, minW, maxW;

			minX = minY = minZ = minW = int.MaxValue;
			maxX = maxY = maxZ = maxW = int.MinValue;

			foreach (var cube in actives)
			{
				minX = Math.Min(minX, cube.X);
				minY = Math.Min(minY, cube.Y);
				minZ = Math.Min(minZ, cube.Z);
				minW = Math.Min(minW, cube.W);

				maxX = Math.Max(maxX, cube.X);
				maxY = Math.Max(maxY, cube.Y);
				maxZ = Math.Max(maxZ, cube.Z);
				maxW = Math.Max(maxW, cube.W);
			}

			minX--;
			maxX++;
			minY--;
			maxY++;
			minZ--;
			maxZ++;
			minW--;
			maxW++;

			if (!isHyper)
			{
				minW = maxW = 0;
			}

			return (minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
		}
	}
}

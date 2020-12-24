using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day24
	{
		const string _inputFilename = @"Inputs\Day24.txt";

		const string _example = @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";

		[Test]
		public void SilverTest()
		{
			var black = RunSilver(FileHelpers.ReadAllLinesFromString(_example));
			Assert.AreEqual(10, black.Count);
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var black = RunSilver(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(300, black.Count);
		}
		
		[Test]
		public void GoldTest()
		{
			var result = RunGold(FileHelpers.ReadAllLinesFromString(_example));
			Assert.AreEqual(2208, result);
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunGold(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(3466, result);
		}
		
		static HashSet<(double, int)> RunSilver(IEnumerable<string> input)
		{
			var black = new HashSet<(double, int)>();
			
			foreach (var line in input)
			{
				var row = 0;
				var col = 0.0;
				foreach (var move in ParseMoves(line))
				{
					switch (move)
					{
						case "e":
							col++;
							break;
						case "w":
							col--;
							break;
						case "ne":
							col += .5;
							row--;
							break;
						case "nw":
							col -= .5;
							row--;
							break;
						case "se":
							col += .5;
							row++;
							break;
						case "sw":
							col -= .5;
							row++;
							break;
					}
				}
				
				if (black.Contains((col, row)))
				{
					black.Remove((col, row));
				}
				else
				{
					black.Add((col, row));
				}
			}

			return black;
		}

		static int RunGold(IEnumerable<string> input)
		{
			var blackTiles = RunSilver(input);
			
			for (int day = 0; day < 100; day++)
			{
				var newBlack = new HashSet<(double, int)>();

				foreach (var tile in blackTiles)
				{
					var neighborCount = Neighbors(tile).Sum(t => blackTiles.Contains(t) ? 1 : 0);
					if (neighborCount == 1 || neighborCount == 2)
					{
						newBlack.Add(tile);
					}

					foreach (var neighbor in Neighbors(tile))
					{
						if (!blackTiles.Contains(neighbor))
						{
							var nnCount = Neighbors(neighbor).Sum(t => blackTiles.Contains(t) ? 1 : 0);
							if (nnCount == 2)
							{
								newBlack.Add(neighbor);
							}
						}
					}
				}

				blackTiles = newBlack;
			}

			return blackTiles.Count;
		}
				
		static IEnumerable<string> ParseMoves(string input)
		{
			var cmd = string.Empty;
			foreach (char ch in input)
			{
				cmd += ch;
				switch (ch)
				{
					case 'e':
					case 'w':
						yield return cmd;
						cmd = string.Empty;
						break;
					case 'n':
					case 's':
						break;
				}
			}
		}

		static IEnumerable<(double, int)> Neighbors((double col, int row) tile)
		{
			yield return (tile.col - 1, tile.row);
			yield return (tile.col + 1, tile.row);
			yield return (tile.col - .5, tile.row - 1);
			yield return (tile.col - .5, tile.row + 1);
			yield return (tile.col + .5, tile.row - 1);
			yield return (tile.col + .5, tile.row + 1);
		}
	}
}

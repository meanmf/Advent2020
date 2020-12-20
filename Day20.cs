using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Advent2020
{
	class Day20
	{
		const string _inputFilename = @"Inputs\Day20.txt";

		const string _example =
			@"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";

		[Test]
		public void SilverTest()
		{
			var total = FindSilver(FileHelpers.ReadAllLinesFromString(_example));
			Assert.AreEqual(20899048083289, total);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var total = FindSilver(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(13983397496713, total);
		}

		static long FindSilver(IEnumerable<string> input)
		{
			var tiles = new TileCollection(input);

			long result = 1;

			foreach (var tile in tiles.CornerPieces)
			{
				result *= tile.Num;
			}

			return result;
		}

		[Test]
		public void GoldTest()
		{
			const int size = 3;
			var result = FindGold(size, FileHelpers.ReadAllLinesFromString(_example));
			Assert.AreEqual(273, result);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);
			
			const int size = 12;
			var result = FindGold(size, FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(2424, result);
		}

		static long FindGold(int size, IEnumerable<string> input)
		{
			var tiles = new TileCollection(input);

			var grid = new Tile[size, size];

			// Find a corner piece and orient it in the top left corner
			var cornerTile = tiles.CornerPieces.First();
			grid[0, 0] = cornerTile;

			if (tiles.SideMatches(cornerTile.TopId).Count > 1)
			{
				cornerTile.FlipVertical();
			}

			if (tiles.SideMatches(cornerTile.LeftId).Count > 1)
			{
				cornerTile.FlipHorizontal();
			}

			// Fill in the left column
			for (int y = 1; y < size; y++)
			{
				grid[y, 0] = tiles.FindAndOrientTileBelow(grid[y - 1, 0]);
			}

			// Fill in the rest of the grid
			for (int y = 0; y < size; y++)
			{
				for (int x = 1; x < size; x++)
				{
					grid[y, x] = tiles.FindAndOrientTileRight(grid[y, x - 1]);
				}
			}

			// Piece together the final image
			var image = new FinalImage(size * 8);
			for (int y = 0; y < size; y++)
			{
				for (int line = 1; line <= 8; line++)
				{
					var sb = new StringBuilder();
					for (int x = 0; x < size; x++)
					{
						sb.Append(grid[y, x].GetLineInner(line));
					}

					image.AddLine(sb.ToString());
				}
			}

			// Solution is total # count minus the number of characters taken up by monsters
			const int monsterSize = 15;
			var hashMarkCount = image.CountCharacter('#');
			var monsterCount = image.CountMonsters();
			return hashMarkCount - (monsterCount * monsterSize);
		}
	}

	class TileCollection
	{
		readonly Dictionary<int, Tile> _tiles = new();
		readonly Dictionary<string, List<int>> _sideMap = new();

		public TileCollection(IEnumerable<string> input)
		{
			ReadTiles(input);
			BuildSideMap();
		}

		public IEnumerable<Tile> CornerPieces
		{
			get
			{
				// find all pieces with two unique edges
				foreach (var tile in _tiles.Values)
				{
					int edgeCount = 0;
					foreach (var side in tile.EdgeIds)
					{
						if (SideMatches(side).Count == 1) edgeCount++;
					}

					if (edgeCount == 2)
					{
						yield return tile;
					}
				}
			}
		}

		public IReadOnlyList<int> SideMatches(string sideHash)
		{
			return _sideMap[sideHash];
		}

		public Tile FindAndOrientTileBelow(Tile topNeighbor)
		{
			var nextTile = _tiles[SideMatches(topNeighbor.BottomId).Single(n => n != topNeighbor.Num)];
			if (nextTile.TopId != topNeighbor.BottomId)
			{
				if (nextTile.BottomId == topNeighbor.BottomId)
				{
					nextTile.FlipVertical();
				}
				else if (nextTile.RightId == topNeighbor.BottomId)
				{
					nextTile.RotateLeft();
				}
				else if (nextTile.LeftId == topNeighbor.BottomId)
				{
					nextTile.RotateRight();
				}
			}

			if (nextTile.TopHash != topNeighbor.BottomHash)
			{
				nextTile.FlipHorizontal();
			}

			return nextTile;
		}

		public Tile FindAndOrientTileRight(Tile leftNeighbor)
		{
			var nextTile = _tiles[SideMatches(leftNeighbor.RightId).Single(n => n != leftNeighbor.Num)];
			if (nextTile.LeftId != leftNeighbor.RightId)
			{
				if (nextTile.RightId == leftNeighbor.RightId)
				{
					nextTile.FlipHorizontal();
				}
				else if (nextTile.TopId == leftNeighbor.RightId)
				{
					nextTile.RotateLeft();
				}
				else if (nextTile.BottomId == leftNeighbor.RightId)
				{
					nextTile.RotateRight();
				}
			}

			if (nextTile.LeftHash != leftNeighbor.RightHash)
			{
				nextTile.FlipVertical();
			}

			return nextTile;
		}

		void ReadTiles(IEnumerable<string> input)
		{
			Tile? tile = null;

			foreach (var line in input)
			{
				if (string.IsNullOrWhiteSpace(line)) continue;
				if (line.StartsWith("Tile"))
				{
					var tokens = line.Split(new[] {' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
					tile = new Tile(int.Parse(tokens[1]));
					_tiles.Add(tile.Num, tile);
				}
				else
				{
					if (tile == null) throw new InvalidOperationException("Invalid input - no tile #");
					tile.AddLine(line);
				}
			}
		}

		void BuildSideMap()
		{
			foreach (var tile in _tiles.Values)
			{
				foreach (var side in tile.EdgeIds)
				{
					if (!_sideMap.ContainsKey(side))
					{
						_sideMap[side] = new List<int>();
					}

					_sideMap[side].Add(tile.Num);
				}
			}
		}
	}

	class FinalImage : StringMatrix
	{
		static readonly IEnumerable<(int y, int x)> _monsterOffsets = new List<(int, int)>
		{
			(1, -18),
			(1, -13),
			(1, -12),
			(1, -7),
			(1, -6),
			(1, -1),
			(1, 0),
			(1, 1),
			(2, -17),
			(2, -14),
			(2, -11),
			(2, -8),
			(2, -5),
			(2, -2),
		};

		readonly int _size;

		public FinalImage(int size) : base(size)
		{
			_size = size;
		}

		public int CountMonsters()
		{
			var monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			RotateLeft();
			monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			RotateLeft();
			monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			RotateLeft();
			monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			FlipHorizontal();
			monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			RotateLeft();
			monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			RotateLeft();
			monsters = TryFindMonsters();
			if (monsters > 0) return monsters;

			RotateLeft();
			monsters = TryFindMonsters();
			return monsters;
		}

		int TryFindMonsters()
		{
			int monsterCount = 0;
			var minX = _monsterOffsets.Min(o => o.x);
			var maxX = _monsterOffsets.Max(o => o.x);
			var maxY = _monsterOffsets.Max(o => o.y);

			for (int y = 0; y < _size - maxY; y++)
			{
				for (int x = 0 - minX; x < _size - maxX; x++)
				{
					if (this[y][x] == '#')
					{
						bool isMonster = true;
						foreach (var offset in _monsterOffsets)
						{
							if (this[y + offset.y][x + offset.x] != '#')
							{
								isMonster = false;
								break;
							}
						}

						if (isMonster)
						{
							monsterCount++;
						}
					}
				}
			}

			return monsterCount;
		}
	}

	class Tile : StringMatrix
	{
		const int _tileSize = 10;

		public int Num { get; }

		public Tile(int num) : base(_tileSize)
		{
			Num = num;
		}

		public string GetLineInner(int lineNum)
		{
			return this[lineNum].Substring(1, _tileSize - 2);
		}

		public string LeftId => MakeEdgeId(LeftHash, LeftFlipHash);

		public string RightId => MakeEdgeId(RightHash, RightFlipHash);

		public string TopId => MakeEdgeId(TopHash, TopFlipHash);

		public string BottomId => MakeEdgeId(BottomHash, BottomFlipHash);

		static string MakeEdgeId(int hash, int flipHash)
		{
			return hash < flipHash ? $"{hash}-{flipHash}" : $"{flipHash}-{hash}";
		}

		public IEnumerable<string> EdgeIds
		{
			get
			{
				yield return TopId;
				yield return RightId;
				yield return BottomId;
				yield return LeftId;
			}
		}

		public int TopHash
		{
			get
			{
				int hash = 0;
				for (int x = 0; x < _tileSize; x++)
				{
					if (this[0][x] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}

		public int TopFlipHash
		{
			get
			{
				int flipHash = 0;
				for (int x = _tileSize - 1; x >= 0; x--)
				{
					if (this[0][x] == '#')
					{
						flipHash++;
					}

					flipHash <<= 1;
				}

				return flipHash;
			}
		}

		public int BottomHash
		{
			get
			{
				int hash = 0;
				for (int x = 0; x < _tileSize; x++)
				{
					if (this[_tileSize - 1][x] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}

		public int BottomFlipHash
		{
			get
			{
				int hash = 0;
				for (int x = _tileSize - 1; x >= 0; x--)
				{
					if (this[_tileSize - 1][x] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}

		public int LeftHash
		{
			get
			{
				int hash = 0;
				for (int y = 0; y < _tileSize; y++)
				{
					if (this[y][0] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}

		public int LeftFlipHash
		{
			get
			{
				int hash = 0;
				for (int y = _tileSize - 1; y >= 0; y--)
				{
					if (this[y][0] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}

		public int RightHash
		{
			get
			{
				int hash = 0;
				for (int y = 0; y < _tileSize; y++)
				{
					if (this[y][_tileSize - 1] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}

		public int RightFlipHash
		{
			get
			{
				int hash = 0;
				for (int y = _tileSize - 1; y >= 0; y--)
				{
					if (this[y][_tileSize - 1] == '#')
					{
						hash++;
					}

					hash <<= 1;
				}

				return hash;
			}
		}
	}
}


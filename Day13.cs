using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day13
	{
		const string _inputFilename = @"Inputs\Day13.txt";

		const string _example1 = @"939
7,13,x,x,59,x,31,19";

		const string _example2 = "17,x,13,19";

		const string _example3 = "67,7,59,61";

		const string _example4 = "67,x,7,59,61";

		const string _example5 = "67,7,x,59,61";

		const string _example6 = "1789,37,47,1889";

		[Test]
		public void SilverTest()
		{
			var input = FileHelpers.ReadAllLinesFromString(_example1).ToArray();
			Assert.AreEqual(295, RunSilver(input));
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var input = FileHelpers.EnumerateLines(_inputFilename);
			Assert.AreEqual(2095, RunSilver(input));
		}

		static long RunSilver(IEnumerable<string> input)
		{
			var startTime = long.Parse(input.First());
			var schedule = input.Last();

			var buses = new List<int>();
			var tokens = schedule.Split(',');
			foreach (var token in tokens)
			{
				if (int.TryParse(token, out int bus))
				{
					buses.Add(bus);
				}
			}

			for (long currentTime = startTime;; currentTime++)
			{
				var bus = buses.FirstOrDefault(b => currentTime % b == 0);
				if (bus != default)
				{
					return (bus * (currentTime - startTime));
				}
			}
		}

		[Test]
		public void GoldTest()
		{
			Assert.AreEqual(1068781, RunGold(FileHelpers.ReadAllLinesFromString(_example1).Last()));
			Assert.AreEqual(3417, RunGold(FileHelpers.ReadAllLinesFromString(_example2).Last()));
			Assert.AreEqual(754018, RunGold(FileHelpers.ReadAllLinesFromString(_example3).Last()));
			Assert.AreEqual(779210, RunGold(FileHelpers.ReadAllLinesFromString(_example4).Last()));
			Assert.AreEqual(1261476, RunGold(FileHelpers.ReadAllLinesFromString(_example5).Last()));
			Assert.AreEqual(1202161486, RunGold(FileHelpers.ReadAllLinesFromString(_example6).Last()));
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(598411311431841, RunGold(FileHelpers.EnumerateLines(_inputFilename).Last()));
		}
		
		static long RunGold(string schedule)
		{
			var buses = new List<int>();
			var tokens = schedule.Split(',');
			foreach (var token in tokens)
			{
				if (int.TryParse(token, out int bus))
				{
					buses.Add(bus);
				}
				else
				{
					buses.Add(-1);
				}
			}

			long offset = buses[0];
			long mod = buses[0];

			for (int busIndex = 0; busIndex < buses.Count; busIndex++)
			{
				if (buses[busIndex] > 0)
				{
					(offset, mod) = FindNextGold(offset, mod, buses[busIndex], busIndex);
				}
			}

			return offset;
		}
		
		static (long nextOffset, long nextMod) FindNextGold(long offset, long mod, int bus, int busIndex)
		{
			long nextOffset = -1;

			for (long currentTime = offset;; currentTime += mod)
			{
				if ((currentTime + busIndex) % bus == 0)
				{
					if (nextOffset < 0) nextOffset = currentTime;
					else
					{
						return (nextOffset, currentTime - nextOffset);
					}
				}
			}
		}
	}
}

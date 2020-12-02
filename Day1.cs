using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	public class Day1
	{
		[Test]
		public void SilverTest()
		{
			var entries = new HashSet<int> {1721, 979, 366, 299, 675, 1456};
			Assert.AreEqual(514_579, Find2(entries));
		}

		[Test]
		public void Silver()
		{
			var entries = FileHelpers.EnumerateLines("Inputs\\Day1.txt").Select(int.Parse).ToHashSet();
			Assert.AreEqual(1_010_299, Find2(entries));
		}

		static long Find2(IReadOnlySet<int> entries)
		{
			foreach (int entry in entries)
			{
				if (entries.Contains(2020 - entry))
				{
					return entry * (2020 - entry);
				}
			}

			throw new InvalidOperationException("No match found");
		}

		[Test]
		public void GoldTest()
		{
			var entries = new[] {1721, 979, 366, 299, 675, 1456};
			Assert.AreEqual(241_861_950, Find3(entries));
		}

		[Test]
		public void Gold()
		{
			var entries = FileHelpers.EnumerateLines("Inputs\\Day1.txt").Select(int.Parse).ToArray();
			var total = Find3(entries);

			Assert.AreEqual(42_140_160, total);
		}

		static long Find3(IReadOnlyList<int> entries)
		{
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i] > 2020) continue;

				for (int j = i + 1; j < entries.Count; j++)
				{
					if (entries[i] + entries[j] > 2020) continue;

					for (int k = j + 1; k < entries.Count; k++)
					{
						if (entries[i] + entries[j] + entries[k] == 2020)
						{
							return entries[i] * entries[j] * entries[k];
						}
					}
				}
			}

			throw new InvalidOperationException("No match found");
		}
	}
}

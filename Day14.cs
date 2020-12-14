using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace Advent2020
{
	class Day14
	{
		const string _inputFilename = @"Inputs\Day14.txt";

		const string _silverExample =
			@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";

		const string _goldExample = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";

		[Test]
		public void SilverTest()
		{
			var result = RunSilver(FileHelpers.ReadAllLinesFromString(_silverExample));
			Assert.AreEqual(165, result);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunSilver(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(13476250121721, result);
		}
		
		static long RunSilver(IEnumerable<string> input)
		{
			string mask = string.Empty;

			var memory = new long[65536];

			foreach (var line in input)
			{
				var tokens = line.Split(new[] {' ', '='}, StringSplitOptions.RemoveEmptyEntries);
				if (tokens[0] == "mask")
				{
					mask = Strings.StrReverse(tokens[1]);
					Console.WriteLine(mask);
				}
				else if (tokens[0].StartsWith("mem["))
				{
					var address = int.Parse(tokens[0].Substring(4, tokens[0].Length - 5));
					var val = long.Parse(tokens[1]);

					for (int c = 0; c < 36; c++)
					{
						var bit = 1L << c;
						switch (mask[c])
						{
							case 'X':
								if (((val >> c) & 1) == 1)
								{
									memory[address] = memory[address] | bit;
								}
								else
								{
									memory[address] = memory[address] & ~bit;
								}
								break;
							case '1':
								memory[address] = memory[address] | bit;
								break;
							case '0':
								memory[address] = memory[address] & ~bit;
								break;
						}
					}
				}
				else
				{
					throw new InvalidOperationException("Invalid input: " + line);
				}
			}

			return memory.Sum();
		}

		[Test]
		public void GoldTest()
		{
			var result = RunGold(FileHelpers.ReadAllLinesFromString(_goldExample));
			Assert.AreEqual(208, result);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunGold(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(4463708436768, result);
		}

		static long RunGold(IEnumerable<string> input)
		{
			string mask = string.Empty;

			var allMem = new Dictionary<long, long>();

			foreach (var line in input)
			{
				var tokens = line.Split(new[] {' ', '='}, StringSplitOptions.RemoveEmptyEntries);
				if (tokens[0] == "mask")
				{
					mask = Strings.StrReverse(tokens[1]);
				}
				else if (tokens[0].StartsWith("mem["))
				{
					var address = long.Parse(tokens[0].Substring(4, tokens[0].Length - 5));
					var val = long.Parse(tokens[1]);

					for (int c = 0; c < 36; c++)
					{
						if (mask[c] == '1')
						{
							address |= (1L << c);
						}
					}

					TryGold(allMem, address, 0, mask, val);
				}
				else
				{
					throw new InvalidOperationException("Invalid input: " + line);
				}
			}

			long total = allMem.Sum(a => a.Value);
			return total;
		}

		static void TryGold(IDictionary<long, long> allMem, long address, int bit, string mask, long val)
		{
			allMem[address] = val;
			for (int c = bit; c < 36; c++)
			{
				if (mask[c] == 'X')
				{
					TryGold(allMem, (address | (1L << c)), c + 1, mask, val);
					TryGold(allMem, (address & ~(1L << c)), c + 1, mask, val);
				}
			}
		}
	}
}

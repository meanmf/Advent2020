using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day25
	{
		const string _inputFilename = @"Inputs\Day25.txt";

		const string _example = @"5764801
17807724";

		const long _mod = 20201227;

		[Test]
		public void SilverTest()
		{
			Assert.AreEqual(14897079, Run(FileHelpers.ReadAllLinesFromString(_example)));
		}

		[Test]
		public void Silver()
		{
			Assert.AreEqual(19774660, Run(FileHelpers.EnumerateLines(_inputFilename)));
		}

		static long Run(IEnumerable<string> input)
		{
			var cardPublicKey = long.Parse(input.First());
			var doorPublicKey = long.Parse(input.Last());

			long cardLoopSize = FindLoopSize(cardPublicKey);
			return TransformKey(doorPublicKey, cardLoopSize);
		}

		static long FindLoopSize(long publicKey)
		{
			long value = 1;
			for (int loopSize = 1;; loopSize++)
			{
				value = ((value << 3) - value) % _mod; // value * 7 % mod
				if (value == publicKey)
				{
					return loopSize;
				}
			}
		}

		static long TransformKey(long key, long loopSize)
		{
			long value = 1;
			for (int i = 0; i < loopSize; i++)
			{
				value = (value * key) % _mod;
			}

			return value;
		}
	}
}

using System;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day5
	{
		const string _inputFilename = @"Inputs\Day5.txt";

		static readonly (string text, int row, int col, int seat)[] _testPasses =
		{
			("BFFFBBFRRR", 70, 7, 567),
			("FFFBBBFRRR", 14, 7, 119),
			("BBFFBBFRLL", 102, 4, 820),
		};

		[Test]
		public void SilverTest()
		{
			foreach (var testPass in _testPasses)
			{
				var (row, col, seat) = ParseBoardingPass(testPass.text);
				Assert.AreEqual(testPass.row, row);
				Assert.AreEqual(testPass.col, col);
				Assert.AreEqual(testPass.seat, seat);
			}
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var max = FileHelpers.EnumerateLines(_inputFilename).Select(ParseSeat).Max();
			Assert.AreEqual(980, max);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var seats = FileHelpers.EnumerateLines(_inputFilename).Select(ParseSeat).ToList();
			seats.Sort();

			int missingSeat = 0;
			for (int i = 0; i < seats.Count - 1; i++)
			{
				if (seats[i] != seats[i + 1] - 1)
				{
					missingSeat = seats[i] + 1;
					break;
				}
			}

			Assert.AreEqual(607, missingSeat);
		}

		static (int row, int column, int SeatId) ParseBoardingPass(string pass)
		{
			var seatId = ParseSeat(pass);
			var row = (seatId & 0b1111111000) >> 3;
			var column = seatId & 0b111;

			return (row, column, seatId);
		}

		static int ParseSeat(string pass)
		{
			var binaryString = pass.Replace('B', '1').Replace('F', '0').Replace('L', '0').Replace('R', '1');
			return Convert.ToInt32(binaryString, 2);
		}
	}
}

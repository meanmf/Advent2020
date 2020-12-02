using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Advent2020
{
	public class Day2
	{
		const string _inputFilename = @"Inputs\Day2.txt";

		[Test]
		public void SilverTest()
		{
			Assert.IsTrue(IsSilverValid("1-3 a: abcde"));
			Assert.IsFalse(IsSilverValid("1-3 b: cdefg"));
			Assert.IsTrue(IsSilverValid("2-9 c: ccccccccc"));
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var total = FileHelpers.EnumerateLines(_inputFilename).Count(IsSilverValid);
			Assert.AreEqual(582, total);
		}

		static bool IsSilverValid(string line)
		{
			var (from, to, matchCharacter, password) = ParseLine(line);

			var matchCount = password.Count(passwordChar => passwordChar == matchCharacter);
			return (matchCount >= from && matchCount <= to);
		}

		[Test]
		public void GoldTest()
		{
			Assert.IsTrue(IsGoldValid("1-3 a: abcde"));
			Assert.IsFalse(IsGoldValid("1-3 b: cdefg"));
			Assert.IsFalse(IsGoldValid("2-9 c: ccccccccc"));
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var total = FileHelpers.EnumerateLines(_inputFilename).Count(IsGoldValid);
			Assert.AreEqual(729, total);
		}

		static bool IsGoldValid(string line)
		{
			var (index1, index2, matchCharacter, password) = ParseLine(line);

			return (password[index1 - 1]) == matchCharacter ^ (password[index2 - 1] == matchCharacter);
		}

		static readonly Regex _regex = new Regex(@"(?<a>\d+)-(?<b>\d+) (?<matchCharacter>.+): (?<password>\S+)");
		static (int, int, char, string) ParseLine(string line)
		{
			var match = _regex.Match(line);

			return (
				int.Parse(match.Groups["a"].Value),
				int.Parse(match.Groups["b"].Value),
				match.Groups["matchCharacter"].Value[0],
				match.Groups["password"].Value);
		}
	}
}
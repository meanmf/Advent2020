using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day4
	{
		const string _inputFilename = @"Inputs\Day4.txt";
		
		const string _birthYear = "byr";
		const string _issueYear = "iyr";
		const string _expirationYear = "eyr";
		const string _height = "hgt";
		const string _hairColor = "hcl";
		const string _eyeColor = "ecl";
		const string _passportId = "pid";

		const string _hairColorChars = "0123456789abcdef";
		readonly string[] _eyeColors = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

		#region Example Input
		const string _silver = @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";

		const string _bad = @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";

		const string _good = @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
		#endregion

		[Test]
		public void SilverTest()
		{
			var count = CountValidPassports(FileHelpers.ReadAllLinesFromString(_silver), ValidateSilver);
			Assert.AreEqual(2, count);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var count = CountValidPassports(FileHelpers.EnumerateLines(_inputFilename), ValidateSilver);
			Assert.AreEqual(237, count);
		}
		
		[Test]
		public void GoldTest()
		{
			var count = CountValidPassports(FileHelpers.ReadAllLinesFromString(_bad), ValidateGold);
			Assert.AreEqual(0, count);

			count = CountValidPassports(FileHelpers.ReadAllLinesFromString(_good), ValidateGold);
			Assert.AreEqual(4, count);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var count = CountValidPassports(FileHelpers.EnumerateLines(_inputFilename), ValidateGold);
			Assert.AreEqual(172, count);
		}

		static int CountValidPassports(IEnumerable<string> inputs, Func<IReadOnlyDictionary<string, string>, bool> validator)
		{
			var passport = new Dictionary<string, string>();
			var valid = 0;

			foreach (var line in inputs)
			{
				if (string.IsNullOrEmpty(line))
				{
					valid += (validator(passport) ? 1 : 0);
					passport.Clear();
				}
				else
				{
					var tokens = line.Split(" ");
					foreach (var token in tokens)
					{
						var kv = token.Split(":");
						passport.Add(kv[0], kv[1]);
					}
				}
			}

			valid += (validator(passport) ? 1 : 0);

			return valid;
		}

		static bool ValidateSilver(IReadOnlyDictionary<string, string> passport)
		{
			return passport.ContainsKey(_birthYear) &&
			       passport.ContainsKey(_issueYear) &&
			       passport.ContainsKey(_expirationYear) &&
			       passport.ContainsKey(_height) &&
			       passport.ContainsKey(_hairColor) &&
			       passport.ContainsKey(_eyeColor) &&
			       passport.ContainsKey(_passportId);
		}

		bool ValidateGold(IReadOnlyDictionary<string, string> passport)
		{
			if (!ValidateSilver(passport)) return false;

			if (!CheckIntRange(passport[_birthYear], 1920, 2002)) return false;

			if (!CheckIntRange(passport[_issueYear], 2010, 2020)) return false;

			if (!CheckIntRange(passport[_expirationYear], 2020, 2030)) return false;

			if (!int.TryParse(passport[_height].Substring(0, passport[_height].Length - 2), out int height)) return false;
			if (passport[_height].EndsWith("cm"))
			{
				if (height < 150 || height > 193) return false;
			}
			else if (passport[_height].EndsWith("in"))
			{
				if (height < 59 || height > 76) return false;
			}
			else
			{
				return false;
			}

			if (passport[_hairColor][0] != '#') return false;
			
			if (passport[_hairColor].Skip(1).Any(c => !_hairColorChars.Contains(c))) return false;

			if (!_eyeColors.Contains(passport[_eyeColor])) return false;

			if (!int.TryParse(passport[_passportId], out _) || passport[_passportId].Length != 9) return false;

			return true;
		}

		static bool CheckIntRange(string field, int min, int max)
		{
			if (!int.TryParse(field, out int value)) return false;
			return (value >= min && value <= max);
		}
	}
}

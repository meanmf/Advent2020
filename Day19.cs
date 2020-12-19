using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Advent2020
{
	class Day19
	{
		const string _inputFilename = @"Inputs\Day19.txt";

		const string _silverExample =
			@"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb";

		const string _goldExample = @"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba";

		[Test]
		public void SilverTest()
		{
			var result = RunSilver(FileHelpers.ReadAllLinesFromString(_silverExample));
			Assert.AreEqual(2, result);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunSilver(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(265, result);
		}

		[Test]
		public void GoldTest()
		{
			var result = RunGold(FileHelpers.ReadAllLinesFromString(_goldExample));
			Assert.AreEqual(12, result);
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var result = RunGold(FileHelpers.EnumerateLines(_inputFilename));
			Assert.AreEqual(394, result);
		}

		class RuleSet
		{
			readonly IDictionary<int, string> _rules = new Dictionary<int, string>();
			
			public RuleSet(IEnumerable<string> input)
			{
				foreach (var line in input.Where(line => line.Contains(":")))
				{
					var tokens = line.Split(":");
					_rules[int.Parse(tokens[0])] = tokens[1];
				}
			}
			
			public string ToRegex(string rule)
			{
				var tokens = rule.Split(" ");
				return string.Join("", tokens.Select(TokenToRegex));
			}

			string TokenToRegex(string token)
			{
				if (!int.TryParse(token, out int ruleNum))
				{
					return token.Trim('"');
				}

				var subRules = _rules[ruleNum].Split("|");
				var expressions = subRules.Select(ToRegex).ToArray();

				return expressions.Length == 1 ? expressions.Single() : $"({string.Join("|", expressions)})";
			}
		}

		static int RunSilver(IEnumerable<string> input)
		{
			var rules = new RuleSet(input);
			
			var rule0 = rules.ToRegex("0");
			var regex = new Regex($"^{rule0}$");

			return input.Count(regex.IsMatch);
		}

		static int RunGold(IEnumerable<string> input)
		{
			var rules = new RuleSet(input);

			var rule42 = rules.ToRegex("42");
			var rule31 = rules.ToRegex("31");
			
			// 8: 42 | 42 8
			// 42 repeated at least once
			var rule8 = $"({rule42}){{1,}}";

			// 11: 42 31 | 42 11 31
			// 42 31 | 42 42 31 31 | 42 42 42 31 31 31 ...
			// ((x){1}(y){1})|(x){2}(y){2}|...
			const int maxRecursion = 5;
			var rule11 = "(" + string.Join("|", Enumerable.Range(1, maxRecursion).Select(i => $"({rule42}{{{i}}}{rule31}{{{i}}})")) + ")";
			
			var regex = new Regex($"^{rule8}{rule11}$");
			return input.Count(regex.IsMatch);
		}
	}
}

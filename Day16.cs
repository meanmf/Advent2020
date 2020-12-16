using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day16
	{
		const string _inputFilename = @"Inputs\Day16.txt";

		const string _silverExample =
			@"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";

		const string _goldExample = @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9";

		record Rule(string Name, int From1, int To1, int From2, int To2)
		{
			public HashSet<int> InvalidFields { get; } = new();

			public bool IsValueInRange(int value)
			{
				return (value >= From1 && value <= To1) || (value >= From2 && value <= To2);
			}
		}

		[Test]
		public void SilverTest()
		{
			Assert.AreEqual(71, RunSilver(FileHelpers.ReadAllLinesFromString(_silverExample)));
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(20013, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
		}

		static int RunSilver(IEnumerable<string> input)
		{
			var rules = ParseRules(input);
			int total = 0;
			foreach (var line in input)
			{
				if (string.IsNullOrWhiteSpace(line) || line.Contains(':')) continue;

				var tokens = line.Split(',');
				var ticket = tokens.Select(int.Parse);
				
				foreach (var field in ticket)
				{
					bool isValid = rules.Any(rule => rule.IsValueInRange(field));
					if (!isValid) total += field;
				}
			}

			return total;
		}
		
		[Test]
		public void GoldTest()
		{
			Assert.AreEqual(12, RunGold(FileHelpers.ReadAllLinesFromString(_goldExample), "cl"));
		}
		
		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			Assert.AreEqual(5_977_293_343_129, RunGold(FileHelpers.EnumerateLines(_inputFilename), "departure"));
		}
		
		static long RunGold(IEnumerable<string> input, string match)
		{
			var rules = ParseRules(input);

			int[]? yourTicket = null;

			foreach (var line in input)
			{
				if (string.IsNullOrWhiteSpace(line) || line.Contains(':')) continue;

				var tokens = line.Split(',');
				var ticket = tokens.Select(int.Parse).ToArray();

				yourTicket ??= ticket;

				bool isTicketValid = true;
				foreach (int f in ticket)
				{
					bool isFieldValid = false;
					foreach (var rule in rules)
					{
						if ((f >= rule.From1 && f <= rule.To1) || (f >= rule.From2 && f <= rule.To2))
						{
							isFieldValid = true;
							break;
						}
					}

					if (!isFieldValid)
					{
						isTicketValid = false;
						break;
					}
				}

				if (isTicketValid)
				{
					for (int i = 0; i < ticket.Length; i++)
					{
						int f = ticket[i];
						foreach (var rule in rules)
						{
							if (!rule.IsValueInRange(f))
							{
								rule.InvalidFields.Add(i);
							}
						}
					}
				}
			}

			if (yourTicket == null) throw new InvalidOperationException("No tickets found");

			var positions = FindPositions(rules, yourTicket.Length);

			long total = 1;
			foreach (var field in positions.Where(position => position.Key.StartsWith(match)))
			{
				total *= yourTicket[field.Value];
			}

			return total;
		}
			
		static IReadOnlyList<Rule> ParseRules(IEnumerable<string> input)
		{
			var rules = new List<Rule>();
			foreach (var line in input)
			{
				if (!line.Contains(":")) continue;
				if (line.StartsWith("your ticket") || line.StartsWith("nearby tickets")) continue;
				
				var tok = line.Split(":");
				var key = tok[0];
				var tokens = tok[1].Split(new[] {':', ' ', '-'},StringSplitOptions.RemoveEmptyEntries);
				var from1 = int.Parse(tokens[0]);
				var to1 = int.Parse(tokens[1]);
				var from2 = int.Parse(tokens[3]);
				var to2 = int.Parse(tokens[4]);

				rules.Add(new Rule(key, from1, to1, from2, to2));
			}

			return rules;
		}

		static IReadOnlyDictionary<string, int> FindPositions(IEnumerable<Rule> rules, int fieldCount)
		{
			var positions = new Dictionary<string, int>();
			var foundPositions = new HashSet<int>();

			foreach (var rule in rules.OrderByDescending(rule => rule.InvalidFields.Count))
			{
				for (int i = 0; i < fieldCount; i++)
				{
					if (!foundPositions.Contains(i) && !rule.InvalidFields.Contains(i))
					{
						positions[rule.Name] = i;
						foundPositions.Add(i);
						break;
					}
				}
			}

			return positions;
		}
	}
}

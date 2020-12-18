using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day18
	{
		const string _inputFilename = @"Inputs\Day18.txt";

		record Example(string Expression, int Silver, int Gold);

		static readonly IEnumerable<Example> _examples = new[]
		{
			new Example("1 + (2 * 3) + (4 * (5 + 6))", 51, 51),
			new Example("2 * 3 + (4 * 5)", 26, 46),
			new Example("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437, 1445),
			new Example("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240, 669060),
			new Example("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632, 23340),
		};

		[Test]
		public void SilverTest()
		{
			foreach ((var expression, int result, _) in _examples)
			{
				Assert.AreEqual(result, ParseSilver(expression));
			}
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var total = FileHelpers.EnumerateLines(_inputFilename).Sum(ParseSilver);
			Assert.AreEqual(510009915468, total);
		}
		
		[Test]
		public void GoldTest()
		{
			foreach ((var expression, _, int result) in _examples)
			{
				Assert.AreEqual(result, ParseGold(expression));
			}
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var total = FileHelpers.EnumerateLines(_inputFilename).Sum(ParseGold);
			Assert.AreEqual(321176691637769, total);
		}

		static long ParseSilver(string input) => Parse(input, EvalSilver);
		static long ParseGold(string input) => Parse(input, EvalGold);
				
		static long Parse(string input, Func<string, long> eval)
		{
			for(;;)
			{
				var openParen = input.IndexOf('(');
				if (openParen == -1) break;
				
				int closeParen = FindCloseParen(input, openParen);
				var subExpression = input.Substring(openParen + 1, closeParen - openParen - 1);
				var subExpressionValue = Parse(subExpression, eval);
				input = input.Substring(0, openParen) + subExpressionValue + input.Substring(closeParen + 1);
			}
			
			return eval(input);
		}

		static long EvalSilver(string expression)
		{
			expression = Eval(expression);
			return long.Parse(expression);
		}

		static long EvalGold(string expression)
		{
			expression = Eval(expression, "+");
			expression = Eval(expression, "*");

			return long.Parse(expression);
		}
		
		static string Eval(string expression, string? operatorFilter = null)
		{
			var tokens = expression.Split(' ').ToList();

			for (int i = 1; i < tokens.Count;)
			{
				if (operatorFilter == null || tokens[i] == operatorFilter)
				{
					var val1 = long.Parse(tokens[i - 1]);
					var val2 = long.Parse(tokens[i + 1]);

					switch (tokens[i])
					{
						case "+":
							tokens[i - 1] = (val1 + val2).ToString();
							break;
						case "*":
							tokens[i - 1] = (val1 * val2).ToString();
							break;
						default:
							throw new InvalidOperationException("unknown operator: " + operatorFilter);
					}

					tokens.RemoveAt(i);
					tokens.RemoveAt(i);
				}
				else
				{
					i += 2;
				}
			}

			return string.Join(" ", tokens);
		}

		static int FindCloseParen(string input, int open)
		{
			int currentDepth = 1;
			int close = open;
			while (currentDepth > 0)
			{
				close++;
				if (input[close] == '(')
				{
					currentDepth++;
				}
				else if (input[close] == ')')
				{
					currentDepth--;
				}
			}

			return close;
		}
	}
}

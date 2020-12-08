using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Advent2020
{
	class Day8
	{
		const string _inputFilename = @"Inputs\Day8.txt";

		const string _example =
			@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

		[Test]
		public void SilverTest()
		{
			var program = new Program(FileHelpers.ReadAllLinesFromString(_example));
			var result = program.Run();
			Assert.AreEqual(5, result.acc);
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var program = new Program(FileHelpers.EnumerateLines(_inputFilename));
			var result = program.Run();
			Assert.AreEqual(1384, result.acc);
		}

		[Test]
		public void GoldTest()
		{
			var program = new Program(FileHelpers.ReadAllLinesFromString(_example));
			var result = RunGold(program);
			Assert.AreEqual(8, result);
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var program = new Program(FileHelpers.EnumerateLines(_inputFilename));
			var result = RunGold(program);
			Assert.AreEqual(761, result);
		}

		static int RunGold(Program program)
		{
			for (int i = 0; i < program.Instructions.Count; i++)
			{
				if (program.Instructions[i].Command != Operator.jmp &&
				    program.Instructions[i].Command != Operator.nop) continue;

				var patch = new Dictionary<int, Statement>
				{
					{
						i,
						new Statement(program.Instructions[i].Command == Operator.jmp ? Operator.nop : Operator.jmp,
							program.Instructions[i].Operand)
					}
				};

				(int acc, int ip) = program.Run(patch);
				if (ip > program.Instructions.Count - 1)
				{
					return acc;
				}
			}

			return -1;
		}

		enum Operator
		{
			nop,
			acc,
			jmp
		}

		record Statement
		{
			public Operator Command { get; }
			public int Operand { get; }

			public Statement(Operator command, int operand)
			{
				Command = command;
				Operand = operand;
			}
		}

		class Program
		{
			public IReadOnlyList<Statement> Instructions { get; }

			public Program(IEnumerable<string> input)
			{
				var instructions = new List<Statement>();

				foreach (var line in input)
				{
					var tokens = line.Split(" ");

					if (!Enum.TryParse<Operator>(tokens[0], out var command))
					{
						throw new InvalidOperationException($"Invalid operator: {tokens[0]}");
					}

					if (!int.TryParse(tokens[1], out var operand))
					{
						throw new InvalidOperationException($"Invalid operand: {tokens[1]}");
					}

					instructions.Add(new Statement(command, operand));
				}

				Instructions = instructions;
			}

			public (int acc, int ip) Run(IReadOnlyDictionary<int, Statement>? patch = null)
			{
				int acc = 0;
				int ip = 0;
				
				var visited = new HashSet<int>();
				while (ip < Instructions.Count && !visited.Contains(ip))
				{
					visited.Add(ip);

					if (patch == null || !patch.TryGetValue(ip, out var statement))
					{
						statement = Instructions[ip];
					}

					switch (statement.Command)
					{
						case Operator.nop:
							ip++;
							break;
						case Operator.acc:
							acc += statement.Operand;
							ip++;
							break;
						case Operator.jmp:
							ip += statement.Operand;
							break;
						default:
							throw new InvalidOperationException($"Unknown command: {statement.Command}");
					}
				}

				return (acc, ip);
			}
		}
	}
}

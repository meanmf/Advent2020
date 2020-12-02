using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Advent2020
{
	static class FileHelpers
	{
		public static IEnumerable<string> EnumerateLines(string filename)
		{
			using var inputFile = new StreamReader(File.OpenRead(filename));
			while (!inputFile.EndOfStream)
			{
				yield return inputFile.ReadLine();
			}
		}

		public static string GetSingle(string filename)
		{
			using var inputFile = new StreamReader(File.OpenRead(filename));
			return inputFile.ReadLine();
		}

		public static IEnumerable<string> ReadAllLinesFromString(string input)
		{
			using var inputStream = new StringReader(input);

			for (; ; )
			{
				var line = inputStream.ReadLine();
				if (line == null) yield break;

				yield return line;
			}
		}

		public static void CheckInputs(string filename)
		{
			if (!File.Exists(filename))
			{
				Assert.Inconclusive("Inputs not available");
			}
		}
	}
}
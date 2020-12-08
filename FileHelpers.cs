using System.Collections.Generic;
using System.IO;
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
				var line = inputFile.ReadLine();
				if (line == null) break;
				yield return line;
			}
		}

		public static string GetSingle(string filename)
		{
			using var inputFile = new StreamReader(File.OpenRead(filename));
			var line = inputFile.ReadLine();
			if (line == null) return string.Empty;
			return line;
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
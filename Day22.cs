using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advent2020
{
	class Day22
	{
		const string _inputFilename = @"Inputs\Day22.txt";

		const string _example =
			@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";

		[Test]
		public void SilverTest()
		{
			var input = FileHelpers.ReadAllLinesFromString(_example);
			Assert.AreEqual(306, RunSilver(input));
		}

		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var input = FileHelpers.EnumerateLines(_inputFilename);
			Assert.AreEqual(34566, RunSilver(input));
		}

		[Test]
		public void GoldTest()
		{
			var input = FileHelpers.ReadAllLinesFromString(_example);
			Assert.AreEqual(291, RunGold(input));
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var input = FileHelpers.EnumerateLines(_inputFilename);
			var timer = new Stopwatch();
			timer.Start();
			Assert.AreEqual(31854, RunGold(input));
			Console.WriteLine(timer.ElapsedMilliseconds);
		}

		static long RunSilver(IEnumerable<string> input)
		{
			var decks = LoadDecks(input);
			RunGame(decks, false);
			return CountScore(decks);
		}

		static long RunGold(IEnumerable<string> input)
		{
			var decks = LoadDecks(input);
			RunGame(decks, true);
			return CountScore(decks);
		}

		static int RunGame(Queue<int>[] decks, bool recursive)
		{
			var hashes = new HashSet<(int,int)>();
			for (; ; )
			{
				var deckHash = GameStateHash(decks);
				if (hashes.Contains(deckHash))
				{
					return 0;
				}

				hashes.Add(deckHash);

				if (decks[0].Count == 0)
				{
					return 1;
				}

				if (decks[1].Count == 0)
				{
					return 0;
				}

				var card1 = decks[0].Dequeue();
				var card2 = decks[1].Dequeue();

				int roundWinner;

				if (recursive && decks[0].Count >= card1 && decks[1].Count >= card2)
				{
					var newDecks = new[]
					{
						new Queue<int>(decks[0].Take(card1)),
						new Queue<int>(decks[1].Take(card2)),
					};

					roundWinner = RunGame(newDecks, recursive);
				}
				else if (card1 > card2)
				{
					roundWinner = 0;
				}
				else
				{
					roundWinner = 1;
				}

				if (roundWinner == 0)
				{
					decks[0].Enqueue(card1);
					decks[0].Enqueue(card2);
				}
				else
				{
					decks[1].Enqueue(card2);
					decks[1].Enqueue(card1);
				}
			}
		}

		static long CountScore(Queue<int>[] decks)
		{
			long total = 0;
			foreach (var deck in decks)
			{
				int multiplier = deck.Count;
				while (deck.TryDequeue(out int card))
				{
					total += card * multiplier;
					multiplier--;
				}
			}

			return total;
		}

		static int DeckHash(Queue<int> deck)
		{
			int hash = 17;
			foreach (var card in deck)
			{
				// hash = (hash * 31) + card
				hash = (hash << 5 - hash) + card;
			}

			return hash;
		}
		
		static (int, int) GameStateHash(Queue<int>[] decks)
		{
			return (DeckHash(decks[0]), DeckHash(decks[1]));
		}

		static Queue<int>[] LoadDecks(IEnumerable<string> input)
		{
			var decks = new Queue<int>[] { new(), new() };

			int player = 0;
			foreach (var line in input)
			{
				if (line.StartsWith("Player")) continue;
				if (string.IsNullOrEmpty(line))
				{
					player++;
					continue;
				}

				decks[player].Enqueue(int.Parse(line));
			}

			return decks;
		}
	}
}

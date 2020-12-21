using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Advent2020
{
	class Day21
	{
		const string _inputFilename = @"Inputs\Day21.txt";

		const string _example =
			@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";

		enum State
		{
			Unknown,
			Possible,
			Impossible,
		}

		[Test]
		public void SilverTest()
		{
			var input = FileHelpers.ReadAllLinesFromString(_example);
			Assert.AreEqual(5, RunSilver(input));
		}
		
		[Test]
		public void Silver()
		{
			FileHelpers.CheckInputs(_inputFilename);
			
			var input = FileHelpers.EnumerateLines(_inputFilename);
			Assert.AreEqual(2573, RunSilver(input));
		}

		[Test]
		public void GoldTest()
		{
			var input = FileHelpers.ReadAllLinesFromString(_example);
			Assert.AreEqual("mxmxvkd,sqjhc,fvjkl", RunGold(input));
		}

		[Test]
		public void Gold()
		{
			FileHelpers.CheckInputs(_inputFilename);

			var input = FileHelpers.EnumerateLines(_inputFilename);
			Assert.AreEqual("bjpkhx,nsnqf,snhph,zmfqpn,qrbnjtj,dbhfd,thn,sthnsg", RunGold(input));
		}
		
		static int RunSilver(IEnumerable<string> input)
		{
			var (allFoods, byIngredient) = FindIngredients(input);

			int count = 0;
			foreach (var i in byIngredient.Where(i => i.Value.Count == 0))
			{
				foreach (var f in allFoods)
				{
					if (f.ingredients.Contains(i.Key))
					{
						count++;
					}
				}
			}

			return count;
		}
		
		static string RunGold(IEnumerable<string> input)
		{
			var (_, byIngredient) = FindIngredients(input);
			
			var allergens = new Dictionary<string, string>();
			for (;;)
			{
				bool foundAny = false;
				foreach (var ingredient in byIngredient)
				{
					if (ingredient.Value.Count == 1)
					{
						var allergen = ingredient.Value.Single();
						foundAny = true;
						allergens[ingredient.Key] = allergen;
						foreach (var otherIngredient in byIngredient)
						{
							otherIngredient.Value.Remove(allergen);
						}
					}
				}

				if (!foundAny) break;
			}

			return string.Join(",", allergens.OrderBy(f => f.Value).Select(f => f.Key));
		}

		static (List<(string[] ingredients, string[] allergens)> allFoods,
				IReadOnlyDictionary<string, HashSet<string>> byIngredient) FindIngredients(IEnumerable<string> input)
		{
			var allFoods = new List<(string[] ingredients, string[] allergens)>();
			
			foreach (var line in input)
			{
				var tokens = line.Split(new[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
				var ingredients = tokens[0].Split(' ',StringSplitOptions.RemoveEmptyEntries);
				var allergens = tokens[1].Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

				allFoods.Add((ingredients, allergens));
			}

			var allIngredients = allFoods.SelectMany(f => f.ingredients).Distinct().ToList();
			var allAllergens = new Dictionary<string, Dictionary<string, State>>();
			
			foreach (var food in allFoods)
			{
				foreach (var i in food.allergens)
				{
					if (!allAllergens.ContainsKey(i))
					{
						allAllergens.Add(i, allIngredients.ToDictionary(a => a, _ => State.Unknown));
					}
				}
			}

			foreach (var food in allFoods)
			{
				foreach (var allergen in food.allergens)
				{
					foreach (var ingredient in allIngredients)
					{
						if (!food.ingredients.Contains(ingredient))
						{
							allAllergens[allergen][ingredient] = State.Impossible;
						}
					}
				}
				
				foreach (var allergen in allAllergens.Keys)
				{
					if (food.allergens.Contains(allergen))
					{
						foreach (var i in food.ingredients)
						{
							if (allAllergens[allergen][i] == State.Unknown)
							{
								allAllergens[allergen][i] = State.Possible;
							}
						}
					}
				}
			}

			var byIngredient = allIngredients.ToDictionary(i => i, _ => new HashSet<string>());
			foreach (var allergen in allAllergens)
			{
				foreach (var ingredient in allergen.Value.Where(i => i.Value == State.Possible))
				{
					byIngredient[ingredient.Key].Add(allergen.Key);
				}
			}

			return (allFoods, byIngredient);
		}
	}
}

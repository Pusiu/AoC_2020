using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Allergen
	{
		public string name;
		public List<string> possibleIngredients = new List<string>();

		public override string ToString()
		{
			return name;
		}
	}

	class Day21 : Day
	{
		List<(List<string> ingredients, List<string> allergens)> recipes = new List<(List<string>, List<string>)>();
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day21.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				lines.Add(line);
				Match m = Regex.Match(line, @"^((?<ingredient>\w+) )+\(contains ((?<allergen>\w+)(, )?)+\)$", RegexOptions.ExplicitCapture);

				List<string> ingredients = new List<string>();
				List<string> allergens = new List<string>();
				foreach (Capture al in m.Groups["allergen"].Captures)
				{
					allergens.Add(al.Value);
				}
				foreach (Capture ing in m.Groups["ingredient"].Captures)
					ingredients.Add(ing.Value);

				recipes.Add((ingredients, allergens));
			}

			Dictionary<string, string> allergenToIngredientMap = new Dictionary<string, string>();
			List<string> allAllergens = recipes.SelectMany(x => x.allergens).Distinct().ToList();

			do
			{
				foreach (string allergen in allAllergens.ToList())
				{
					HashSet<string> candidateIngredients = new HashSet<string>(recipes.First(x => x.allergens.Contains(allergen)).ingredients.ToList());
					foreach ((List<string> i, List<string> a) recipe in recipes.Where(x => x.allergens.Contains(allergen)).Skip(1))
					{
						candidateIngredients.IntersectWith(recipe.i);
					}
					if (candidateIngredients.Count == 1)
					{
						string finalIngredient = candidateIngredients.Single();
						allergenToIngredientMap[allergen] = finalIngredient;
						foreach (var recipe in recipes)
							recipe.ingredients.Remove(finalIngredient);

						allAllergens.Remove(allergen);
					}
				}

			} while (allAllergens.Count != 0);

			Console.WriteLine($"Part 1: {recipes.SelectMany(x => x.ingredients).Count().ToString()}");
			Console.WriteLine($"Part 2: {string.Join(",", allergenToIngredientMap.OrderBy(x => x.Key).Select(x => x.Value))}");
		}
	}
}

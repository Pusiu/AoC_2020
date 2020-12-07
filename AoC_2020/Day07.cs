using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Bag
	{
		public string color;

		public Bag(string color)
		{
			this.color = color;
		}

		public HashSet<Bag> parents = new HashSet<Bag>();
		public Dictionary<Bag, int> children = new Dictionary<Bag, int>();

		public override string ToString()
		{
			return color;
		}
	}

	class Day07 : Day
	{
		List<Bag> allBags = new List<Bag>();
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day07.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());


			foreach (string bag in lines)
			{
				string[] s1 = bag.Split(new string[] { "s contain " }, StringSplitOptions.RemoveEmptyEntries);
				string col = s1[0];

				Bag thisBag = allBags.Where(x => x.color == col).FirstOrDefault();
				if (thisBag == null)
				{
					thisBag = new Bag(col);
					allBags.Add(thisBag);
				}

				if (s1[1] != "no other bags.")
				{
					string[] s2 = s1[1].Substring(0, s1[1].Length - 1).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string sb in s2)
					{
						Match m = Regex.Match(sb, @"(\d+) (.+)");
						int count = int.Parse(m.Groups[1].Value);
						string otherBagColor = m.Groups[2].Value;
						if (otherBagColor[otherBagColor.Length - 1] == 's')
							otherBagColor = otherBagColor.Substring(0, otherBagColor.Length - 1);

						Bag b = allBags.Where(x => x.color == otherBagColor).FirstOrDefault();
						if (b == null)
						{
							b = new Bag(otherBagColor);
							allBags.Add(b);
						}
						b.parents.Add(thisBag);

						thisBag.children.Add(b, count);

					}
				}
			}

			int part1 = 0;
			HashSet<Bag> c = new HashSet<Bag>();
			foreach (Bag b in allBags)
			{
				if (b.color == "shiny gold bag")
					continue;

				Queue<Bag> q = new Queue<Bag>();
				HashSet<Bag> visited = new HashSet<Bag>();
				foreach (Bag child in b.children.Keys)
					q.Enqueue(child);

				while (q.Count > 0)
				{
					Bag b2 = q.Dequeue();
					visited.Add(b2);
					if (b2.color == "shiny gold bag")
					{
						c.Add(b);
						break;
					}
					foreach (Bag child in b2.children.Keys)
					{
						if (!visited.Contains(child))
							q.Enqueue(child);					
					}
				}
			}
			part1 = c.Count;

			Bag goldbag = allBags.FirstOrDefault(x => x.color == "shiny gold bag");
			int part2 = p2(goldbag)-1;

			Console.WriteLine($"Part 1: {part1}");
			Console.WriteLine($"Part 2: {part2}");

		}

		int p2(Bag root)
		{
			if (root.children.Keys.Count == 0)
				return 1;

			int c = 1;
			foreach (Bag b in root.children.Keys)
			{
				c += p2(b)*root.children[b];
			}
			return c;
		}
	}
}

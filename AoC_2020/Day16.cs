using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Field
	{
		public string name;
		public Tuple<int, int> range1;
		public Tuple<int, int> range2;

		public Field(string name, Tuple<int, int> range1, Tuple<int, int> range2)
		{
			this.name = name;
			this.range1 = range1;
			this.range2 = range2;
		}

		public override string ToString()
		{
			return name;
		}
	}

	class Day16 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day16.txt");
			List<string> lines = new List<string>();
			/*while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());*/



			string[] sections = sr.ReadToEnd().Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			List<Field> fields = new List<Field>();

			foreach (string l in sections[0].Split('\n'))
			{
				Match m = Regex.Match(l, @"(.+):.?(\d+)-(\d+) or (\d+)-(\d+)");
				Field f = new Field(m.Groups[1].Value, 
					new Tuple<int, int>(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value)),
					new Tuple<int, int>(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value))
					);

				fields.Add(f);
			}

			List<int> yourTicket = new List<int>();
			MatchCollection mc = Regex.Matches(sections[1], @"\d+");
			foreach (Match m in mc)
				yourTicket.Add(int.Parse(m.Value));

			List<List<int>> nearby = new List<List<int>>();
			foreach (string l in sections[2].Split('\n'))
			{
				mc = Regex.Matches(l, @"\d+");
				List<int> li = new List<int>();
				foreach (Match m in mc)
					li.Add(int.Parse(m.Value));

				if (li.Count > 0)
					nearby.Add(li);
			}

			int part1 = 0;
			List<List<int>> validTickets = new List<List<int>>();
			foreach (List<int> ticket in nearby)
			{
				bool isValidTicket = true;
				foreach (int n in ticket)
				{
					bool isValidField = false;
					foreach (Field f in fields)
					{
						if ((n >= f.range1.Item1 && n <= f.range1.Item2) ||
							(n >= f.range2.Item1 && n <= f.range2.Item2))
						{
							isValidField = true;
							break;
						}
					}

					if (!isValidField)
					{
						part1 += n;
						isValidTicket = false;
					}
				}

				if (isValidTicket)
					validTickets.Add(ticket);

			}

			Console.WriteLine($"Part 1: {part1}");

			List<Field> candidates = new List<Field>(fields);
			Dictionary<int, Field> indexFieldMap = new Dictionary<int, Field>();

			Dictionary<int, List<Field>> columnPossibilites = new Dictionary<int, List<Field>>();

			while (candidates.Count > 0)
			{
				for (int i = 0; i < yourTicket.Count; i++)
				{
					if (indexFieldMap.ContainsKey(i))
						continue;

					if (!columnPossibilites.ContainsKey(i))
						columnPossibilites.Add(i, new List<Field>());
					else
						columnPossibilites[i].Clear();

					foreach (Field f in candidates)
					{
						bool valid = true;
						foreach (List<int> ticket in validTickets)
						{
							if (!((ticket[i] >= f.range1.Item1 && ticket[i] <= f.range1.Item2)
								||
								(ticket[i] >= f.range2.Item1 && ticket[i] <= f.range2.Item2)
								))
							{
								valid = false;
								break;
							}
						}
						if (valid)
						{
							columnPossibilites[i].Add(f);
						}
					}
				}

				KeyValuePair<int, List<Field>> min = columnPossibilites.OrderBy(x => x.Value.Count).First();
				indexFieldMap.Add(min.Key, min.Value[0]);
				candidates.Remove(min.Value[0]);
				columnPossibilites.Remove(min.Key);
				/*foreach (int i in columnPossibilites.Keys)
				{
					Console.Write($"Column {i} has {columnPossibilites[i].Count} possibilities: ");
					foreach (Field pos in columnPossibilites[i])
						Console.Write(pos.name + ", ");

					Console.Write("\n");
				}
				Console.Write("\n\n\n");*/
			}

			foreach (int i in indexFieldMap.Keys)
			{
				Console.WriteLine($"Column {i} is {indexFieldMap[i].name}");
			}

			/*for (int i=0; i < yourTicket.Count;i++)
			{
				Field f = indexFieldMap[i];
				if (!((yourTicket[i] >= f.range1.Item1 && yourTicket[i] <= f.range1.Item2)
						||
						(yourTicket[i] >= f.range2.Item1 && yourTicket[i] <= f.range2.Item2)
						))
				{
					Console.WriteLine("Your ticket is invalid");
					break;
				}
				else
				{
					Console.WriteLine($"Field {f.name} at index {i} in your ticket is valid");
				}
			}*/

			long sum2 = 1;
			Console.WriteLine("Your Ticket:");
			for (int i=0; i < yourTicket.Count;i++)
			{
				if (indexFieldMap[i].name.Contains("departure"))
					sum2 *= yourTicket[i];

				Console.WriteLine($"{indexFieldMap[i].name}: {yourTicket[i]}");
			}

			Console.WriteLine($"Part 2: {sum2}");
		}
	}
}

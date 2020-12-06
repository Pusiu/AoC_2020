using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day06 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day06.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			int sum = 0;
			int sum2 = 0;
			int lcount = 0;
			List<char> h = new List<char>();
			foreach (string line in lines)
			{
				if (line == "")
				{
					sum += h.Distinct().Count();
					sum2 += h.GroupBy(x=>x).OrderByDescending(x => x.Count()).Where(x => x.Count() == lcount).Count();
					h.Clear();
					lcount = 0;
				}
				else
				{
					for (int i = 0; i < line.Length; i++)
						h.Add(line[i]);

					lcount++;
				}
			}

			Console.WriteLine($"Part 1: {sum}");
			Console.WriteLine($"Part 2: {sum2}");
		}
	}
}

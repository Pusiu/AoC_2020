using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day10 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day10.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			int sum1 = 0;
			int sum3 = 0;

			List<int> adapters = new List<int>(lines.Select(x => int.Parse(x)));
			int maxAdapter = adapters.Max();

			int curAdapter = 0;
			while (adapters.Count > 0)
			{
				int newAdapt = adapters.Where(x => x - curAdapter <= 3).Min();
				//Console.Write($"Next adapt: {newAdapt}, diff={newAdapt-curAdapter}");

				if (newAdapt - curAdapter == 1)
					sum1++;

				if (newAdapt - curAdapter == 3)
					sum3++;

				//Console.Write($" sum1={sum1} sum3={sum3}\n");

				adapters.Remove(newAdapt);
				curAdapter = newAdapt;
			}
			sum3++;
			Console.WriteLine($"Part1: {sum1*sum3}, 1 differences={sum1}, 3 differences={sum3}, maxAdapter = {maxAdapter}, device jolts = {maxAdapter+3}");

			adapters = new List<int>(lines.Select(x => int.Parse(x)).OrderBy(x => x));
			adapters.Insert(0,0);
			int m = adapters.Max();
			adapters.Insert(adapters.IndexOf(m)+1, m + 3);

			Dictionary<int, long> paths = new Dictionary<int, long>();
			foreach (int a in adapters)
				paths.Add(a, 0);
			paths[0]=1;

			foreach (int adapter in adapters)
			{
				for (int i=1; i < 4; i++)
				{
					int nextAdapter = adapter + i;
					if (adapters.Contains(nextAdapter))
						paths[nextAdapter] += paths[adapter];
				}
			}
			Console.WriteLine($"Part 2: {paths[adapters.Max()]}");

		}

	}
}

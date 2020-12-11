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

			List<int> everyThree = new List<int>();
			foreach (int a in adapters)
			{

			}

			Console.WriteLine($"Part 2: {p2(0,adapters)}");
		}

		Dictionary<int, int> paths = new Dictionary<int, int>();
		int p2(int k, List<int> a)
		{
			int p = 0;
			int i = a.IndexOf(k);
			for (int j=1; j <=3; j++)
			{
				if (i+j < a.Count)
				{
					if (a[i + j] - k == 3 ||
						a[i + j] - k == 2 ||
						a[i + j] - k == 1)
						p++;
				}
			}
			return p;

		}
	}
}

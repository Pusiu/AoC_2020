using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day09 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day09.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			int r = 25;
			int part1 = 0;

			List<int> previous = new List<int>();
			previous.AddRange(lines.Take(r).Select(x => int.Parse(x)));

			for (int i = r; i < lines.Count; i++)
			{
				int n = int.Parse(lines[i]);
				bool isGood = false;

				for (int j = 0; j < previous.Count; j++)
				{
					for (int k = 0; k < previous.Count; k++)
					{
						if (k == j)
							continue;

						if (previous[j] + previous[k] == n)
						{
							isGood = true;
							break;
						}
					}
				}

				previous.RemoveAt(0);
				previous.Add(n);

				if (!isGood)
				{
					part1 = n;
					Console.WriteLine($"Part 1: {n}");
					break;
				}
			}

			int range = 2;
			while (range < lines.Count)
			{
				for (int i=0; i < lines.Count - range;i++)
				{
					long sum = 0;
					for (int j=0;j<range;j++)
					{
						sum += long.Parse(lines[i+j]);
					}
					if (sum == part1)
					{
						var l = lines.Skip(i).Take(range).Select(x => long.Parse(x));
						Console.WriteLine($"Part 2: sum={sum} range={range} min={l.Min()} max={l.Max()} add={l.Min()+l.Max()}");
						return;
					}
				}

				range++;
			}
		}
	}
}

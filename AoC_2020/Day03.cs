using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day03 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day03.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());


			Dictionary<Tuple<int, int>, int> slopeTreesMap = new Dictionary<Tuple<int, int>, int>{
				{ new Tuple<int, int>(1,1), 0},
				{ new Tuple<int, int>(3,1), 0},
				{ new Tuple<int, int>(5,1), 0},
				{ new Tuple<int, int>(7,1), 0},
				{ new Tuple<int, int>(1,2), 0},
			};


			foreach (Tuple<int,int> slope in slopeTreesMap.Keys.ToList())
			{
				int xStep = slope.Item1;
				int yStep = slope.Item2;

				int x = 0;
				int y = 0;
				int trees = 0;
				do
				{
					x += xStep;
					y += yStep;

					if (y >= lines.Count)
						break;

					while (lines[y].Length <= x)
					{
						lines[y] += lines[y];
					}

					if (lines[y][x] == '#')
						trees++;

				} while (y < lines.Count - 1);
				slopeTreesMap[slope] = trees;
			}

			Console.WriteLine($"Part 1: {slopeTreesMap[new Tuple<int, int>(3,1)]}");
			Console.WriteLine($"Part 2: {slopeTreesMap.Values.Aggregate((a, x) => a * x)}");
		}
	}
}

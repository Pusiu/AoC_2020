using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day08 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day08.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			HashSet<int> indices = new HashSet<int>();

			bool part1Found = false;

			for (int j = -1; j < lines.Count; j++)
			{
				List<string> copy = new List<string>(lines);
				if (j >= 0)
				{
					string[] l = copy[j].Split(' ');
					if (l[0] == "acc")
						continue;

					copy[j] = ((l[0] == "nop") ? "jmp " : "nop ") + l[1];
				}


				int acc = 0;
				int i;
				int maxIters = copy.Count * 5;
				for (i = 0; i < copy.Count;)
				{
					if (!part1Found)
					{
						if (!indices.Add(i))
						{
							Console.WriteLine($"Part 1: {acc}");
							part1Found = true;
						}
					}

					string[] l = copy[i].Split(' ');
					switch (l[0])
					{
						case "acc":
							acc += int.Parse(l[1]);
							i++;
							break;
						case "nop":
							i++;
							break;
						case "jmp":
							i += int.Parse(l[1]);
							break;
					}
					if (maxIters-- <= 0)
						break;
				}

				if (i >= copy.Count)
				{
					Console.WriteLine($"Part 2: {acc}, changed index={j}");
				}
			}
		}
	}
}

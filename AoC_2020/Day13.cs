using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day13 : Day
	{
		List<string> lines = new List<string>();
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day13.txt");
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			int timestamp = int.Parse(lines[0]);
			Dictionary<int, int> buses = lines[1].Split(new char[] { ',' }).Where(x => x != "x").Select(k => int.Parse(k)).ToDictionary(x => x);
			long t = 0;
			List<int> keys = new List<int>(buses.Keys);
			int closest = -1;
			while (true)
			{
				if (closest > 0)
				{
					Console.WriteLine($"Part1: Closest bus: {closest}, departing at {t-1}, you have to wait {t-1-timestamp} min, multiplied by busID it's {(t-1-timestamp)*closest}\n");
					break;
				}
				if (t >= timestamp)
				{
					if (t == timestamp)
					{
						Console.Write("time\t");
						foreach (int busID in buses.Keys)
							Console.Write($"bus {busID}\t");
						Console.Write("\n");
					}
					Console.Write(t.ToString() + "\t");
					foreach (int busID in keys)
					{
						Console.Write($"{((t % busID == 0) ? "D" : ".")}\t");
						if (t % busID == 0)
						{
							closest = busID;
						}
					}
					Console.Write("\n");
				}

				foreach (int busID in keys)
				{
					if (t % busID == 0)
						buses[busID] += busID;
				}
				t++;
			}

			part2();
		}

		void PrintBuses(decimal t, int c, List<Tuple<int,int>> buses)
		{
			Console.Write("time\t");
			foreach (int busID in buses.Select(x => x.Item1))
				Console.Write($"bus {busID}\t");
			Console.Write("\n");

			for (int i=0; i < c; i++)
			{
				Console.Write(t.ToString() + "\t");
				foreach (int busID in buses.Select(x => x.Item1))
				{
					Console.Write($"{((t % busID == 0) ? "D" : ".")}\t");
				}
				Console.Write("\n");
				t++;
			}
		}

		void part2()
		{
			string[] b = lines[1].Split(new char[] { ',' });
			List<Tuple<int, int>> buses = new List<Tuple<int, int>>(); //arrival time, offset time
			for (int i = 0; i < b.Length; i++)
				if (b[i] != "x")
					buses.Add(new Tuple<int, int>(int.Parse(b[i]), i));
			
			ulong t = 1;
			ulong step = 1;
			ulong mul = (ulong)buses[0].Item1;
			for (int i=1; i < buses.Count;i++)
			{
				Tuple<int, int> bus = buses[i];
				while (true)
				{
					t += step;
					if ((t % (ulong)buses[0].Item1 == 0) && ((t + (ulong)bus.Item2) % (ulong)bus.Item1 == 0))
					{
						mul *= (ulong)bus.Item1;
						step = mul;

						if (i == buses.Count-1)
						{
							PrintBuses(t - 3, 15, buses);
							Console.WriteLine($"Part 2: {t}");
						}
						break;
					}
				}
			}

		}
	}
}


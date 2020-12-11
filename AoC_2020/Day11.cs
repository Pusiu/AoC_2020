using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day11 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day11.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());


			char[,] map = new char[lines.Count, lines[0].Length];

			for (int y = 0; y < lines.Count; y++)
				for (int x = 0; x < lines[y].Length; x++)
					map[y, x] = lines[y][x];

			//PrintMap(map);
			bool changed = true;
			char[,] lastMap = map;
			while (changed)
			{
				char[,] newMap = lastMap.Clone() as char[,];
				changed = false;
				for (int y = 0; y < lastMap.GetLength(0); y++)
				{
					for (int x = 0; x < lastMap.GetLength(1); x++)
					{
						switch (lastMap[y,x])
						{
							case '#':
								if (GetAdjecentOccupiedCount(x, y, lastMap) >= 4)
								{
									newMap[y, x] = 'L';
									changed = true;
								}
								break;
							case 'L':
								if (GetAdjecentOccupiedCount(x, y, lastMap) == 0)
								{
									newMap[y, x] = '#';
									changed = true;
								}
								break;
						}
					}
				}
				lastMap = newMap.Clone() as char[,];
				//PrintMap(newMap);
				
			}

			int part1 = 0;
			for (int y = 0; y < lastMap.GetLength(0); y++)
				for (int x = 0; x < lastMap.GetLength(1); x++)
					if (lastMap[y, x] == '#')
						part1++;

			Console.WriteLine($"Part 1: {part1}");

			changed = true;
			lastMap = map;
			while (changed)
			{
				char[,] newMap = lastMap.Clone() as char[,];
				changed = false;
				for (int y = 0; y < lastMap.GetLength(0); y++)
				{
					for (int x = 0; x < lastMap.GetLength(1); x++)
					{
						switch (lastMap[y, x])
						{
							case '#':
								if (GetAdjecentOccupiedCount2(x, y, lastMap) >= 5)
								{
									newMap[y, x] = 'L';
									changed = true;
								}
								break;
							case 'L':
								if (GetAdjecentOccupiedCount2(x, y, lastMap) == 0)
								{
									newMap[y, x] = '#';
									changed = true;
								}
								break;
						}
					}
				}
				lastMap = newMap.Clone() as char[,];
				//PrintMap(newMap);
			}

			int part2 = 0;
			for (int y = 0; y < lastMap.GetLength(0); y++)
				for (int x = 0; x < lastMap.GetLength(1); x++)
					if (lastMap[y, x] == '#')
						part2++;

			Console.WriteLine($"Part 2: {part2}");

		}

		int GetAdjecentOccupiedCount(int x, int y, char[,] map)
		{
			int c = 0;

			if (x - 1 >= 0 && map[y, x-1] == '#')
				c++;
			if (x + 1 < map.GetLength(1) && map[y, x+1] == '#')
				c++;
			if (y - 1 >= 0 && map[y-1, x] == '#')
				c++;
			if (y + 1 < map.GetLength(0) && map[y+1, x] == '#')
				c++;

			if (x - 1 >= 0 && y - 1 >= 0 && map[y-1, x-1] == '#')
				c++;
			if (x + 1 < map.GetLength(1) && y - 1 >= 0 && map[y-1, x+1] == '#')
				c++;
			if (x - 1 >= 0 && y + 1 < map.GetLength(0) && map[y+1, x-1] == '#')
				c++;
			if (x + 1 < map.GetLength(1) && y + 1 < map.GetLength(0) && map[y+1, x+1] == '#')
				c++;

			return c;
		}

		int GetAdjecentOccupiedCount2(int x, int y, char[,] map)
		{
			int c = 0;

			List<Tuple<int, int>> directions = new List<Tuple<int, int>>()
			{
				new Tuple<int, int>(1,0),
				new Tuple<int, int>(0,1),
				new Tuple<int, int>(-1,0),
				new Tuple<int, int>(0,-1),
				new Tuple<int, int>(1,1),
				new Tuple<int, int>(-1,1),
				new Tuple<int, int>(1,-1),
				new Tuple<int, int>(-1,-1)
			};

			while (directions.Count > 0)
			{
				Tuple<int, int> dir = directions[0];
				directions.RemoveAt(0);
				int i = 1;
				while (true)
				{
					if (x + dir.Item1 * i < 0 ||
						x + dir.Item1 * i >= map.GetLength(1) ||
						y + dir.Item2 * i < 0 ||
						y + dir.Item2 * i >= map.GetLength(0)
						)
						break;

					char k = map[y + dir.Item2 * i, x + dir.Item1 * i];
					if (k == '#')
					{
						c++;
						break;
					}
					else if (k == 'L')
						break;

					i++;
				}
			}

			return c;
		}

		void PrintMap(char[,] m)
		{
			for (int y = 0; y < m.GetLength(0); y++)
			{
				for (int x = 0; x < m.GetLength(1); x++)
					Console.Write(m[y, x]);

				Console.Write("\n");
			}
			Console.WriteLine("\n");
		}
	}
}

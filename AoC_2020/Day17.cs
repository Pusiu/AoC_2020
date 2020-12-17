using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day17 : Day
	{
		List<List<List<char>>> currentMap;
		List<List<List<List<char>>>> currentMap2; //w,z,y,x
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day17.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			List<List<List<char>>> map = new List<List<List<char>>>(); //z,y,x
			List<List<List<List<char>>>> map2 = new List<List<List<List<char>>>>(); //z,y,x
			map.Add(new List<List<char>>());
			map2.Add(new List<List<List<char>>>());
			map2[0].Add(new List<List<char>>());
			for (int i=0; i < lines.Count;i++)
			{
				map[0].Add(new List<char>());
				map2[0][0].Add(new List<char>());
				for (int j = 0; j < lines[i].Length; j++)
				{
					map2[0][0][i].Add(lines[i][j]);
					map[0][i].Add(lines[i][j]);
				}
			}
			map2[0].Insert(0, new List<List<char>>(Enumerable.Repeat(Enumerable.Repeat('.', map2[0][0][0].Count).ToList(), map2[0][0].Count).ToList()));
			map2[0].Add(new List<List<char>>(Enumerable.Repeat(Enumerable.Repeat('.', map2[0][0][0].Count).ToList(), map2[0][0].Count).ToList()));
			//PrintMap(map);

			for (int i=0; i < 6; i++)
			{
				map = ResizeMap(map);

				//map = CopyMap(currentMap);

				/*Console.WriteLine("Before: ");
				PrintMap(map);*/

				currentMap = CopyMap(map);

				for (int z = 0; z < map.Count; z++)
					for (int y = 0; y < map[0].Count; y++)
						for (int x=0; x < map[0][0].Count;x++)
						{
							int a = GetActiveNeighbours(x, y, z, map);
							//if (map[x, y, z] == '#')
							if (map[z][y][x] == '#')
							{
								if (!(a == 2 || a == 3))
									//newMap[x, y, z] = '.';
									currentMap[z] [y] [x] = '.';
							}
							else
							{
								if (a == 3)
									//newMap[x, y, z] = '#';
									currentMap[z][y][x] = '#';
							}
							
						}

				//map = new List<List<List<char>>>(currentMap);
				map = CopyMap(currentMap);
				/*Console.WriteLine("After " + (i+1) + " cycle ");
				PrintMap(map);*/
			}

			int sum = 0;
			for (int z = 0; z < map.Count; z++)
				for (int y = 0; y < map[0].Count; y++)
					for (int x = 0; x < map[0][0].Count; x++)
						if (map[z][y][x] == '#')
							sum++;

			Console.WriteLine($"Part1: {sum}");

			for (int i = 0; i < 6; i++)
			{
				map2 = ResizeMap(map2);

				currentMap2 = CopyMap(map2);

				for (int w = 0; w < map2.Count; w++)
				{
					for (int z = 0; z < map2[0].Count; z++)
						for (int y = 0; y < map2[0][0].Count; y++)
							for (int x = 0; x < map2[0][0][0].Count; x++)
							{
								int a = GetActiveNeighbours(x, y, z,w, map2);
								//if (map[x, y, z] == '#')
								if (map2[w][z][y][x] == '#')
								{
									if (!(a == 2 || a == 3))
										//newMap[x, y, z] = '.';
										currentMap2[w][z][y][x] = '.';
								}
								else
								{
									if (a == 3)
										//newMap[x, y, z] = '#';
										currentMap2[w][z][y][x] = '#';
								}

							}
				}
				//map = new List<List<List<char>>>(currentMap);
				map2 = CopyMap(currentMap2);
				/*Console.WriteLine("After " + (i+1) + " cycle ");
				PrintMap(map);*/
			}


			sum = 0;
			sum = map2.Sum(w => w.Sum(z => z.Sum(y => y.Count(x => x == '#'))));

			/*for (int z = 0; z < map.Count; z++)
				for (int y = 0; y < map[0].Count; y++)
					for (int x = 0; x < map[0][0].Count; x++)
						if (map[z][y][x] == '#')
							sum++;*/

			Console.WriteLine($"Part 2: {sum}");
		}

		List<List<List<char>>> ResizeMap(List<List<List<char>>> source)
		{
			List<List<List<char>>> n = CopyMap(source);

			for (int j = 0; j < source.Count; j++)
			{
				//resize left-right
				for (int k = 0; k < source[j].Count; k++)
				{
					n[j][k].Insert(0, '.');
					n[j][k].Add('.');
				}
				//resize front-back
				n[j].Insert(0, new List<char>(Enumerable.Repeat('.', n[j][0].Count).ToList()));
				n[j].Add(new List<char>(Enumerable.Repeat('.', n[j][0].Count).ToList()));
			}
			//resize up-down layers
			n.Insert(0, new List<List<char>>(Enumerable.Repeat(Enumerable.Repeat('.', n[0][0].Count).ToList(), n[0].Count).ToList()));
			n.Add(new List<List<char>>(Enumerable.Repeat(Enumerable.Repeat('.', n[0][0].Count).ToList(), n[0].Count).ToList()));

			return n;
		}

		List<List<List<List<char>>>> ResizeMap(List<List<List<List<char>>>> source)
		{
			List<List<List<List<char>>>> n = CopyMap(source);
			for (int l = 0; l < source.Count; l++)
			{
				for (int j = 0; j < source[0].Count; j++)
				{
					//resize left-right
					for (int k = 0; k < source[0][j].Count; k++)
					{
						n[l][j][k].Insert(0, '.');
						n[l][j][k].Add('.');
					}
					//resize front-back
					n[l][j].Insert(0, new List<char>(Enumerable.Repeat('.', n[l][j][0].Count).ToList()));
					n[l][j].Add(new List<char>(Enumerable.Repeat('.', n[l][j][0].Count).ToList()));
				}
				//resize up-down layers
				n[l].Insert(0, new List<List<char>>(Enumerable.Repeat(Enumerable.Repeat('.', n[0][0][0].Count).ToList(), n[0][0].Count).ToList()));
				n[l].Add(new List<List<char>>(Enumerable.Repeat(Enumerable.Repeat('.', n[0][0][0].Count).ToList(), n[0][0].Count).ToList()));
			}
			//resize 4 dim layers
			n.Insert(0, new List<List<List<char>>>(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat('.', n[0][0][0].Count).ToList(), n[0][0].Count).ToList(), n[0].Count).ToList()));
			n.Add(new List<List<List<char>>>(Enumerable.Repeat(Enumerable.Repeat(Enumerable.Repeat('.', n[0][0][0].Count).ToList(), n[0][0].Count).ToList(), n[0].Count).ToList()));

			return n;
		}

		List<List<List<char>>> CopyMap(List<List<List<char>>> source)
		{
			List<List<List<char>>> n = new List<List<List<char>>>();
			for (int z=0; z < source.Count;z++)
			{
				n.Add(new List<List<char>>());
				for (int y = 0; y < source[0].Count; y++)
				{
					n[z].Add(new List<char>());
					for (int x = 0; x < source[0][0].Count; x++)
					{
						n[z][y].Add(source[z][y][x]);
					}
				}
			}
			return n;
		}

		List<List<List<List<char>>>> CopyMap(List<List<List<List<char>>>> source)
		{
			List<List<List<List<char>>>> n = new List<List<List<List<char>>>>();
			for (int w = 0; w < source.Count; w++)
			{
				n.Add(new List<List<List<char>>>());
				for (int z = 0; z < source[0].Count; z++)
				{
					n[w].Add(new List<List<char>>());
					for (int y = 0; y < source[0][0].Count; y++)
					{
						n[w][z].Add(new List<char>());
						for (int x = 0; x < source[0][0][0].Count; x++)
						{
							n[w][z][y].Add(source[w][z][y][x]);
						}
					}
				}
			}
			return n;
		}

		void PrintMap(List<List<List<char>>> map)
		{
			for (int z = 0; z < map.Count; z++)
			{
				Console.WriteLine("z=" + z);
				for (int y = 0; y < map[0].Count; y++)
				{
					for (int x = 0; x < map[0][0].Count; x++)
					{
						Console.Write(map[z] [y] [x]);
					}
					Console.Write('\n');
				}
			}
		}

		int GetActiveNeighbours(int x, int y, int z, List<List<List<char>>> map)
		{
			int c = 0;

			for (int zz = -1; zz < 2; zz++)
				for (int yy = -1; yy < 2; yy++)
					for (int xx=-1; xx < 2; xx++)
					{
						if (x == x+xx && y == y+yy && z == z+zz)
							continue;

						if ((z + zz >= 0 && z + zz < map.Count) &&
							(y + yy >= 0 && y + yy < map[0].Count) &&
							(x + xx >= 0 && x + xx < map[0][0].Count) 
							)
						{
							if (map[z+zz] [y + yy] [x + xx] == '#')
								c++;
						}
					}

			return c;
		}

		int GetActiveNeighbours(int x, int y, int z, int w, List<List<List<List<char>>>> map)
		{
			int c = 0;

			for (int ww = -1; ww < 2; ww++)
			{
				for (int zz = -1; zz < 2; zz++)
					for (int yy = -1; yy < 2; yy++)
						for (int xx = -1; xx < 2; xx++)
						{
							if (x == x + xx && y == y + yy && z == z + zz && w == w + ww)
								continue;

							if ((w + ww >= 0 && w + ww < map.Count) &&
								(z + zz >= 0 && z + zz < map[0].Count) &&
								(y + yy >= 0 && y + yy < map[0][0].Count) &&
								(x + xx >= 0 && x + xx < map[0][0][0].Count)
								)
							{
								if (map[w + ww][z + zz][y + yy][x + xx] == '#')
									c++;
							}
						}
			}
			return c;
		}
	}
}


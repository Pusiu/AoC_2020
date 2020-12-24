using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class HexTile
	{
		public bool isBlack = false;
		public enum Direction { E=0,SE=1,SW=2,W=3,NW=4,NE=5};

		public int x = 0;
		public int y = 0;

		public HexTile(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public HexTile(HexTile h)
		{
			x = h.x;
			y = h.y;
			isBlack = h.isBlack;
		}

		public override string ToString()
		{
			return $"{((isBlack) ? "Black" : "White")} tile at {x} {y}";
		}
	}

	class Day24 : Day
	{
		List<Tuple<int, int>> adjecentDirs = new List<Tuple<int, int>>()
			{
				new Tuple<int, int>(1,-1),
				new Tuple<int, int>(1,0),
				new Tuple<int, int>(0,1),
				new Tuple<int, int>(0,-1),
				new Tuple<int, int>(-1,0),
				new Tuple<int, int>(-1,1)
			};

		Dictionary<Tuple<int, int>, HexTile> tilemap = new Dictionary<Tuple<int, int>, HexTile>();
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day24.txt");
			List<string> lines = new List<string>();
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			HexTile referenceTile = new HexTile(0,0);
			tilemap.Add(new Tuple<int, int>(referenceTile.x, referenceTile.y), referenceTile);

			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				lines.Add(line);
				Match m;
				HexTile currentTile = referenceTile;
				do
				{
					m = Regex.Match(line, @"(se)|(ne)|(sw)|(nw)|(e)|(w)");
					if (m.Success)
					{
						HexTile.Direction dir = (HexTile.Direction)Enum.Parse(typeof(HexTile.Direction), m.Value.ToUpper());
						int x = currentTile.x;
						int y = currentTile.y;
						switch (dir)
						{
							case HexTile.Direction.NE:
								x++;
								y--;
								break;
							case HexTile.Direction.E:
								x++;
								break;
							case HexTile.Direction.SE:
								y++;
								break;
							case HexTile.Direction.NW:
								y--;
								break;
							case HexTile.Direction.W:
								x--;
								break;
							case HexTile.Direction.SW:
								x--;
								y++;
								break;
						}
						var pos = new Tuple<int, int>(x, y);
						if (!tilemap.ContainsKey(pos))
						{
							HexTile t = new HexTile(pos.Item1,pos.Item2);
							tilemap.Add(pos, t);
						}

						currentTile = tilemap[pos];
					}
					line = line.Substring(m.Value.Length);
				} while (m.Success);
				currentTile.isBlack = !currentTile.isBlack;
			}

			Console.WriteLine($"Part 1: {tilemap.Values.Count(x => x.isBlack)}");

			for (int i=1; i <= 100; i++)
			{
				HashSet<Tuple<int, int>> tilesToFill = new HashSet<Tuple<int, int>>();
				foreach (var k in tilemap.Keys.ToList())
				{
					foreach (var d in adjecentDirs)
					{
						var pos = new Tuple<int, int>(k.Item1 + d.Item1, k.Item2 + d.Item2);
						if (!tilemap.ContainsKey(pos))
							tilesToFill.Add(pos);
					}
				}
				foreach (var t in tilesToFill)
				{
					tilemap.Add(t, new HexTile(t.Item1, t.Item2));
				}

				var toFlip = new List<Tuple<int, int>>();

				foreach (var k in tilemap.Keys)
				{
					int c = GetAdjecentBlackTileCount(tilemap[k]);
					if (tilemap[k].isBlack)
					{
						if (c == 0 || c > 2)
							toFlip.Add(k);
					}
					else
					{
						if (c == 2)
							toFlip.Add(k);
					}
				}

				foreach (var k in toFlip)
					tilemap[k].isBlack = !tilemap[k].isBlack;

				Console.WriteLine($"Day {i}: {tilemap.Values.Count(x => x.isBlack)}");
			}
			Console.WriteLine($"Part 2: {tilemap.Values.Count(x => x.isBlack)}");
		}

		int GetAdjecentBlackTileCount(HexTile t)
		{
			int count = 0;
			int total = 0;
			
			foreach (Tuple<int,int> dir in adjecentDirs)
			{
				var pos = new Tuple<int, int>(t.x + dir.Item1, t.y + dir.Item2);
				if (tilemap.ContainsKey(pos))
				{
					total++;
					if (tilemap[pos].isBlack)
						count++;
				}
			}

			return count;
		}
	}
}

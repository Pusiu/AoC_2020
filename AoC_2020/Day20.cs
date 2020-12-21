using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class MapTile
	{
		public static int tileSize = 10;

		public int tileId;
		public int x = -5;
		public int y = -5;
		public char[,] pixels = new char[tileSize, tileSize]; //row, column

		public enum Direction {Top=0, Right=1, Bottom=2, Left = 3};
		public Dictionary<Direction, MapTile> adjecentTiles = new Dictionary<Direction, MapTile>();
		public bool isAligned = false;

		public Direction currentRotation = Direction.Top;

		public enum FlipEnum { NoFlip=0, FlipX=1, FlipY=2, FlipXY=3};
		public FlipEnum currentFlip = FlipEnum.NoFlip;

		public MapTile()
		{
			foreach (Direction d in Enum.GetValues(typeof(Direction)))
			{
				adjecentTiles.Add(d, null);
			}
		}

		public void AlignToOther(Direction dir, MapTile other)
		{
			isAligned = true;
			adjecentTiles[dir] = other;
		}

		public MapTile(MapTile copy)
		{
			tileId = copy.tileId;
			for (int y = 0; y < tileSize; y++)
				for (int x = 0; x < tileSize; x++)
					pixels[y,x] = copy.pixels[y,x];
		}

		public void Rotate(bool right)
		{

			char[,] newMap = new char[tileSize, tileSize];
			for (int y=0; y < tileSize; y++)
				for (int x = 0; x < tileSize; x++)
				{
					if (right)
						newMap[y, x] = pixels[tileSize - x - 1, y];
					else
						newMap[y, x] = pixels[x, tileSize - y - 1];
				}

			pixels = newMap;
			currentRotation = (Direction)(((int)currentRotation + 1) % 3);
		}

		public void Flip(bool flipx)
		{
			char[,] newMap = new char[tileSize, tileSize];
			for (int y = 0; y < tileSize; y++)
				for (int x = 0; x < tileSize; x++)
				{
					if (flipx)
						newMap[y, x] = pixels[y, tileSize-x-1];
					else
						newMap[y, x] = pixels[tileSize - y - 1, x];
				}

			pixels = newMap;
			if (flipx)
			{
				if (currentFlip == FlipEnum.NoFlip)
					currentFlip = FlipEnum.FlipX;
				else if (currentFlip == FlipEnum.FlipX)
					currentFlip = FlipEnum.NoFlip;
				else if (currentFlip == FlipEnum.FlipY)
					currentFlip = FlipEnum.FlipXY;
				else if (currentFlip == FlipEnum.FlipXY)
					currentFlip = FlipEnum.FlipY;
			}
			else
			{
				if (currentFlip == FlipEnum.NoFlip)
					currentFlip = FlipEnum.FlipY;
				else if (currentFlip == FlipEnum.FlipY)
					currentFlip = FlipEnum.NoFlip;
				else if (currentFlip == FlipEnum.FlipX)
					currentFlip = FlipEnum.FlipXY;
				else if (currentFlip == FlipEnum.FlipXY)
					currentFlip = FlipEnum.FlipX;
			}
		}

		public char[] GetEdgeRowByDirection(Direction dir)
		{
			char[] edgeRow = new char[tileSize];
			for (int i = 0; i < tileSize; i++)
			{
				switch (dir)
				{
					case Direction.Left:
						edgeRow[i] = pixels[i, 0];
						break;
					case Direction.Top:
						edgeRow[i] = pixels[0, i];
						break;
					case Direction.Right:
						edgeRow[i] = pixels[i, tileSize - 1];
						break;
					case Direction.Bottom:
						edgeRow[i] = pixels[tileSize - 1, i];
						break;
				}
			}
			return edgeRow;
		}

		public static bool AreRowsEqual(char[] lhs, char[] rhs)
		{
			for (int i = 0; i < tileSize; i++)
			{
				if (lhs[i] != rhs[i])
					return false;
			}

			return true;
		}

		public void RemoveBorders()
		{
			char[,] n = new char[tileSize-2,tileSize-2];

			for (int y=1; y < tileSize-1; y++)
			{
				for (int x=1; x < tileSize-1;x++)
				{
					n[y-1, x-1] = pixels[y, x];
				}
			}
			pixels = n;
		}

		public override string ToString()
		{
			return "Tile " + tileId.ToString();
		}
	}

	class Day20 : Day
	{
		List<MapTile> allTiles = new List<MapTile>();
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day20.txt");

			MapTile t = null;
			int index = 0;
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();

				if (line.Contains("Tile"))
				{
					if (t != null)
						allTiles.Add(t);

					t = new MapTile();
					t.tileId = int.Parse(Regex.Match(line, @"\d+").Value);
					index = 0;
				}
				else
				{
					for (int i=0; i < line.Length;i++)
					{
						if (line[i] == '.' || line[i] == '#')
							t.pixels[index, i] = line[i];
					}
					index++;
				}
			}
			allTiles.Add(t);
			/*PrintTile(allTiles[0]);
			Console.WriteLine("");
			allTiles[0].Rotate(true);
			PrintTile(allTiles[0]);
			allTiles[0].Rotate(false);
			Console.WriteLine("");
			PrintTile(allTiles[0]);*/

			List<MapTile> freeTiles = new List<MapTile>(allTiles);
			Queue<MapTile> toCheck = new Queue<MapTile>();
			freeTiles[0].x = 0;
			freeTiles[0].y = 0;
			toCheck.Enqueue(freeTiles[0]);
			while (toCheck.Count > 0)
			{
				MapTile currentTile = toCheck.Dequeue();
				//freeTiles.Remove(currentTile);
				foreach (MapTile.Direction dir in currentTile.adjecentTiles.Keys.ToList())
				{
					if (currentTile.adjecentTiles[dir] != null)
						continue;

					char[] edgeRow = currentTile.GetEdgeRowByDirection(dir);
					foreach (MapTile otherTile in freeTiles)
					{
						bool isAligning = false;
						for (int flip =0; flip <= 3; flip++) //0=no flip, 1=flipx, 2=flipy, 3=flipxy
						{
							//MapTile copy = new MapTile(otherTile);
							if (flip == 1)
								otherTile.Flip(true);
							else if (flip == 2)
								otherTile.Flip(false);
							else if (flip == 3)
							{
								otherTile.Flip(true);
								otherTile.Flip(false);
							}

							for (int rot=0; rot <= 4; rot++)
							{
								MapTile.Direction otherDir = (MapTile.Direction)((((int)dir)+2) % 4);
								char[] otherRow = otherTile.GetEdgeRowByDirection(otherDir);
								if (MapTile.AreRowsEqual(edgeRow, otherRow))
								{
									isAligning = true;
									break;
								}
								otherTile.Rotate(true);
							}

							if (isAligning)
								break;
						}

						if (isAligning)
						{
							currentTile.adjecentTiles[dir] = otherTile;
							otherTile.adjecentTiles[(MapTile.Direction)((((int)dir) + 2) % 4)] = currentTile;
							freeTiles.Remove(otherTile);
							toCheck.Enqueue(otherTile);
							switch (dir)
							{
								case MapTile.Direction.Top:
									otherTile.y = currentTile.y - 1;
									otherTile.x = currentTile.x;
									break;
								case MapTile.Direction.Right:
									otherTile.x = currentTile.x + 1;
									otherTile.y = currentTile.y;
									break;
								case MapTile.Direction.Bottom:
									otherTile.y = currentTile.y + 1;
									otherTile.x = currentTile.x;
									break;
								case MapTile.Direction.Left:
									otherTile.x = currentTile.x - 1;
									otherTile.y = currentTile.y;
									break;
							}
							break;
						}
					}
				}
				if (currentTile.adjecentTiles.Count(x => x.Value == null) >= 3)
					freeTiles.Add(currentTile);
			}

			//PrintMap();

			int minY = allTiles.Min(x => x.y);
			int minX = allTiles.Min(x => x.x);
			int maxY = allTiles.Max(x => x.y);
			int maxX = allTiles.Max(x => x.x);

			ulong p1 = (ulong)allTiles.First(x => x.x == minX && x.y == minY).tileId *
					(ulong)allTiles.First(x => x.x == minX && x.y == maxY).tileId *
					(ulong)allTiles.First(x => x.x == maxX && x.y == minY).tileId *
					(ulong)allTiles.First(x => x.x == maxX && x.y == maxY).tileId;


			Console.WriteLine($"Part 1: {p1}");
			

			MapTile fullMap = new MapTile();
			fullMap.pixels = new char[(MapTile.tileSize-2) * (int)Math.Sqrt(allTiles.Count), (MapTile.tileSize-2) * (int)Math.Sqrt(allTiles.Count)];
			List<MapTile> orderedTiles = allTiles.OrderBy(x => x.x).OrderBy(x => x.y).ToList();
			int lastY = 0;
			int lastX = 0;
			foreach (MapTile mt in allTiles)
			{
				mt.RemoveBorders();
				mt.y -= minY;
				mt.x -= minX;
			}
			//PrintMap();
			for (int i=0; i < orderedTiles.Count;i++)
			{
				if (orderedTiles[i].y != lastY)
				{
					lastY = orderedTiles[i].y;
				}
				if (orderedTiles[i].x != lastX)
					lastX = orderedTiles[i].x;

				for (int j=0; j < orderedTiles[i].pixels.GetLength(0); j++)
					for (int l = 0; l < orderedTiles[i].pixels.GetLength(1); l++)
					{
						int yindx = lastY * (orderedTiles[i].pixels.GetLength(0)) + j;
						int xindx = lastX * (orderedTiles[i].pixels.GetLength(1)) + l;
						fullMap.pixels[yindx, xindx] = orderedTiles[i].pixels[j, l];
					}
			}



			//example map is rotated once right and flipped by x
			MapTile.tileSize = fullMap.pixels.GetLength(0);
			//fullMap.Rotate(true);
			//fullMap.Flip(true);


			CreateMonsterTemplate();
			int monsterCount = 0;
			int roughness = 0;
			bool monsterFound = false;
			for (int flip = 0; flip < 4; flip++)
			{
				if (monsterFound)
					break;

				if (flip == 1)
					fullMap.Flip(true);
				else if (flip == 2)
					fullMap.Flip(false);
				else if (flip == 3)
				{
					fullMap.Flip(true);
					fullMap.Flip(false);
				}

				for (int r = 0; r <= 4; r++)
				{
					if (monsterFound)
						break;

					fullMap.Rotate(true);
					roughness = 0;
					for (int y = 0; y < fullMap.pixels.GetLength(0); y++)
					{
						for (int x = 0; x < fullMap.pixels.GetLength(1); x++)
						{
							if (fullMap.pixels[y, x] == '#')
							{
								List<Tuple<int, int>> monsterPixels = IsMonster(fullMap, x, y);
								if (monsterPixels != null)
								{
									monsterFound = true;
									monsterCount++;
									foreach (Tuple<int, int> mp in monsterPixels)
										fullMap.pixels[mp.Item1, mp.Item2] = 'O';
								}
								else
									roughness++;
							}
						}
					}
				}
			}
			PrintMap(fullMap);

			Console.WriteLine($"Part 2: Monster count: {monsterCount}, roughness = {roughness}");
		}

		Dictionary<Tuple<int, int>, char> monsterTemplate = new Dictionary<Tuple<int, int>, char>();

		void CreateMonsterTemplate()
		{
			string[] t = new string[]
			{
				"                  # ",
				"#    ##    ##    ###",
				" #  #  #  #  #  #   "
			};

			int headStart = 18;
			for (int i=0; i < t.Length;i++)
			{
				for (int j = 0; j < t[i].Length; j++)
					monsterTemplate.Add(new Tuple<int, int>(i, j - headStart), t[i][j]);
			}
		}

		List<Tuple<int,int>> IsMonster(MapTile t, int headx, int heady)
		{
			//monster is 20 characters long
			List<Tuple<int, int>> tiles = new List<Tuple<int, int>>();
			if (t.pixels[heady, headx] != '#')
				return null;

			for (int y=0; y < 3; y++)
			{
				for (int x=-18;x < 2; x++)
				{
					if (!IsInMapBounds(headx+x, heady+y, t))
						return null;

					char expected = monsterTemplate[new Tuple<int, int>(y, x)];
					char atPixel = t.pixels[heady + y, headx + x];
					if (expected == '#')
					{
						if (atPixel != '#')
							return null;

						tiles.Add(new Tuple<int, int>(heady + y, headx + x));
					}
				}
			}

			return tiles;
		}

		bool IsInMapBounds(int x, int y, MapTile t)
		{
			if ((y >= 0 && y < t.pixels.GetLength(0)) &&
				(x >= 0 && x < t.pixels.GetLength(1))
				)
				return true;

			return false;
		}

		void PrintMap(MapTile t)
		{
			for (int y = 0; y < t.pixels.GetLength(0); y++)
			{
				for (int x = 0; x < t.pixels.GetLength(1); x++)
				{
					Console.Write(t.pixels[y, x]);
				}
				Console.Write("\n");
			}
			Console.Write("\n");
		}

		void PrintMap()
		{
			int minY = allTiles.Min(x => x.y);
			int maxX = allTiles.Max(x => x.x);
			int currentY = minY;
			int c = 0;
			while (true)
			{
				List<MapTile> currentLevelTiles = allTiles.Where(x => x.y == currentY).OrderBy(x => x.x).ToList();
				foreach (MapTile tile in currentLevelTiles)
					Console.Write(tile.ToString() + $" {tile.y} {tile.x}\t");
				Console.Write("\n");
				for (int y = 0; y < MapTile.tileSize; y++)
				{
					foreach (MapTile tile in currentLevelTiles)
					{
						if (y < tile.pixels.GetLength(0))
						{
							for (int x = 0; x < tile.pixels.GetLength(1); x++)
							{
								Console.Write(tile.pixels[y, x]);
							}
						}
						Console.Write("\t");
					}
					Console.Write("\n");
				}
				Console.Write("\n");
				c += currentLevelTiles.Count;
				if (c == allTiles.Count)
					break;

				currentY++;
			}
		}

		void PrintTile(MapTile t)
		{
			for (int y = 0; y < MapTile.tileSize; y++)
			{
				for (int x = 0; x < MapTile.tileSize; x++)
					Console.Write(t.pixels[y, x]);
				Console.Write("\n");
			}
		}
	}
}

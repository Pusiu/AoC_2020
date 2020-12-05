using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day05 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day05.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			int highestId = 0;

			char[,] map = new char[128,8];
			for (int i = 0; i < map.GetLength(0); i++)
				for (int j = 0; j < map.GetLength(1); j++)
					map[i, j] = 'O';

			foreach (string l in lines)
			{
				int rowMin = 0;
				int rowMax = 127;
				int columnMin = 0;
				int columnMax = 7;

				for (int i=0; i < l.Length;i++)
				{
					if (l[i] == 'F')
						rowMax = (rowMin+rowMax)/2;
					else if (l[i] == 'B')
						rowMin = (rowMin + rowMax) / 2;
					if (l[i] == 'R')
						columnMin = (columnMin+ columnMax) / 2;
					if (l[i] == 'L')
						columnMax = (columnMin + columnMax) / 2;
				}

				int row = (int)Math.Ceiling((rowMin + rowMax) / 2.0f);
				int col = (int)Math.Ceiling((columnMin + columnMax) / 2.0f);
				if (row * 8 + col > highestId)
					highestId = row * 8 + col;

				//Console.WriteLine($"Row: {row} Column: {col}, Seat ID = {row*8+col}");
				map[row, col] = '.';
			}


			int myRow = -1;
			int myColumn=-1;
			bool ignore = true;
			bool found = false;
			for (int i = 0; i < map.GetLength(0); i++)
			{
				for (int j = 0; j < map.GetLength(1); j++)
				{
					Console.Write(map[i, j]);
					if (ignore)
					{
						if (map[i, j] == '.')
							ignore = false;
					}
					else if (!found && !ignore)
					{
						if (map[i,j] == 'O')
						{
							myRow = i;
							myColumn = j;
							ignore = true;
							found = true;
						}
					}
				}
				Console.Write("\n");
			}

			Console.WriteLine($"Part 1: {highestId}");
			Console.WriteLine($"Part 2: {myRow*8+myColumn}");
		}
	}
}

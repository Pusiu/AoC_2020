﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Waypoint
	{
		public int x;
		public int y;
	}

	class Day12 : Day
	{
		public enum Directions { N = 0, S = 180, W = 270, E = 90 };
		public Directions direction = Directions.E;
		int x = 0;
		int y = 0;
		Waypoint wp = new Waypoint();

		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day12.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			foreach (string s in lines)
			{
				char command = s[0];
				int amount = int.Parse(s.Substring(1));
				if (command == 'L')
					amount = -amount;

				if (command == 'L' || command == 'R')
					Rotate(amount);
				else if (command == 'F')
					Move(direction, amount);
				else
				{
					Directions d;
					Enum.TryParse(command.ToString(), out d);
					Move(d, amount);
				}

			}
			Console.WriteLine($"Part 1: {Math.Abs(x) + Math.Abs(y)}");

			x = 0;
			y = 0;
			wp.x = 10;
			wp.y = 1;
			direction = Directions.E;
			//part 2
			foreach (string s in lines)
			{
				char command = s[0];
				int amount = int.Parse(s.Substring(1));
				if (command == 'L')
					amount = -amount;

				if (command == 'L' || command == 'R')
					RotateWp(amount);
				else if (command == 'F')
					Move(wp, amount);
				else
				{
					switch (command)
					{
						case 'N':
							wp.y += amount;
							break;
						case 'S':
							wp.y -= amount;
							break;
						case 'W':
							wp.x -= amount;
							break;
						case 'E':
							wp.x += amount;
							break;
					}
				}

			}

			Console.WriteLine($"Part 2: {Math.Abs(x) + Math.Abs(y)}");
		}

		public void Move(Directions d, int amount)
		{
			switch (d)
			{
				case Directions.N:
					y += amount;
					break;
				case Directions.S:
					y -= amount;
					break;
				case Directions.W:
					x -= amount;
					break;
				case Directions.E:
					x += amount;
					break;
			}
		}

		public void Move(Waypoint w, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				x += w.x;
				y += w.y;
			}
		}

		public void Rotate(int amount)
		{
			int a = Math.Abs(amount);
			int d = (int)direction;
			while (a > 0)
			{
				d += (amount > 0) ? 1 : -1;
				if (d < 0)
					d += 360;

				a -= 1;
			}
			direction = (Directions)((d) % 360);
		}

		public void RotateWp(int amount)
		{
			int newx = wp.x;
			int newy = wp.y;

			bool clockwise = (amount > 0);
			amount = Math.Abs(amount);
			int d = (int)direction;
			while (amount > 0)
			{
				d += (clockwise) ? 90 : -90;
				if (d < 0)
					d += 360;

				amount -= 90;
				direction = (Directions)((d) % 360);
				newx = (clockwise) ? wp.y : -wp.y;
				newy = (clockwise) ? -wp.x : wp.x;
				wp.x = newx;
				wp.y = newy;
			}
		}
	}
}

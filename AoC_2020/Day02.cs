using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day02 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day02.txt");
			List<string> passwords = new List<string>();
			while (!sr.EndOfStream)
				passwords.Add(sr.ReadLine());

			int validCount = 0;
			int validCount2 = 0;
			foreach (string password in passwords)
			{
				Match m = Regex.Match(password, @"(\d+)-(\d+).(.): (.+)");
				int min = int.Parse(m.Groups[1].Value);
				int max = int.Parse(m.Groups[2].Value);
				char letter = m.Groups[3].Value[0];
				string rest = m.Groups[4].Value;

				int occurances = 0;
				for (int i=0; i < rest.Length;i++)
				{
					if (rest[i] == letter)
						occurances++;
				}

				bool hasOne = false;
				if (rest[min - 1] == letter)
					hasOne = true;

				if (rest[max-1] == letter)
				{
					if (hasOne)
						hasOne = false;
					else
						hasOne = true;
				}

				if (hasOne)
					validCount2++;

				if (occurances >= min && occurances <= max)
					validCount++;
			}

			Console.WriteLine($"Part 1: {validCount}");
			Console.WriteLine($"Part 2: {validCount2}");
		}
	}
}

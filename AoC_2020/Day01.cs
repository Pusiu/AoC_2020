using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	public class Day01 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day01.txt");
			List<int> numbers = new List<int>();
			string[] n = sr.ReadToEnd().Split('\n');
			foreach (string k in n)
				numbers.Add(int.Parse(k));

			for (int i=0;i<numbers.Count;i++)
				for (int j=0;j<numbers.Count;j++)
				{
					if (numbers[i]+numbers[j]==2020)
					{
						Console.WriteLine($"Part 1: {numbers[i]*numbers[j]}");
					}

					for (int k=0; k < numbers.Count;k++)
					{
						if (numbers[i] + numbers[j] +numbers[k]== 2020)
						{
							Console.WriteLine($"Part 2: {numbers[i] * numbers[j]*numbers[k]}");
							return;
						}
					}
				}
		}
	}
}

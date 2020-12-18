using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day18 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day18.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			long sum1 = 0;
			foreach (string l in lines)
			{
				long v = Evaluate(l);
				//Console.WriteLine($"{l} = {v}");
				sum1 += v;
			}

			long sum2 = 0;
			foreach (string l in lines)
			{
				long v = Evaluate(l, true);
				//Console.WriteLine($"{l} = {v}");
				sum2 += v;
			}
			Console.WriteLine($"Part 1: {sum1}");
			Console.WriteLine($"Part 2: {sum2}");
		}

		long Evaluate(string expr, bool additionFirst=false)
		{
			long result = 0;

			Match m;
			do
			{
				m = Regex.Match(expr, @"(\([^(]+?\))");
				if (m.Success)
				{
					long e = Evaluate(m.Value.Substring(1, m.Length - 2), additionFirst);
					int ind = m.Index;
					StringBuilder sb = new StringBuilder();
					sb.Append(expr.Substring(0, ind));
					sb.Append(e);
					sb.Append(expr.Substring(ind + m.Length));
					expr = sb.ToString();
				}
			} while (m.Success);

			if (additionFirst)
			{
				do
				{
					m = Regex.Match(expr, @"(\d+ \+ \d+)");
					if (m.Success)
					{
						long e = Evaluate(m.Value, false);
						int ind = m.Index;
						StringBuilder sb = new StringBuilder();
						sb.Append(expr.Substring(0, ind));
						sb.Append(e);
						sb.Append(expr.Substring(ind + m.Length));
						expr = sb.ToString();
					}
				} while (m.Success);
			}

			string[] s = expr.Split(' ');
			result = long.Parse(s[0]);
			char op = '.';
			for (int i = 1; i < s.Length; i++)
			{
				if (s[i] == "+" || s[i] == "*")
					op = s[i][0];
				else
				{
					switch (op)
					{
						case '+':
							result += long.Parse(s[i]);
							break;
						case '*':
							result *= long.Parse(s[i]);
							break;
					}
				}
			}

			return result;
		}
	}
}

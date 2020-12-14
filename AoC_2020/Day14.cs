using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day14 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day14.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			string curMask = "";
			Dictionary<ulong, string> memory = new Dictionary<ulong, string>();

			foreach (string line in lines)
			{
				if (line.Contains("mask"))
					curMask = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries)[1];
				else
				{
					MatchCollection m = Regex.Matches(line, @"(\d+)");
					ulong address = ulong.Parse(m[0].Value);
					ulong value = ulong.Parse(m[1].Value);

					if (!memory.ContainsKey(address))
						memory.Add(address, String.Join("", Enumerable.Repeat("0", 36)));

					string v = ValueToBits(value);
					string res = ApplyMask(v, curMask);
					memory[address] = res;
					BitsToValue(res);
				}
			}

			ulong sum = 0;
			foreach (ulong k in memory.Keys)
			{
				sum += BitsToValue(memory[k]);
			}
			Console.WriteLine($"Part 1: {sum}");
			memory.Clear();

			foreach (string line in lines)
			{
				if (line.Contains("mask"))
					curMask = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries)[1];
				else
				{
					MatchCollection m = Regex.Matches(line, @"(\d+)");
					ulong address = ulong.Parse(m[0].Value);
					ulong value = ulong.Parse(m[1].Value);

					if (!memory.ContainsKey(address))
						memory.Add(address, String.Join("", Enumerable.Repeat("0", 36)));

					string v = ValueToBits(value);
					List<string> addresses = GetAddressess(ValueToBits(address), curMask);
					foreach (string ad in addresses)
					{
						memory[BitsToValue(ad)] = v;
					}
				}
			}

			sum = 0;
			foreach (ulong k in memory.Keys)
			{
				sum += BitsToValue(memory[k]);
			}
			Console.WriteLine($"Part 2: {sum}");
		}

		ulong BitsToValue(string bits)
		{
			ulong v = 0;
			ulong mul = 1;
			for (int i=0; i < bits.Length; i++)
			{
				if (bits[bits.Length - 1 - i] == '1')
					v += mul;

				mul *= 2;
			}

			return v;
		}

		string ValueToBits(ulong v)
		{
			return Convert.ToString((long)v, 2).PadLeft(36, '0');
		}

		string ApplyMask(string value, string mask)
		{
			StringBuilder result = new StringBuilder();
			for (int i=0; i < value.Length;i++)
			{
				if (mask[i] == 'X')
					result.Append(value[i]);
				else
					result.Append(mask[i]);
			}
			return result.ToString();
		}

		List<string> GetAddressess(string address, string mask, int index=0)
		{
			List<string> l = new List<string>();
			StringBuilder masked;
			if (index == 0)
			{
				masked = new StringBuilder();
				for (int i = 0; i < address.Length; i++)
				{
					if (mask[i] == 'X')
						masked.Append(mask[i]);
					else
						masked.Append(int.Parse(mask[i].ToString()) | int.Parse(address[i].ToString()));
				}
			}
			else
				masked = new StringBuilder(mask);

			for (int i=index; i < masked.Length;i++)
			{
				if (masked[i]=='X')
				{
					StringBuilder c = new StringBuilder(masked.ToString());
					c[i] = '0';
					//l.Add(c.ToString());
					l.AddRange(GetAddressess(address, c.ToString(), i));
					c[i] = '1';
					//l.Add(c.ToString());
					l.AddRange(GetAddressess(address, c.ToString(), i));
					return l;
				}
			}
			l.Add(masked.ToString());

			return l;
		}
	}
}

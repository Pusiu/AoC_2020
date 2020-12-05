using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Passport
	{
		public Dictionary<string, string> fields = new Dictionary<string, string>();
		public List<string> cand = new List<string>();

		public bool isValid = true;
		public bool isValidStructure = true;

		public Passport()
		{
			fields.Add("byr", "");
			fields.Add("iyr", "");
			fields.Add("eyr", "");
			fields.Add("hgt", "");
			fields.Add("hcl", "");
			fields.Add("ecl", "");
			fields.Add("pid", "");
			fields.Add("cid", "");
		}

		public void Verify()
		{
			foreach (string key in fields.Keys)
			{
				if (fields[key] == "" && key != "cid")
				{
					Console.WriteLine("Passport lacks fields: " + key);
					isValid = false;
					isValidStructure = false;
					return;
				}

				string value = fields[key];

				bool v = true;

				switch (key)
				{
					case "byr":
						if (int.Parse(value) >= 1920 && int.Parse(value) <= 2002)
							v = true;
						else
							v = false;

						Console.WriteLine("byr is " + v);
						break;
					case "iyr":
						if (int.Parse(value) >= 2010 && int.Parse(value) <= 2020)
							v = true;
						else
							v = false;

						Console.WriteLine("yir is " + v);
						break;
					case "eyr":
						if (int.Parse(value) >= 2020 && int.Parse(value) <= 2030)
							v = true;
						else
							v = false;
						Console.WriteLine("eyr is " + v);
						break;
					case "hgt":
						Match m = Regex.Match(value,@"(\d+)(\D+)*");
						if (m.Groups.Count > 2)
						{
							int n = int.Parse(m.Groups[1].Value);
							string unit = m.Groups[2].Value;
							if ((unit == "cm" && n >= 150 && n <= 193) || (unit == "in" && n >= 59 && n <= 76))
								v = true;
							else
								v = false;

						}
						else
						{
							v = false;
						}
						Console.WriteLine("hgt is " + v);
						break;
					case "hcl":
						v = Regex.IsMatch(value, @"#(\d|[a-f]){6}");
						Console.WriteLine("hcl is " + v);
						break;
					case "ecl":
						string[] cols = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
						v = cols.Contains(value);
						Console.WriteLine("ecl is " + v);
						break;
					case "pid":
						v = Regex.IsMatch(value, @"^\d{9}$");
						Console.WriteLine("pid is " + v);
						break;

				}

				if (isValid)
					isValid = v;
			}

			
		}

		public void Print()
		{
			Console.WriteLine("New password:");
			foreach (string s in cand)
				Console.WriteLine(s);

			Console.WriteLine("Results:");
			/*foreach (string f in fields.Keys)
			{
				Console.Write($"{f}:{fields[f]}\n");
			}*/
			Verify();
			Console.WriteLine($"Password is {(isValid ? "valid" : "invalid") }");
			Console.WriteLine("\n");
		}
	}

	class Day04 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day04.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
				lines.Add(sr.ReadLine());

			string[] fields = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" }; //cid optional

			int validPasswords1 = 0;
			int validPasswords2 = 0;

			Passport p = new Passport();

			for (int i=0; i < lines.Count;i++)
			{
				string l = lines[i];
				p.cand.Add(l);

				if (l.Length > 2)
				{
					string[] f2 = l.Split(' ');
					foreach (string s in f2)
					{
						string[] k = s.Split(':');
						if (p.fields[k[0]] == "")
						{
							p.fields[k[0]] = k[1];
						}
						else
						{
							p.isValid = false;
						}
					}
				}
				if (l.Length < 2 || i == lines.Count-1)
				{

					p.Print();

					if (p.isValidStructure)
						validPasswords1++;
					if (p.isValid)
						validPasswords2++;

					p = new Passport();

					continue;
				}
			}

			Console.WriteLine($"Part 1: {validPasswords1}");
			Console.WriteLine($"Part 2: {validPasswords2}");
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Rule
	{
		public int id;
		public string value = "";
		public List<Rule> subRules = new List<Rule>();
		public List<Rule> subRulesAlternative = new List<Rule>();

		public Rule(int id)
		{
			this.id = id;
		}

		public override string ToString()
		{
			return "Rule " + id;
		}
	}

	class Day19 : Day
	{
		Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day19.txt");
			List<string> lines = new List<string>();
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				lines.Add(line);
				if (line.Length > maxRuleCount)
					maxRuleCount = line.Length;
			}

			RebuildRules(lines);

			string regex = "^" + GenerateRegex(rules[0]) + "$";
			int entriesIndex = lines.IndexOf("");
			int good = 0;
			for (int i = entriesIndex+1;i < lines.Count;i++)
			{
				if (Regex.Match(lines[i], regex).Success)
					good++;
			}

			Console.WriteLine($"Part 1: {good}");

			lines[lines.FindIndex(x => x == "8: 42")] = "8: 42 | 42 8";
			lines[lines.FindIndex(x => x == "11: 42 31")] = "11: 42 31 | 42 11 31";

			RebuildRules(lines);

			regex = "^" + GenerateRegex(rules[0]) + "$";
			good = 0;
			for (int i = entriesIndex + 1; i < lines.Count; i++)
			{
				if (Regex.Match(lines[i], regex).Success)
					good++;
			}

			Console.WriteLine($"Part 2: {good}");

		}

		void RebuildRules(List<string> lines)
		{
			rules.Clear();
			foreach (string line in lines)
			{
				Match m = Regex.Match(line, @"(\d+):");
				if (m.Success)
				{
					int id = int.Parse(m.Groups[1].Value);
					Rule r = new Rule(id);
					r.value = line.Substring(m.Index + m.Length);
					rules.Add(id, r);
				}
			}

			foreach (int id in rules.Keys)
			{
				Match m = Regex.Match(rules[id].value, "\"(.)\"");
				if (m.Success)
				{
					rules[id].value = m.Groups[1].Value;
					continue;
				}
				string[] sp = rules[id].value.Split('|');
				rules[id].value = "";
				for (int j = 0; j < sp.Length; j++)
				{
					MatchCollection mc = Regex.Matches(sp[j], @"\d+");
					List<Rule> sub = (j == 0) ? rules[id].subRules : rules[id].subRulesAlternative;
					foreach (Match match in mc)
					{
						sub.Add(rules[int.Parse(match.Value)]);
					}
				}
			}
		}

		int maxRuleCount = 0;
		string GenerateRegex(Rule r, int rulesCount=0)
		{
			StringBuilder reg = new StringBuilder();
			reg.Append("(");
			if (r.value != "")
			{
				reg.Append(r.value);
				reg.Append(")");
				return reg.ToString();
			}

			foreach(Rule sr in r.subRules)
			{
				if (rulesCount < maxRuleCount)
				{
					rulesCount++;
					reg.Append(GenerateRegex(sr,rulesCount));
				}
			}
			if (r.subRulesAlternative.Count > 0)
			{
				reg.Append("|");
				foreach (Rule sr in r.subRulesAlternative)
				{
					if (rulesCount < maxRuleCount)
					{
						rulesCount++;
						reg.Append(GenerateRegex(sr, rulesCount));
					}
				}
			}

			reg.Append(")");
			return reg.ToString();
		}
	}
}


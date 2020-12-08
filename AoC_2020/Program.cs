using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Day> days = Assembly.GetExecutingAssembly().
				GetTypes().
				Where(x => x.IsClass && x.BaseType == typeof(Day) && !x.IsAbstract && x.Namespace == "AoC_2020").
				Select(x => (Day)Activator.CreateInstance(x)).ToList();

			Stopwatch t = new Stopwatch();
			t.Start();
			days[days.Count - 1].Solve();
			t.Stop();
			Console.WriteLine($"Program took {t.ElapsedMilliseconds} ms");
			Console.ReadLine();
		}
	}
}

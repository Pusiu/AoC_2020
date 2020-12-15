using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day15 : Day
	{
		public override void Solve()
		{
			//List<int> startingNumbers = new List<int>() { 0,3,6};
			List<int> startingNumbers = new List<int>() { 0, 13, 1, 8, 6, 15 };

			//Play(10, startingNumbers);
			Console.WriteLine($"Part 1: {Play(2020, startingNumbers)}");
			Console.WriteLine($"Part 2: {Play(30000000, startingNumbers)}");
		}

		int Play(int length, List<int> startingNumbers)
		{
			List<int> turns = new List<int>();
			Dictionary<int, int> numberTurnMap = new Dictionary<int, int>(); //key - number, value - last turn
			for (int i = 0; i < startingNumbers.Count; i++)
				numberTurnMap.Add(startingNumbers[i], i);

			int lastNumber = startingNumbers[startingNumbers.Count-1];
			for (int i = startingNumbers.Count; i < length; i++)
			{
				int n = 0;

				if (numberTurnMap.ContainsKey(lastNumber))
				{
					n = (i - 1) - numberTurnMap[lastNumber];
					numberTurnMap[lastNumber] = i - 1;
				}
				else
					numberTurnMap.Add(lastNumber, i - 1);

				lastNumber = n;

				/*int lastIndex = -1;
				int lastNumber = turns[i - 1];
				for (int j = i - 2; j >= 0; j--)
				{
					if (turns[j] == lastNumber)
					{
						lastIndex = i - j - 1;
						break;
					}
				}*/


			}

			return lastNumber;

					/*if (lastIndex == -1)
						turns.Add(0);
					else
						turns.Add(lastIndex);*/

				return turns[length - 1];
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day23 : Day
	{
		public bool log = false;
		public override void Solve()
		{
			//string input = "389125467";
			string input = "624397158";

			LinkedList<int> circle = new LinkedList<int>();

			for (int i=0; i < input.Length; i++)
			{
				circle.AddLast(int.Parse(input[i].ToString()));
			}
			Play(new LinkedList<int>(circle), 100);

			int max = circle.Max()+1;
			while (circle.Count < 1000000)
			{
				circle.AddLast(max++);
			}
			Play(new LinkedList<int>(circle), 10000000, true);
		}

		public void Play(LinkedList<int> circle, int moves, bool twoCups = false)
		{
			Dictionary<int, LinkedListNode<int>> map = new Dictionary<int, LinkedListNode<int>>();
			var currentCup = circle.First;

			while (currentCup != null)
			{
				map.Add(currentCup.Value, currentCup);
				currentCup = currentCup.Next;
			}
			currentCup = circle.First;
			for (int i = 0; i < moves; i++)
			{

				if (log)
				{
					Console.WriteLine("Move " + (i + 1));
					Console.Write("cups: ");
					foreach (int c in circle)
					{
						if (c == currentCup.Value)
							Console.Write($"({c}) ");
						else
							Console.Write($"{c} ");
					}
				}

				LinkedList<int> threeCups = new LinkedList<int>();

				var temp = currentCup;
				for (int j = 1; j <= 3; j++)
				{
					if (temp.Next != null)
					{
						threeCups.AddLast(temp.Next.Value);
						temp = temp.Next;
					}
					else
					{
						threeCups.AddLast(circle.First.Value);
						temp = circle.First;
					}
				}

				if (log)
					Console.WriteLine("\npick up: " + string.Join(",", threeCups));

				//circle.RemoveAll(x => threeCups.Contains(x));
				foreach (int c in threeCups)
					circle.Remove(map[c]);

				int destinationCup = currentCup.Value;
				do
				{
					destinationCup--;
					if (destinationCup <= 0)
						destinationCup = circle.Max();

				} while (threeCups.Contains(destinationCup) || destinationCup <= 0);


				if (log)
					Console.WriteLine("destination: " + destinationCup);


				LinkedListNode<int> destination = map[destinationCup];
				LinkedListNode<int> last = destination.Next;

				foreach (var c in threeCups)
				{
					LinkedListNode<int> n = new LinkedListNode<int>(c);
					circle.AddAfter(destination, n);
					destination = n;
					map[c] = n;
				}

				currentCup = (currentCup.Next == null) ? circle.First : currentCup.Next;
				/*int destinationIndex = circle.IndexOf(destinationCup);

				circle.InsertRange(destinationIndex + 1, threeCups);
				currentCupIndex = (circle.IndexOf(currentCup) + 1) % circle.Count;*/
			}

			if (log)
			{
				Console.WriteLine("Final");
				Console.Write("cups: ");
				foreach (int c in circle)
				{
					if (c == currentCup.Value)
						Console.Write($"({c}) ");
					else
						Console.Write($"{c} ");
				}
				Console.Write("\n");
			}

			//currentCupIndex = (circle.IndexOf(1) + 1) % circle.Count;
			if (twoCups)
			{
				currentCup = circle.Find(1);
				var firstCup = (currentCup.Next == null) ? circle.First : currentCup.Next;
				var secondCup = (firstCup.Next == null) ? circle.First : firstCup.Next;
				Console.WriteLine($"\nTwo cups after 1: {firstCup.Value} {secondCup.Value}");
				Console.WriteLine("Product: " + (long)((long)firstCup.Value * (long)secondCup.Value));
			}
			else
			{
				Console.Write("\nOrder after cup 1: ");
				currentCup = circle.Find(1);
				do
				{
					currentCup = (currentCup.Next == null) ? circle.First : currentCup.Next;
					if (currentCup.Value == 1)
						break;

					Console.Write(currentCup.Value);
					//currentCupIndex = (currentCupIndex + 1) % circle.Count;
				} while (true);
			}
			Console.Write("\n");
		}
	}
}

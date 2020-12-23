using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day22 : Day
	{
		public bool log = true;
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day22.txt");
			List<string> lines = new List<string>();

			Queue<int> player1Deck = new Queue<int>();
			Queue<int> player2Deck = new Queue<int>();
			Queue<int> currentDeck = player1Deck;

			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				lines.Add(line);
				if (line == "Player 2:")
				{
					currentDeck = player2Deck;
				}
				else
				{
					int c = 0;
					if (int.TryParse(line, out c))
					{
						currentDeck.Enqueue(c);
					}
				}
			}
			log = false;
			PlayCombat(new Queue<int>(player1Deck), new Queue<int>(player2Deck));
			PlayRecursiveCombat(new Queue<int>(player1Deck), new Queue<int>(player2Deck));

		}

		public bool DeckExistsInHistory(List<List<int>> history, List<int> deck)
		{
			foreach (List<int> roundDeck in history)
			{
				if (roundDeck.Count != deck.Count)
					continue;

				bool isEqual = true;
				for (int i=0; i < roundDeck.Count;i++)
				{
					if (roundDeck[i] != deck[i])
					{
						isEqual = false;
						break;
					}
				}
				if (isEqual)
					return true;
			}
			return false;
		}

		public bool PlayRecursiveCombat(Queue<int> player1Deck, Queue<int> player2Deck, int gameID=1)
		{
			if (gameID == 1)
			{
				Console.WriteLine("Playing the Recursive Combat game");
			}

			int round = 1;
			List<List<int>> player1DeckHistory = new List<List<int>>();
			List<List<int>> player2DeckHistory = new List<List<int>>();
			while (player1Deck.Count > 0 && player2Deck.Count > 0)
			{
				Log($"\nRound {round} of Game {gameID}");
				Log($"Player 1's deck: {string.Join(",", player1Deck.ToList())}");
				Log($"Player 2's deck: {string.Join(",", player2Deck.ToList())}");

				if (DeckExistsInHistory(player1DeckHistory, player1Deck.ToList()) || DeckExistsInHistory(player2DeckHistory, player2Deck.ToList()))
				{
					Log($"Game ends with player 1 winning by the infinite rule");
					return true;
				}
				player1DeckHistory.Add(player1Deck.ToList());
				player2DeckHistory.Add(player2Deck.ToList());

				int p1Card = player1Deck.Dequeue();
				int p2Card = player2Deck.Dequeue();
				Log($"Player 1 plays: {p1Card}\nPlayer 2 plays: {p2Card}");

				bool? subBattleResult = null;
				if (player1Deck.Count >= p1Card && player2Deck.Count >= p2Card)
				{
					Log($"Playing a sub-game to determine the winner...");
					subBattleResult = PlayRecursiveCombat(new Queue<int>(player1Deck.Take(p1Card)), new Queue<int>(player2Deck.Take(p2Card)), gameID+1);
					Log($"...back to game {gameID}");
				}

				if (
					(subBattleResult.HasValue && subBattleResult.Value == true) || 
					(!subBattleResult.HasValue && (p1Card > p2Card))
					)
				{
					player1Deck.Enqueue(p1Card);
					player1Deck.Enqueue(p2Card);
					Log($"Player 1 wins round {round} of game {gameID}");
				}
				else if (
					(subBattleResult.HasValue && subBattleResult.Value == false) ||
					(!subBattleResult.HasValue && p1Card < p2Card)
					)
				{
					player2Deck.Enqueue(p2Card);
					player2Deck.Enqueue(p1Card);
					Log($"Player 2 wins round {round} of game {gameID}");
				}
				Log("");
				round++;
			}

			Log($"The winner of game {gameID} is {((player1Deck.Count == 0) ? "Player 2" : "Player1")}");

			if (gameID == 1)
			{
				int winnerScore = 0;
				var winnerDeck = ((player1Deck.Count == 0) ? player2Deck : player1Deck).ToList();
				for (int i = 0; i < winnerDeck.Count; i++)
				{
					winnerScore += winnerDeck[i] * (winnerDeck.Count - i);
				}


				Console.WriteLine($"\nPost-game results");
				Console.WriteLine($"Player 1's deck: {string.Join(",", player1Deck.ToList())}");
				Console.WriteLine($"Player 2's deck: {string.Join(",", player2Deck.ToList())}");
				Console.WriteLine($"Winner score (part 2 answer): {winnerScore}\n");
			}

			return player2Deck.Count == 0; //returns true if player 1 won the game
		}

		public void PlayCombat(Queue<int> player1Deck, Queue<int> player2Deck)
		{
			int round = 1;
			while (player1Deck.Count > 0 && player2Deck.Count > 0)
			{
				Log($"Round {round++}");
				Log($"Player 1's deck: {string.Join(",", player1Deck.ToList())}");
				Log($"Player 2's deck: {string.Join(",", player2Deck.ToList())}");

				int p1Card = player1Deck.Dequeue();
				int p2Card = player2Deck.Dequeue();
				Log($"Player 1 plays: {p1Card}\nPlayer 2 plays: {p2Card}");

				if (p1Card > p2Card)
				{
					player1Deck.Enqueue(p1Card);
					player1Deck.Enqueue(p2Card);
					Log("Player 1 wins the round");
				}
				else
				{
					player2Deck.Enqueue(p2Card);
					player2Deck.Enqueue(p1Card);
					Log("Player 2 wins the round");
				}
				Log("");
			}

			int winnerScore = 0;
			var winnerDeck = ((player1Deck.Count == 0) ? player2Deck : player1Deck).ToList();
			for (int i = 0; i < winnerDeck.Count; i++)
			{
				winnerScore += winnerDeck[i] * (winnerDeck.Count - i);
			}


			Console.WriteLine($"\nPost-game results");
			Console.WriteLine($"Player 1's deck: {string.Join(",", player1Deck.ToList())}");
			Console.WriteLine($"Player 2's deck: {string.Join(",", player2Deck.ToList())}");
			Console.WriteLine($"Winner score: {winnerScore}\n");
		}

		public void Log(string text)
		{
			if (log)
				Console.WriteLine(text);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020
{
	class Day25 : Day
	{
		public override void Solve()
		{
			StreamReader sr = new StreamReader("Inputs/day25.txt");

			long cardPK = long.Parse(sr.ReadLine());
			long doorPK = long.Parse(sr.ReadLine());

			long loop1 = GetLoop(cardPK, 7);
			long loop2 = GetLoop(doorPK, 7);

			long cardEncryptionKey = Transform(doorPK, loop1);
			long doorEncryptionKey = Transform(cardPK, loop2);

			if (cardEncryptionKey != doorEncryptionKey)
				Console.WriteLine($"Encryption keys don't match! {cardEncryptionKey} {doorEncryptionKey} ");

			Console.WriteLine($"Encryption key = {doorEncryptionKey}");
		}

		long GetLoop(long pkey, long subject)
		{
			long val = 1;
			for (int i=1; ;i++)
			{
				val = val * subject % 20201227;
				if (pkey == val)
					return i;
			}
		}

		long Transform(long subject, long loop)
		{
			long current = 1;
			for (int i=1; i <= loop;i++)
			{
				current = current * subject % 20201227;
			}
			return current;
		}
	}
}



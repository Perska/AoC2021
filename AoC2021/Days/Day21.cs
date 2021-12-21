using System;
using System.Collections.Generic;

namespace AoC2021
{
	partial class Program
	{
		static void Day21(List<string> input)
		{
			int _p1 = int.Parse(input[0].SplitToStringArray(": ", false)[1]);
			int _p2 = int.Parse(input[1].SplitToStringArray(": ", false)[1]);
			int p1 = _p1;
			int p2 = _p2;
			int score1 = 0;
			int score2 = 0;
			int dice = 1;
			int tosses = 0;
			while (true)
			{
				Turn(ref p1, ref score1);
				if (score1 >= 1000) break;
				Turn(ref p2, ref score2);
				if (score2 >= 1000) break;
			}
			Console.WriteLine($"{(score1 >= 1000 ? score2 : score1) * tosses}");

			(int A, int B)[] repeat = new (int, int)[] { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) };
			p1 = _p1;
			p2 = _p2;
			score1 = 0;
			score2 = 0;
			dice = 1;
			tosses = 0;
			long p1w = 0;
			long p2w = 0;
			Dictionary<(int P1, int P2, int S1, int S2, bool Turn, int Toss), (long, long)> winCache = new Dictionary<(int P1, int P2, int S1, int S2, bool Turn, int Toss), (long, long)>();
			
			for (int i = 0; i < repeat.Length; i++)
			{
				(long a, long b) = QTurn(p1, 0, p2, 0, false, repeat[i].A);
				p1w += a * repeat[i].B;
				p2w += b * repeat[i].B;
			}
			Console.WriteLine($"{(p1w > p2w ? p1w : p2w)}");
			
			void Turn(ref int pos, ref int score)
			{
				int toss = ThrowDice() + ThrowDice() + ThrowDice();
				pos += toss;
				while (pos > 10)
				{
					pos -= 10;
				}
				score += pos;
			}

			(long, long) QTurn(int pos1, int sco1, int pos2, int sco2, bool player, int toss)
			{
				(long P1, long P2) winner;
				if (winCache.TryGetValue((pos1, pos2, sco1, sco2, player, toss), out winner))
				{
					return winner;
				}
				int pos, score;
				if (!player)
				{
					pos = pos1;
					score = sco1;
				}
				else
				{
					pos = pos2;
					score = sco2;
				}
				pos += toss;
				while (pos > 10)
				{
					pos -= 10;
				}
				score += pos;
				if (score >= 21)
				{
					winCache[(pos1, pos2, sco1, sco2, player, toss)] = player ? (0, 1) : (1, 0);
					return player ? (0, 1) : (1, 0);
				}
				else
				{
					(int pr1, int pr2, int sc1, int sc2) = (pos1, pos2, sco1, sco2);
					if (!player)
					{
						pos1 = pos;
						sco1 = score;
					}
					else
					{
						pos2 = pos;
						sco2 = score;
					}
					long win1 = 0, win2 = 0;
					for (int i = 0; i < repeat.Length; i++)
					{
						(long a, long b) = QTurn(pos1, sco1, pos2, sco2, !player, repeat[i].A);
						win1 += a * repeat[i].B;
						win2 += b * repeat[i].B;
					}
					winCache[(pr1, pr2, sc1, sc2, player, toss)] = (win1, win2);
					return (win1, win2);

				}
			}

			int ThrowDice()
			{
				int result = dice;
				dice++;
				if (dice == 101) dice -= 100;
				tosses++;
				return result;
			}
		}
	}
}

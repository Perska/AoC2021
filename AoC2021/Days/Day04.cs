using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
	partial class Program
	{
		[UseSRL]
		static void Day04(List<string> input)
		{
			List<int> bingo = new List<int>();

			foreach (string num in input[0].Split(','))
            {
				bingo.Add(int.Parse(num));
            }

			List<(int,bool)[,]> boards = new List<(int,bool)[,]>();

			(int, bool)[,] currentBoard = null;
			int y = 0;
            for (int i = 1; i < input.Count - 1; i++)
            {
				if (input[i] == "")
                {
					currentBoard = new (int, bool)[5, 5];
                    boards.Add(currentBoard);
                    y = 0;
                }
                else
                {
					var row = input[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					for (int x = 0; x < 5; x++)
                    {
						currentBoard[x, y] = (int.Parse(row[x]), false);
                    }
					y++;
                }
            }

			int finalNum = 0;

            int winnerIndex = 0;
            for (int i = 0; i < bingo.Count; i++)
            {
				BingoMark(bingo[i]);
                //PrintBoard(boards[2]);
                if (BingoWin(out currentBoard))
                {
                    finalNum = bingo[i];
                    winnerIndex = i;
                    break;
                }
            }

            int sum = 0;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!currentBoard[i, j].Item2)
                    {
                        sum += currentBoard[i, j].Item1;
                    }
                }
            }

            //PrintBoard(currentBoard);

            Console.WriteLine($"{sum} * {finalNum} = {sum * finalNum}");

            finalNum = 0;

            for (int i = winnerIndex + 1; i < bingo.Count; i++)
            {
                BingoMark(bingo[i]);
                //PrintBoard(boards[2]);
                if (BingoWinAll(out var winners))
                {
                    boards.RemoveAll(board => winners.Contains(board));
                    currentBoard = winners.Last();
                    finalNum = bingo[i];
                }
            }

            sum = 0;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!currentBoard[i, j].Item2)
                    {
                        sum += currentBoard[i, j].Item1;
                    }
                }
            }

            //PrintBoard(currentBoard);

            Console.WriteLine($"{sum} * {finalNum} = {sum * finalNum}");

            void PrintBoard((int,bool)[,] board)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write($"{board[i, j].Item1:D2}{(board[i, j].Item2 ? "#" : " ")}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

			void BingoMark(int num)
            {
                foreach (var board in boards)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (board[i, j].Item1 == num)
                            {
                                board[i, j] = (num, true);
                            }
                        }
                    }
                }
            }

            bool BingoWinAll(out List<(int,bool)[,]> winners)
            {
                winners = new List<(int, bool)[,]>();
                foreach (var board in boards)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int consecutive = 0;
                        for (int j = 0; j < 5; j++)
                        {
                            if (board[i, j].Item2) consecutive++;
                            if (consecutive == 5)
                            {
                                winners.Add(board);
                                goto nextBoard;
                            }
                        }
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        int consecutive = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (board[i, j].Item2) consecutive++;
                            if (consecutive == 5)
                            {
                                winners.Add(board);
                                goto nextBoard;
                            }
                        }
                    }
                nextBoard:;
                }
                if (winners.Count > 0)
                {
                    return true;
                }
                winners = null;
                return false;
            }

            bool BingoWin(out (int, bool)[,] winner)
            {
                foreach (var board in boards)
                {
                    winner = board;
                    for (int i = 0; i < 5; i++)
                    {
                        int consecutive = 0;
                        for (int j = 0; j < 5; j++)
                        {
                            if (board[i, j].Item2) consecutive++;
                            if (consecutive == 5) return true;
                        }
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        int consecutive = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (board[i, j].Item2) consecutive++;
                            if (consecutive == 5) return true;
                        }
                    }
                }

                winner = null;
                return false;
            }

        }
    }
}

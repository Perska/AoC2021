using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	partial class Program
	{
		class TreeNode
		{
			public object A;
			public object B;
			public TreeNode Parent;

			public TreeNode()
			{

			}

			public TreeNode(object a, object b)
			{
				A = a;
				B = b;
				if (a is TreeNode) (a as TreeNode).Parent = this;
				if (b is TreeNode) (b as TreeNode).Parent = this;
			}

			public void Swap()
			{
				(A, B) = (B, A);
			}

			public void Replace(int value)
			{
				if (Parent != null)
				{
					if (Parent.A == this)
					{
						Parent.A = value;
					}
					else if (Parent.B == this)
					{
						Parent.B = value;
					}
				}
			}

			public override string ToString()
			{
				return $"[{A},{B}]";
			}
		}
		
		[NoTrailingNewLine]
		static void Day18(List<string> input)
		{
			List<TreeNode> pairs = new List<TreeNode>();

			for (int i = 0; i < input.Count; i++)
			{
				int x = 0;
				pairs.Add(GetItem(input[i], ref x));
				x = 0;
			}

			TreeNode main = pairs[0];
			if (input.Count == 1)
			{
				Reduce(main);
			}
			else
			{
				for (int i = 1; i < pairs.Count; i++)
				{
					main = AddFish(main, pairs[i]);
					Reduce(main);
				}
			}
			Console.WriteLine(Magnitude(main));

			void Reduce(TreeNode calc)
			{
				while (true)
				{
					var splode = FindExplode(calc, 0);
					if (splode != null)
					{
						Explode(splode);
					}
					else
					{
						var (split, direction) = FindTen(calc);
						if (split != null)
						{
							Split(split, direction);
						}
						else
						{
							break;
						}
					}
				}
			}

			int highest = 0;
			for (int i = 0; i < input.Count; i++)
			{
				for (int j = 0; j < input.Count; j++)
				{
					if (i == j) continue;
					int pos = 0;
					TreeNode x = GetItem(input[i], ref pos);
					pos = 0;
					TreeNode y = GetItem(input[j], ref pos);
					TreeNode z = AddFish(x, y);
					Reduce(z);
					highest = Math.Max(highest, Magnitude(z));
				}
			}
			Console.WriteLine(highest);

			void Split(TreeNode item, bool direction)
			{
				if (direction)
				{
					int val = (int)item.B;
					item.B = new TreeNode((int)Math.Floor(val / 2f), (int)Math.Ceiling(val / 2f)) { Parent = item };
				}
				else
				{
					int val = (int)item.A;
					item.A = new TreeNode((int)Math.Floor(val / 2f), (int)Math.Ceiling(val / 2f)) { Parent = item };
				}
			}

			void Explode(TreeNode item)
			{
				var leftNeighbour = FindNeighbour(item, false);
				var rightNeighbour = FindNeighbour(item, true);
				if (leftNeighbour.target != null)
				{
					int left = (int)item.A;
					int leftAdd = (int)(leftNeighbour.direction ? leftNeighbour.target.B : leftNeighbour.target.A);
					if (leftNeighbour.direction)
					{
						leftNeighbour.target.B = leftAdd + left;
					}
					else
					{
						leftNeighbour.target.A = leftAdd + left;
					}
				}
				if (rightNeighbour.target != null)
				{
					int right = (int)item.B;
					int rightAdd = (int)(rightNeighbour.direction ? rightNeighbour.target.B : rightNeighbour.target.A);
					if (rightNeighbour.direction)
					{
						rightNeighbour.target.B = rightAdd + right;
					}
					else
					{
						rightNeighbour.target.A = rightAdd + right;
					}
				}
				item.Replace(0);
			}

			(TreeNode target, bool direction) FindNeighbour(TreeNode start, bool goRight)
			{
				TreeNode current = start;
				TreeNode previous = null;
				bool goingUp = false;
				while (true)
				{
					object climb;
					if (!goingUp)
					{
						(previous, current) = (current, current.Parent);
						if (current == null) return (null, false);
						climb = goRight ? current.B : current.A;
					}
					else
					{
						previous = current;
						climb = goRight ? current.A : current.B;
					}
					if (climb is int)
					{
						return (current, goingUp ^ goRight);
					}
					else if (climb != previous)
					{
						goingUp = true;
						current = climb as TreeNode;
					}
					else if (goingUp && climb == previous)
					{
						return (null, false);
					}
				}
			}

			int Magnitude(TreeNode item)
			{
				int wanted = 0;
				if (item.A is int)
				{
					wanted += 3 * (int)item.A;
				}
				else if (item.A is TreeNode)
				{
					wanted += 3 * Magnitude(item.A as TreeNode);
				}
				if (item.B is int)
				{
					wanted += 2 * (int)item.B;
				}
				else if (item.B is TreeNode)
				{
					wanted += 2 * Magnitude(item.B as TreeNode);
				}
				return wanted;
			}

			TreeNode FindExplode(TreeNode item, int depth)
			{
				if (item.A is int && item.B is int)
				{
					return depth >= 4 ? item : null;
				}
				else
				{
					TreeNode wanted = null;
					if (item.A is TreeNode)
					{
						wanted = FindExplode(item.A as TreeNode, depth + 1);
					}
					if (wanted == null && item.B is TreeNode)
					{
						wanted = FindExplode(item.B as TreeNode, depth + 1);
					}
					return wanted;
				}
			}
			
			(TreeNode target, bool direction) FindTen(TreeNode item)
			{
				(TreeNode target, bool direction) wanted = (null, false);
				if (item.A is int && (int)item.A >= 10)
				{
					wanted = (item, false);
				}
				if (wanted.target == null && item.A is TreeNode)
				{
					wanted = FindTen(item.A as TreeNode);
				}
				if (wanted.target == null && item.B is int && (int)item.B >= 10)
				{
					wanted = (item, true);
				}
				if (wanted.target == null && item.B is TreeNode)
				{
					wanted = FindTen(item.B as TreeNode);
				}
				return wanted;
			}

			TreeNode AddFish(TreeNode a, TreeNode b)
			{
				TreeNode c = new TreeNode(a, b);
				return c;
			}

			TreeNode GetItem(string str, ref int pos)
			{
				TreeNode item = new TreeNode();
				pos++;
				bool parsingNum = false;
				string parsedNum = "";
				while (true)
				{
					char chr = str[pos];
					if (parsingNum)
					{
						if ('0' <= chr && chr <= '9')
						{
							parsingNum = true;
							parsedNum += chr;
						}
						else
						{
							item.B = int.Parse(parsedNum);
							parsingNum = false;
							parsedNum = string.Empty;
						}
					}
					if (chr == '[')
					{
						item.B = GetItem(str, ref pos);
						TreeNode child;
						if ((child = item.B as TreeNode) != null)
						{
							child.Parent = item;
						}
					}
					else if (chr == ']')
					{
						return item;
					}
					else if (chr == ',')
					{
						item.Swap();
					}
					else if (chr == '-' || ('0' <= chr && chr <= '9') && !parsingNum)
					{
						parsingNum = true;
						parsedNum += chr;
					}
					pos++;
				}
			}
		}
	}
}

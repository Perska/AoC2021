using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
	/// <summary>
	/// This class contains code that usually are helpful for multiple days, or aren't written by me.
	/// If any are by someone else, they are marked as such.
	/// </summary>
	static class HelpF
	{
		// Split a string into separate strings, as specified by the delimiter.
		public static string[] SplitToStringArray(this string str, char split, bool removeEmpty)
		{
			return str.Split(new char[] { split }, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
		}

		// Split a string into an int array.
		public static int[] SplitToIntArray(this string str, char split)
		{
			return Array.ConvertAll(str.SplitToStringArray(split, true), s => int.Parse(s));
		}

		// Split a string into a long array.
		public static long[] SplitToLongArray(this string str, char split)
		{
			return Array.ConvertAll(str.SplitToStringArray(split, true), s => long.Parse(s));
		}
	}
}
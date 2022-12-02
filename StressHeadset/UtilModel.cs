using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StressHeadset
{
	public static class UtilModel
	{
		// 단일키
		public static void UpdateFileSkipRow(string fileFullPath, string key)
		{
			string[] lines = File.ReadAllLines(fileFullPath);
			int pos = Array.FindIndex(lines, row => row.Contains(key));
			if (pos < 0) return; // 일치하는게 없다면 return
			UtilModel.RemoveAt<string>(ref lines, pos);
			File.WriteAllLines(fileFullPath, lines);
		}

		// 여러키
		public static void UpdateFileSkipRow(string fileFullPath, List<string> keys)
		{
			if (keys.Count() == 0) return;
			var lines = File.ReadAllLines(fileFullPath);
			foreach (var key in keys)
			{
				var pos = Array.FindIndex(lines, row => row.Contains(key));
				if (pos >= 0) UtilModel.RemoveAt<string>(ref lines, pos);
			}
			File.WriteAllLines(fileFullPath, lines);
		}

		public static void RemoveAt<T>(ref T[] arr, int index)
		{
			for (int a = index; a < arr.Length - 1; a++) arr[a] = arr[a + 1];
			Array.Resize(ref arr, arr.Length - 1);
		}
	}
}


using UnityEngine;
using System.IO;

namespace TheDarkPath
{
	public class IMFile
	{

		public static string ReadLine(FileStream f)
		{
			char auxChar = '\0';
			string auxString;
			while (auxChar <= 32)
			{
				auxChar = (char)f.ReadByte();
			}

			auxString = auxChar.ToString();
			auxChar = (char)f.ReadByte();
			while (auxChar > 31)
			{
				auxString += auxChar.ToString();
				auxChar = (char)f.ReadByte();
			}
			return auxString;
		}

		public static string Filter(string stringSource)
		{
			int positionLastSlash = 0;
			int positionDot = 0;
			for (int i = 0; i < stringSource.Length; i++)
			{
				if (stringSource[i] == '/')
					positionLastSlash = i + 1;
				else if (stringSource[i] == '.')
					positionDot = i;
			}
			string result = new string(stringSource[positionLastSlash], 1);
			for (int i = positionLastSlash + 1; i < positionDot; i++)
			{
				result += stringSource[i];
			}
			return result;
		}

	}
}
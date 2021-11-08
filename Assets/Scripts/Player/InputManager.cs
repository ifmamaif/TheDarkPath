using UnityEngine; // GameObject and Input

namespace TheDarkPath
{
	public class InputManager
	{
		private short move = 0;

		public InputManager()
		{
		}

		//Input.GetKeyDown este adevarat doar cand s-a apasat butonul , nu este adevarat daca butonul este in continuare apasat !!!
		//Input.GetKeyUp este adevarat cand butonul respectiv s-a ridicat / i s-a dat drumul , cu conditia ca un buton sa fie apasat in acel timp

		private void CheckKey(KeyCode keyCode, short value)
		{
			if (Input.GetKeyDown(keyCode))
			{
				move = value;
			}

			short checkValue = value;
			short assignValue = 0;

			if (Input.GetKey(keyCode))
			{
				var temp = checkValue;
				checkValue = assignValue;
				assignValue = temp;
			}
			move = (move == checkValue ? assignValue : move);
		}

		public Vector2Int SetMove()
		{

			CheckKey(KeyCode.RightArrow, 6);
			CheckKey(KeyCode.LeftArrow, 4);
			CheckKey(KeyCode.UpArrow, 8);
			CheckKey(KeyCode.DownArrow, 2);

			return move switch
			{
				2 => new Vector2Int(0, -1),
				4 => new Vector2Int(-1, 0),
				6 => new Vector2Int(1, 0),
				8 => new Vector2Int(0, 1),
				_ => new Vector2Int(0, 0),
			};
		}

		public Vector2Int Control()
		{
			return SetMove();
		}

	}
}
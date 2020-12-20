using System;
using System.Linq;
using Microsoft.VisualBasic;

namespace Advent2020
{
	public class StringMatrix
	{
		string[] _image;
		int _lineCount;

		public StringMatrix(int size)
		{
			_image = new string[size];
		}
			
		public string this[int row] => _image[row];

		public void AddLine(string line)
		{
			_image[_lineCount++] = line;
		}

		public void FlipHorizontal()
		{
			for (int i = 0; i < _image.Length; i++)
			{
				_image[i] = Strings.StrReverse(_image[i]);
			}
		}

		public void FlipVertical()
		{
			Array.Reverse(_image);
		}

		public void RotateLeft()
		{
			var newImage = new string[_image.Length];
			for (int y = 0; y < _image.Length; y++)
			{
				for (int x = 0; x < _image.Length; x++)
				{
					newImage[_image[0].Length - x - 1] += _image[y][x];
				}
			}

			_image = newImage;
		}

		public void RotateRight()
		{
			RotateLeft();
			RotateLeft();
			RotateLeft();
		}

		public int CountCharacter(char findCharacter)
		{
			return _image.Sum(line => line.Sum(c => c == findCharacter ? 1 : 0));
		}
	}
}

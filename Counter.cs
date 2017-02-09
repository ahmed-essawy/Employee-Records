using System;
using System.Windows.Forms;

namespace XMLProject
{
	public partial class Counter : Label
	{
		int num1, num2;
		Char delimiter;

		public int Num1
		{
			get => num1;
			set
			{
				num1 = value;
				this.Text = num1.ToString() + delimiter.ToString() + num2.ToString();
			}
		}
		public int Num2
		{
			get => num2;
			set
			{
				num2 = value;
				this.Text = num1.ToString() + delimiter.ToString() + num2.ToString();
			}
		}
		public char Delimiter
		{
			get => delimiter;
			set
			{
				delimiter = value;
				this.Text = num1.ToString() + delimiter.ToString() + num2.ToString();
			}
		}
		public Counter()
		{
			num1 = num2 = 0;
			delimiter = ':';
		}
	}
}

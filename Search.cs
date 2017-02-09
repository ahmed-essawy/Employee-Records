using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XMLProject
{
	public partial class Search : Form
	{
		int choosen;
		Dictionary<string, int> temp_emp;
		public int Choosen { get => choosen; }
		public Search(Dictionary<string, int> duplicates)
		{
			InitializeComponent();
			temp_emp = duplicates;
			foreach (string name in duplicates.Keys)
			{
				listBox1.Items.Add(name);
			}
			listBox1.SelectedIndex = 0;
		}
		private void Choose(object sender, EventArgs e)
		{
			choosen = temp_emp[listBox1.SelectedItem.ToString()];
			DialogResult = DialogResult.OK;
			Close();
		}
		private void Choose(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				choosen = temp_emp[listBox1.SelectedItem.ToString()];
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}
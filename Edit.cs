using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XMLProject
{
	public partial class Edit : Form
	{
		Employee edited_emp;
		int max_phone = 5, max_address = 3;
		public Edit( Employee emp )
		{
			InitializeComponent();
			textBox1.Text = emp["name"];
			textBox2.Text = emp["mail"];
			int count1 = emp.Phone.Count > max_phone ? max_phone : emp.Phone.Count;
			text_phones = new TextBox[count1];
			comb_phones = new ComboBox[count1];
			this.Phones.Height = ( 24 + 6 ) * count1 + 3;
			for(int i = 0, y = 3; i < count1; i++, y += 24 + 6)
			{
				text_phones[i] = new TextBox();
				text_phones[i].Location = new System.Drawing.Point(154, y + 2);
				text_phones[i].Name = "ext_P_textBox" + i;
				text_phones[i].Size = new System.Drawing.Size(214, 20);
				text_phones[i].TabIndex = 3;
				text_phones[i].Text = emp.Phone[i].Number;
				comb_phones[i] = new ComboBox();
				comb_phones[i].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
				comb_phones[i].Font = new System.Drawing.Font("Tahoma", 10F);
				comb_phones[i].FormattingEnabled = true;
				comb_phones[i].ItemHeight = 16;
				comb_phones[i].Location = new System.Drawing.Point(68, y);
				comb_phones[i].Name = "ext_comboBox" + i;
				comb_phones[i].Size = new System.Drawing.Size(80, 24);
				comb_phones[i].TabIndex = 3;
				foreach(Phone_Type item in Phone_Type.GetValues(typeof(Phone_Type)))
				{
					comb_phones[i].Items.Add(item);
				}
				comb_phones[i].Text = emp.Phone[i].Type.ToString();
				this.Phones.Controls.Add(text_phones[i]);
				this.Phones.Controls.Add(comb_phones[i]);
			}
			int count2 = emp.Address.Count > max_address ? max_address : emp.Address.Count;
			text_addresses = new TextBox[count2 * 5];
			this.Addresses.Location = new System.Drawing.Point(6, this.Phones.Location.Y + this.Phones.Height + 6);
			this.Addresses.Height = ( 20 + 6 ) * 3 * count2 + 7;
			for(int i = 0, j = 0, y = 0; i < count2; i++, j += 5, y += 82)
			{
				text_addresses[j + 0] = new TextBox();
				text_addresses[j + 0].Location = new System.Drawing.Point(68, y + 3);
				text_addresses[j + 0].Name = "ext_A_textBox" + i + "_" + j + 0;
				text_addresses[j + 0].Size = new System.Drawing.Size(300, 20);
				text_addresses[j + 0].TabIndex = 3;
				text_addresses[j + 0].Text = emp.Address[i].Street;
				this.Addresses.Controls.Add(text_addresses[j + 0]);

				text_addresses[j + 1] = new TextBox();
				text_addresses[j + 1].Location = new System.Drawing.Point(68, y + 29);
				text_addresses[j + 1].Name = "ext_A_textBox" + i + "_" + j + 1;
				text_addresses[j + 1].Size = new System.Drawing.Size(147, 20);
				text_addresses[j + 1].TabIndex = 3;
				text_addresses[j + 1].Text = emp.Address[i].Building;
				this.Addresses.Controls.Add(text_addresses[j + 1]);

				text_addresses[j + 2] = new TextBox();
				text_addresses[j + 2].Location = new System.Drawing.Point(221, y + 29);
				text_addresses[j + 2].Name = "ext_A_textBox" + i + "_" + j + 2;
				text_addresses[j + 2].Size = new System.Drawing.Size(147, 20);
				text_addresses[j + 2].TabIndex = 3;
				text_addresses[j + 2].Text = emp.Address[i].Region;
				this.Addresses.Controls.Add(text_addresses[j + 2]);

				text_addresses[j + 3] = new TextBox();
				text_addresses[j + 3].Location = new System.Drawing.Point(68, y + 55);
				text_addresses[j + 3].Name = "ext_A_textBox" + i + "_" + j + 3;
				text_addresses[j + 3].Size = new System.Drawing.Size(147, 20);
				text_addresses[j + 3].TabIndex = 3;
				text_addresses[j + 3].Text = emp.Address[i].City;
				this.Addresses.Controls.Add(text_addresses[j + 3]);

				text_addresses[j + 4] = new TextBox();
				text_addresses[j + 4].Location = new System.Drawing.Point(221, y + 55);
				text_addresses[j + 4].Name = "ext_A_textBox" + i + "_" + j + 4;
				text_addresses[j + 4].Size = new System.Drawing.Size(147, 20);
				text_addresses[j + 4].TabIndex = 3;
				text_addresses[j + 4].Text = emp.Address[i].Country;
				this.Addresses.Controls.Add(text_addresses[j + 4]);
			}
			this.groupBox1.Height += ( count1 - 1 ) * 30 + ( count2 - 1 ) * 82;
			this.Height += ( count1 - 1 ) * 30 + ( count2 - 1 ) * 82;
			this.button1.Location = new System.Drawing.Point(12, this.groupBox1.Height + 16);
			this.button2.Location = new System.Drawing.Point(210, this.groupBox1.Height + 16);
		}
		private void Update( object sender, EventArgs e )
		{
			edited_emp = new Employee();
			edited_emp.Name = textBox1.Text;
			edited_emp.Mail = textBox2.Text;
			edited_emp.Phone = new List<Phones>();
			for(int i = 0; i < ( Phones.Controls.Count - 1 ) / 2; i++)
			{
				Phones temp_phone = new Phones();
				TextBox temp_textbox = (TextBox)Phones.Controls.Find("ext_P_textBox" + i, true)[0];
				ComboBox temp_combpbox = (ComboBox)Phones.Controls.Find("ext_comboBox" + i, true)[0];
				temp_phone.Number = temp_textbox.Text;
				temp_phone.Type = (Phone_Type)temp_combpbox.SelectedItem;
				if(temp_phone.Number != "")
					edited_emp.Phone.Add(temp_phone);
			}
			edited_emp.Address = new List<Addresses>();
			for(int i = 0, j = 0; i < ( Addresses.Controls.Count - 1 ) / 5; i++, j += 5)
			{
				Addresses temp_address = new Addresses();
				TextBox temp_a1 = (TextBox)this.Controls.Find("ext_A_textBox" + i + "_" + j + 0, true)[0];
				TextBox temp_a2 = (TextBox)this.Controls.Find("ext_A_textBox" + i + "_" + j + 1, true)[0];
				TextBox temp_a3 = (TextBox)this.Controls.Find("ext_A_textBox" + i + "_" + j + 2, true)[0];
				TextBox temp_a4 = (TextBox)this.Controls.Find("ext_A_textBox" + i + "_" + j + 3, true)[0];
				TextBox temp_a5 = (TextBox)this.Controls.Find("ext_A_textBox" + i + "_" + j + 4, true)[0];
				temp_address.Street = temp_a1.Text;
				temp_address.Building = temp_a2.Text;
				temp_address.Region = temp_a3.Text;
				temp_address.City = temp_a4.Text;
				temp_address.Country = temp_a5.Text;
				if(!( temp_address.Street == ""
					&& temp_address.Building == ""
					&& temp_address.Region == ""
					&& temp_address.City == ""
					&& temp_address.Country == "" ))
					edited_emp.Address.Add(temp_address);
			}
			DialogResult = DialogResult.OK;
			Exit(sender, e);
		}
		private void Exit( object sender, EventArgs e )
		{
			Close();
		}
		public Employee Edited_Emp { get => edited_emp; }
	}
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace XMLProject
{
	public partial class Insert : Form
	{
		List<Employee> inserted_emp = new List<Employee>();
		public List<Employee> New_Emp { get => inserted_emp; set => inserted_emp = value; }
		public Insert()
		{
			InitializeComponent();
			int ret_val = 0;
			foreach (Phone_Type item in Phone_Type.GetValues(typeof(Phone_Type)))
			{
				ret_val = comboBox1.Items.Add(item);
				comboBox2.Items.Add(item);
			}
			comboBox1.SelectedIndex = comboBox2.SelectedIndex = ret_val;
		}
		private void Add2File(Employee emp)
		{
			saveFileDialog1.ShowDialog();
			string path = saveFileDialog1.FileName;
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(path);
			}
			catch (Exception)
			{
				XmlWriterSettings config = new XmlWriterSettings
				{
					Indent = true,
					NewLineOnAttributes = true
				};
				using (XmlWriter xmlWriter = XmlWriter.Create(path, config))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("employees");
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
					xmlWriter.Flush();
					xmlWriter.Close();
				}
			}
			finally
			{
				doc.Load(path);
			}
			XmlNode root = doc.SelectSingleNode("employees").AppendChild(doc.CreateElement("employee"));
			XmlElement name = doc.CreateElement("name");
			name.InnerText = emp.Name;
			root.AppendChild(name);
			XmlElement phones = doc.CreateElement("phones");
			foreach (Phones item in emp.Phone)
			{
				XmlElement phone = doc.CreateElement("phone");
				phone.InnerText = item.Number;
				phone.SetAttribute("type", item.Type.ToString());
				phones.AppendChild(phone);
			}
			root.AppendChild(phones);
			XmlElement addresses = doc.CreateElement("addresses");
			foreach (Addresses item in emp.Address)
			{
				XmlElement address = doc.CreateElement("address");
				XmlElement street = doc.CreateElement("street");
				street.InnerText = item.Street;
				address.AppendChild(street);
				XmlElement building = doc.CreateElement("building");
				building.InnerText = item.Building;
				address.AppendChild(building);
				XmlElement region = doc.CreateElement("region");
				region.InnerText = item.Region;
				address.AppendChild(region);
				XmlElement city = doc.CreateElement("city");
				city.InnerText = item.City;
				address.AppendChild(city);
				XmlElement country = doc.CreateElement("country");
				country.InnerText = item.Country;
				address.AppendChild(country);
				addresses.AppendChild(address);
			}
			root.AppendChild(addresses);
			XmlElement mail = doc.CreateElement("mail");
			mail.InnerText = emp.Mail;
			root.AppendChild(mail);
			//Check_Validation(doc);
			doc.Save(path);
		}
		private void Clear(object sender, EventArgs e)
		{
			foreach (Control obj in this.groupBox1.Controls)
			{
				if (obj is My_TextBox.My_TextBox)
				{
					((My_TextBox.My_TextBox)obj).Empty();
				}
				else if (obj is ComboBox)
				{
					((ComboBox)obj).SelectedIndex = ((ComboBox)obj).Items.Count - 1;
				}
			}
		}
		private Employee Read_Form()
		{
			Employee emp = new Employee();
			emp["name"] = my_TextBox1.Text;
			emp.Phone = new List<Phones>();
			Phones phone = new Phones
			{
				Type = (Phone_Type)comboBox1.SelectedItem,
				Number = my_TextBox3.Text
			};
			emp.Phone.Add(phone);
			if (my_TextBox4.Text != string.Empty && my_TextBox4.Text != my_TextBox4.PlaceHolder)
			{
				phone.Type = (Phone_Type)comboBox2.SelectedItem;
				phone.Number = my_TextBox4.Text;
				emp.Phone.Add(phone);
			}
			emp.Address = new List<Addresses>();
			Addresses address = new Addresses
			{
				Street = my_TextBox5.Text,
				Building = my_TextBox6.Text,
				Region = my_TextBox7.Text,
				City = my_TextBox8.Text,
				Country = my_TextBox9.Text
			};
			emp.Address.Add(address);
			emp["mail"] = my_TextBox2.Text;
			return emp;
		}
		private void Add(object sender, EventArgs e)
		{
			inserted_emp.Add(Read_Form());
			inserted_emp.Sort((p, q) => p.Name.CompareTo(q.Name));
			Clear(sender, e);
		}
		private void Save(object sender, EventArgs e)
		{
			inserted_emp.Add(Read_Form());
			inserted_emp.Sort((p, q) => p.Name.CompareTo(q.Name));
			Add2File(Read_Form());
			Clear(sender, e);
		}
		private void Close(object sender, EventArgs e)
		{
			Close();
		}
	}
}
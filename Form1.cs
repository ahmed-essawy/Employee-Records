using System;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace XMLProject
{
    public partial class Form1 : Form
    {
        private List<Employee> employees = new List<Employee>();
        private List<string> loaded_files = new List<string>();
        private int changes = 0;

        public Form1()
        {
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
        }

        private void Fill_Form(int index, int flag = 1)
        {
            label1.Text = employees[index]["name"];
            label3.Text = employees[index]["mail"];
            label2.Text = employees[index].Phone.Count() > 0 ? "Phones:" : "There is no phones";
            listBox1.Items.Clear();
            foreach (Phones item in employees[index].Phone)
            {
                listBox1.Items.Add(item.ToString());
            }
            label4.Text = employees[index].Address.Count() > 0 ? "Addresses:" : "There is no addresses";
            listBox2.Items.Clear();
            foreach (Addresses item in employees[index].Address)
            {
                listBox2.Items.Add(item.ToString());
            }
            if (flag == 1)
            {
                comboBox1.SelectedIndex = index;
            }
            if (employees.Count > 1)
            {
                B_Search.Enabled = true;
                B_Save.Enabled = true;
                B_V1.Enabled = true;
                B_V2.Enabled = true;
                B_V3.Enabled = true;
            }
            else if (employees.Count == 1)
            {
                B_Search.Enabled = false;
                B_Save.Enabled = true;
                B_V1.Enabled = true;
                B_V2.Enabled = true;
                B_V3.Enabled = true;
            }
            if (index >= employees.Count - 1)
                B_Next.Enabled = false;
            else
                B_Next.Enabled = true;
            if (index <= 0)
                B_Prev.Enabled = false;
            else
                B_Prev.Enabled = true;
        }

        private void Retrieve_Box()
        {
            if (employees.Count() > 0)
            {
                comboBox1.Items.Clear();
                foreach (Employee emp in employees)
                {
                    comboBox1.Items.Add(emp.Name);
                }
                Fill_Form(0);
            }
        }

        private void LoadXMLFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            if (Check_Validation(doc))
            {
                foreach (XmlNode node in doc.SelectSingleNode("employees"))
                {
                    Employee emp = new Employee
                    {
                        Name = node.SelectSingleNode("name").InnerText
                    };
                    emp.Phone = new List<Phones>();
                    foreach (XmlNode phone in node.SelectSingleNode("phones"))
                    {
                        Phones temp_phone = new Phones
                        {
                            Number = phone.InnerText
                        };
                        switch (phone.Attributes["type"].Value.ToLower())
                        {
                            case "work":
                                temp_phone.Type = Phone_Type.Work;
                                break;

                            case "home":
                                temp_phone.Type = Phone_Type.Home;
                                break;

                            default:
                                temp_phone.Type = Phone_Type.Mobile;
                                break;
                        }
                        emp.Phone.Add(temp_phone);
                    }
                    emp.Address = new List<Addresses>();
                    foreach (XmlNode address in node.SelectSingleNode("addresses"))
                    {
                        Addresses temp_address = new Addresses
                        {
                            Street = address.SelectSingleNode("street")?.InnerText,
                            Building = address.SelectSingleNode("building")?.InnerText,
                            Region = address.SelectSingleNode("region")?.InnerText,
                            City = address.SelectSingleNode("city")?.InnerText,
                            Country = address.SelectSingleNode("country")?.InnerText
                        };
                        emp.Address.Add(temp_address);
                    }
                    emp.Mail = node.SelectSingleNode("mail").InnerText;
                    employees.Add(emp);
                }
                if (!loaded_files.Contains(path))
                    loaded_files.Add(path);
                employees.Sort((p, q) => p.Name.CompareTo(q.Name));
                Retrieve_Box();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
                Fill_Form(comboBox1.SelectedIndex);
            counter1.Num1 = comboBox1.SelectedIndex + 1;
            counter1.Num2 = employees.Count;
        }

        private void Previous(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index > 0 && index < employees.Count())
            {
                Fill_Form(index - 1);
            }
        }

        private void Next(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index >= 0 && index < employees.Count() - 1)
            {
                Fill_Form(index + 1);
            }
        }

        private void New(object sender, EventArgs e)
        {
            Insert insert_form = new Insert();
            DialogResult response = insert_form.ShowDialog();
            if (insert_form.New_Emp.Count > 0)
            {
                foreach (Employee emp in insert_form.New_Emp)
                {
                    employees.Add(emp);
                    comboBox1.Items.Add(emp.Name);
                    ++changes;
                    B_Save.Text = "Save (" + changes + ")";
                }
                Fill_Form(employees.Count() - 1);
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index >= 0)
            {
                Edit edit_form = new Edit(employees[index]);
                DialogResult response = edit_form.ShowDialog();
                if (response == DialogResult.OK)
                {
                    employees[index] = edit_form.Edited_Emp;
                    string name = employees[index].Name;
                    employees.Sort((p, q) => p.Name.CompareTo(q.Name));
                    Retrieve_Box();
                    Fill_Form(comboBox1.Items.IndexOf(name));
                    ++changes;
                    B_Save.Text = "Save (" + changes + ")";
                }
            }
        }

        private void Open(object sender, EventArgs e)
        {
            DialogResult resp = openFileDialog1.ShowDialog();
            if (resp == DialogResult.OK)
            {
                string file_name = openFileDialog1.FileName;
                if (loaded_files.Contains(file_name))
                {
                    DialogResult reload_resp = MessageBox.Show("This file has been loaded before\nDo you want to load file again ?", "Re-Load File", MessageBoxButtons.YesNo);
                    if (reload_resp == DialogResult.Yes)
                    {
                        LoadXMLFile(file_name);
                    }
                }
                else
                {
                    LoadXMLFile(file_name);
                }
            }
        }

        private void Copy(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                Employee emp = new Employee();
                emp = employees[comboBox1.SelectedIndex];
                string copied = "Name: " + emp.Name;
                copied += Environment.NewLine + "Email: " + emp.Mail;
                copied += Environment.NewLine + "Phones:";
                foreach (Phones item in emp.Phone)
                {
                    copied += Environment.NewLine + "\t" + item.ToString();
                }
                copied += Environment.NewLine + "Addresses:";
                foreach (Addresses item in emp.Address)
                {
                    copied += Environment.NewLine + "\t" + item.ToString();
                }
                Clipboard.SetText(copied);
            }
        }

        private void Delete(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index >= 0)
            {
                Next(sender, e);
                if (index == comboBox1.SelectedIndex)
                {
                    Previous(sender, e);
                }
                employees.RemoveAt(index);
                comboBox1.Items.RemoveAt(index);
                ComboBox1_SelectedIndexChanged(sender, e);
                ++changes;
                B_Save.Text = "Save (" + changes + ")";
            }
            if (employees.Count() == 0)
            {
                this.comboBox1.Text = "No names found!";
                Clear_Form();
                loaded_files.Clear();
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void Save(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            string path = saveFileDialog1.FileName;
            XmlDocument doc = new XmlDocument();
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
            doc.Load(path);
            XmlNode root = doc.SelectSingleNode("employees");
            employees.Sort((p, q) => p.Name.CompareTo(q.Name));
            foreach (Employee emp in employees)
            {
                XmlNode cur_emp = root.AppendChild(doc.CreateElement("employee"));
                XmlElement name = doc.CreateElement("name");
                name.InnerText = emp.Name;
                cur_emp.AppendChild(name);
                XmlElement phones = doc.CreateElement("phones");
                foreach (Phones item in emp.Phone)
                {
                    XmlElement phone = doc.CreateElement("phone");
                    phone.InnerText = item.Number;
                    phone.SetAttribute("type", item.Type.ToString());
                    phones.AppendChild(phone);
                }
                cur_emp.AppendChild(phones);
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
                cur_emp.AppendChild(addresses);
                XmlElement mail = doc.CreateElement("mail");
                mail.InnerText = emp.Mail;
                cur_emp.AppendChild(mail);
            }
            if (Check_Validation(doc))
                doc.Save(path);
            if (!loaded_files.Contains(path))
                loaded_files.Add(path);
        }

        private void Cut(object sender, EventArgs e)
        {
            Copy(sender, e);
            Delete(sender, e);
        }

        private void Clear_Form()
        {
            this.label1.Text = this.label2.Text = this.label3.Text = this.label4.Text = string.Empty;
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            B_Search.Enabled = false;
            B_Save.Enabled = false;
            B_V1.Enabled = false;
            B_V2.Enabled = false;
            B_V3.Enabled = false;
            changes = 0;
            B_Save.Text = "Save";
        }

        private void Search(object sender, EventArgs e)
        {
            for (int i = 0, flag = 0; flag == 0 && i < employees.Count(); i++)
            {
                if (employees[i].Name.ToLower().Contains(comboBox1.Text.ToLower()))
                {
                    Fill_Form(i, 0);
                    flag = 1;
                }
                else
                {
                    this.label1.Text = "Name didn't match any results";
                    this.label2.Text = this.label3.Text = this.label4.Text = string.Empty;
                    this.listBox1.Items.Clear();
                    this.listBox2.Items.Clear();
                }
            }
        }

        private void Search_Button(object sender, EventArgs e)
        {
            comboBox1.Focus();
            Dictionary<string, int> results = new Dictionary<string, int>();
            for (int i = 0; i < employees.Count(); i++)
            {
                if (employees[i].Name.ToLower().Contains(comboBox1.Text.ToLower()))
                {
                    results[employees[i].Name] = i;
                }
            }
            if (results.Count() > 1)
            {
                Search choosen = new Search(results);
                if (choosen.ShowDialog() == DialogResult.OK)
                {
                    Fill_Form(choosen.Choosen);
                }
            }
        }

        private void ComboBox_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Search_Button(sender, e);
            }
        }

        public static bool Check_Validation(XmlDocument doc)
        {
            doc.Schemas.Add("", XmlReader.Create("../../sources/Schema.xsd"));
            doc.Validate((s, e) => MessageBox.Show("An error occurred while validating xml document!\nError: " + e.Message));
            return doc.SchemaInfo.Validity == System.Xml.Schema.XmlSchemaValidity.Valid ? true : false;
        }

        private void ViewClick(object sender, EventArgs e)
        {
            string path = Check_in_XML();
            string name = label1.Text;
            if (path != null)
            {
                string clicker = ((Button)sender).Text;
                XmlTextWriter generator = new XmlTextWriter("../../sources/temp/View" + clicker + "/" + name + ".html", null);
                XsltArgumentList args = new XsltArgumentList();
                args.AddParam("selected", "", name);
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                myXslTrans.Load("../../sources/View" + clicker + ".xsl");
                myXslTrans.Transform(new XPathDocument(path), args, generator);
                System.Diagnostics.Process.Start("Chrome", Uri.EscapeDataString("../../sources/temp/View" + clicker + "/" + name + ".html"));
            }
            else
            {
                DialogResult reload_resp = MessageBox.Show("Couldn't find reference file for " + name + "\nDo you want to save file?", "WARRNING", MessageBoxButtons.YesNo);
                if (reload_resp == DialogResult.Yes)
                {
                    Save(sender, e);
                    ViewClick(sender, e);
                }
            }
        }

        private string Check_in_XML()
        {
            string ret_val = null;
            foreach (string path in loaded_files)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    bool check = doc.InnerText.ToLower().Contains(label1.Text.ToLower());
                    if (check)
                        ret_val = path;
                }
                catch (Exception) { }
            }
            return ret_val;
        }

        private void Before_Close(object sender, FormClosingEventArgs e)
        {
            if (changes > 0 && MessageBox.Show("There are " + changes + " unsaved changes\nDo you want to save changes before closing?", "Save changes",
         MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Save(sender, e);
            }
            for (int i = 1; i <= 3; i++)
            {
                foreach (System.IO.FileInfo file in new System.IO.DirectoryInfo("../../sources/temp/View" + i).GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception) { }
                }
            }
        }

        private void exportToDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.IndexOf(label1.Text) >= 0)
            {
                try
                {
                    Employee emp = employees[comboBox1.Items.IndexOf(label1.Text)];
                    sqlConnection1.Open();
                    this.sqlCommand1.CommandText = "insert into Employees ([Name],[E-mail]) values ('" + emp.Name + "','" + emp.Mail + "')";
                    sqlCommand1.ExecuteNonQuery();
                    foreach (Phones item in emp.Phone)
                    {
                        this.sqlCommand1.CommandText = "insert into Phones values ('" + emp.Mail + "','" + item.Number + "','" + item.Type.ToString() + "')";
                        sqlCommand1.ExecuteNonQuery();
                    }
                    foreach (Addresses item in emp.Address)
                    {
                        this.sqlCommand1.CommandText = "insert into Addresses values ('" + emp.Mail + "','" + item.Street + "','" + item.Building + "','" + item.Region + "','" + item.City + "','" + item.Country + "')";
                        sqlCommand1.ExecuteNonQuery();
                    }
                    sqlConnection1.Close();
                }
                catch (Exception) { }
                finally
                {
                    sqlConnection1.Close();
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sqlConnection1.Open();
            sqlCommand1.CommandText = "SELECT * FROM Employees";
            SqlDataReader reader = sqlCommand1.ExecuteReader();
            while (reader.Read())
            {
                Employee emp = new Employee
                {
                    Mail = reader["E-mail"].ToString(),
                    Name = reader["Name"].ToString()
                };
                employees.Add(emp);
            }
            reader.Close();
            foreach (Employee emp in employees)
            {
                sqlCommand1.CommandText = "SELECT * FROM Phones WHERE [E-mail] = '" + emp.Mail + "'";
                reader = sqlCommand1.ExecuteReader();
                emp.Phone = new List<Phones>();
                while (reader.Read())
                {
                    Phones temp_phone = new Phones
                    {
                        Number = reader["Phone"].ToString()
                    };
                    switch (reader["Type"].ToString().ToLower())
                    {
                        case "work":
                            temp_phone.Type = Phone_Type.Work;
                            break;

                        case "home":
                            temp_phone.Type = Phone_Type.Home;
                            break;

                        default:
                            temp_phone.Type = Phone_Type.Mobile;
                            break;
                    }
                    emp.Phone.Add(temp_phone);
                }
                reader.Close();
                sqlCommand1.CommandText = "SELECT * FROM Addresses WHERE [E-mail] = '" + emp.Mail + "'";
                reader = sqlCommand1.ExecuteReader();
                emp.Address = new List<Addresses>();
                while (reader.Read())
                {
                    Addresses temp_address = new Addresses
                    {
                        Street = reader["Street"].ToString(),
                        Building = reader["Building"].ToString(),
                        Region = reader["Region"].ToString(),
                        City = reader["City"].ToString(),
                        Country = reader["Country"].ToString()
                    };
                    emp.Address.Add(temp_address);
                }
                reader.Close();
            }
            sqlConnection1.Close();
            employees.Sort((p, q) => p.Name.CompareTo(q.Name));
            Retrieve_Box();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Employee emp in employees)
            {
                try
                {
                    sqlConnection1.Open();
                    this.sqlCommand1.CommandText = "insert into Employees ([Name],[E-mail]) values ('" + emp.Name + "','" + emp.Mail + "')";
                    sqlCommand1.ExecuteNonQuery();
                    foreach (Phones item in emp.Phone)
                    {
                        this.sqlCommand1.CommandText = "insert into Phones values ('" + emp.Mail + "','" + item.Number + "','" + item.Type.ToString() + "')";
                        sqlCommand1.ExecuteNonQuery();
                    }
                    foreach (Addresses item in emp.Address)
                    {
                        this.sqlCommand1.CommandText = "insert into Addresses values ('" + emp.Mail + "','" + item.Street + "','" + item.Building + "','" + item.Region + "','" + item.City + "','" + item.Country + "')";
                        sqlCommand1.ExecuteNonQuery();
                    }
                    sqlConnection1.Close();
                }
                catch (Exception) { }
                finally
                {
                    sqlConnection1.Close();
                }
            }
        }

        private void emptyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sqlConnection1.Open();
            this.sqlCommand1.CommandText = "DELETE Employees";
            sqlCommand1.ExecuteNonQuery();
            sqlConnection1.Close();
        }
    }
}
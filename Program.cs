using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XMLProject
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
	public enum Phone_Type { Work, Home, Mobile };
	public struct Addresses
	{
		string street;
		string building;
		string region;
		string city;
		string country;
		public string Street { get => street; set => street = value; }
		public string Building { get => building; set => building = value; }
		public string Region { get => region; set => region = value; }
		public string City { get => city; set => city = value; }
		public string Country { get => country; set => country = value; }
		public override string ToString()
		{
			return building + ", " + street + ", " + region + ", " + city + ", " + country;
		}
	}
	public struct Phones
	{
		Phone_Type type;
		string number;
		public Phone_Type Type { get => type; set => type = value; }
		public string Number { get => number; set => number = value; }
		public override string ToString()
		{
			return type + ": " + number;
		}
	}
	public class Employee
	{
		string name;
		List<Phones> phone;
		List<Addresses> address;
		string mail;
		public string Name { get => name; set => name = value; }
		public List<Phones> Phone { get => phone; set => phone = value; }
		public List<Addresses> Address { get => address; set => address = value; }
		public string Mail { get => mail; set => mail = value; }
		public string this[String key]
		{
			get
			{
				if(key == "name") { return name; }
				else if(key == "mail") { return mail; }
				else if(key == "phone") { return phone[0].Number; }
				else if(key == "address") { return address[0].Building + ", " + address[0].Street + ", " + address[0].Region + ", " + address[0].City + ", " + address[0].Country; }
				else { return String.Empty; }
			}
			set
			{
				if(key == "name") { name = value; }
				else if(key == "mail") { mail = value; }
			}
		}
	}
}

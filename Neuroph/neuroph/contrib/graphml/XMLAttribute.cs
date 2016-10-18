using System;

namespace org.neuroph.contrib.graphml
{

	/// <summary>
	/// XML Attribute. Contains id value pairs to characterize XML elements. 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class XMLAttribute
	{
		private string id;
		private string value;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public XMLAttribute(final String id, final String value)
		public XMLAttribute(string id, string value)
		{
			this.id = id;
			this.value = value;
		}

		public override string ToString()
		{
			return Id + "=\"" + Value + "\"";
		}

		//Getter 
		public virtual string Id
		{
			get
			{
				return id;
			}
		}
		public virtual string Value
		{
			get
			{
				return value;
			}
		}

		public static void Main(string[] args)
		{
			XMLAttribute attribute = new XMLAttribute("id", "testValue");
			System.Console.WriteLine(attribute);
		}
	}

}
using System;

namespace org.neuroph.contrib.graphml
{

	/// 
	/// <summary>
	/// XML Data. 
	/// 
	/// Holds two attributes:
	///  
	/// 1. The id of the key this data is referring to. 
	/// 2. The value for the referrenced attribute.  
	///    
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// </summary>
	public class Data : XMLElement
	{

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public Data(final String keyId, final String value)
		public Data(string keyId, string value)
		{
			addAttribute(new XMLAttribute("key", keyId));
			addAttribute(new XMLAttribute("value", value));
		}


		public override string ToString()
		{

			string @out = StartTag;
			@out += Value;
			@out += EndTag;

			return @out;
		}

		private string StartTag
		{
			get
			{
				return "<" + Tag + " " + Attributes[0].ToString() + ">";
			}
		}
		private string Value
		{
			get
			{
				return Attributes[1].Value;
			}
		}
		private string EndTag
		{
			get
			{
				return "</" + Tag + ">";
			}
		}

		public override string Tag
		{
			get
			{
				return "data";
			}
		}

		public static void Main(string[] args)
		{
			System.Console.WriteLine(new Data("d1", "0.0"));
		}

	}

}
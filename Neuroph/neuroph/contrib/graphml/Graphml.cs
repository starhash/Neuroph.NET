namespace org.neuroph.contrib.graphml
{

	/// <summary>
	/// The parent XML element for a graphml document. 
	/// 
	/// Holds header containing namespace and schema links. 
	/// 
	/// Holds key definitions.
	/// 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class Graphml : XMLElement
	{

		public Graphml()
		{
			addAttribute(new XMLAttribute("xmlns", NameSpace));
			addAttribute(new XMLAttribute("xmlns:xsi", XsiNameSpace));
			addAttribute(new XMLAttribute("xsi:schemaLocation", XsiSchemaLocation));

			appendChild(new Key("d1", "edge", "weight", "double"));
		}

		private static string NameSpace
		{
			get
			{
				return "http://graphml.graphdrawing.org/xmlns";
			}
		}
		private static string XsiNameSpace
		{
			get
			{
				return "http://www.w3.org/2001/XMLSchema-instance";
			}
		}
		private static string XsiSchemaLocation
		{
			get
			{
				return "http://graphml.graphdrawing.org/xmlns http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd";
			}
		}

		public override string Tag
		{
			get
			{
				return "graphml";
			}
		}
	}

}
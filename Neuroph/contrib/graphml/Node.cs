namespace org.neuroph.contrib.graphml
{
	/// <summary>
	/// XML Element representing a graph node. 
	/// 
	/// A Node is characterized by a single Id Attribute. 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class Node : XMLElement
	{
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public Node(final String id)
		public Node(string id)
		{
			addAttribute(new XMLAttribute("id", id));
		}

		public override string Tag
		{
			get
			{
				return "node";
			}
		}
	}

}
namespace org.neuroph.contrib.graphml
{

	/// <summary>
	/// XML edge element.  
	/// 
	/// The edge element is characterized by two XML Elements  
	/// 
	/// 1. The source node id
	/// 2. The target node id 
	/// 
	/// And a data field holding the weight of the edge. 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class Edge : XMLElement
	{
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public Edge(final String sourceId, final String targetId, final String weightKeyId, final String weight)
		public Edge(string sourceId, string targetId, string weightKeyId, string weight)
		{
			addAttribute(new XMLAttribute("source", sourceId));
			addAttribute(new XMLAttribute("target", targetId));

			this.appendChild(new Data(weightKeyId, weight));
		}

		public override string Tag
		{
			get
			{
				return "edge";
			}
		}
	}

}
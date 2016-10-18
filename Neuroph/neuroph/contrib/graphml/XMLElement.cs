using System.Collections.Generic;

namespace org.neuroph.contrib.graphml
{

	/// <summary>
	/// XML element consisting of 
	/// i) a tag
	/// ii) a list of attributes
	/// iii) a list of child elements. 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public abstract class XMLElement
	{
		private List<XMLAttribute> attributes = new List<XMLAttribute>();
		private List<XMLElement> childElements = new List<XMLElement>();


		/// <summary>
		/// Append child element. </summary>
		/// <param name="child"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public void appendChild(final XMLElement child)
		public virtual void appendChild(XMLElement child)
		{
			this.childElements.Add(child);
		}

		/// <summary>
		/// Add attribute. </summary>
		/// <param name="attribute"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public void addAttribute(final XMLAttribute attribute)
		public virtual void addAttribute(XMLAttribute attribute)
		{
			this.attributes.Add(attribute);
		}

		public override string ToString()
		{

			string @out = StartTag + ">";
			if (ChildElements.Count != 0)
			{
				@out += "\n";
				@out += ChildElementsString;
			}

			@out += EndTag;

			return @out;
		}

		/// <summary>
		/// Returns XML start tag including all attributes. 
		/// @return
		/// </summary>
		private string StartTag
		{
			get
			{
				return "<" + Tag + AttributesString;
			}
		}

		/// <summary>
		/// Returns XML end tag. 
		/// @return
		/// </summary>
		private string EndTag
		{
			get
			{
				return "</" + Tag + ">";
			}
		}

		/// <summary>
		/// Returns all attribute fields separated by a single whitespace. 
		/// @return
		/// </summary>
		private string AttributesString
		{
			get
			{
				string @out = "";
				foreach (XMLAttribute attribute in Attributes)
				{
					@out += " " + attribute.ToString();
				}
    
				return @out;
			}
		}

		/// <summary>
		/// Gets XML representation of child elements indented by 4 whitespace characters. 
		/// @return
		/// </summary>
		private string ChildElementsString
		{
			get
			{
				string @out = "";
				foreach (XMLElement child in ChildElements)
				{
					@out += child.ToString().Replace("\n", "\n    ") + "\n";
				}
    
				return @out;
			}
		}

		//Getter methods.
		public abstract string Tag {get;}
		public virtual List<XMLAttribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}
		public virtual List<XMLElement> ChildElements
		{
			get
			{
				return this.childElements;
			}
		}
	}

}
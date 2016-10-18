using System;
using System.Collections.Generic;

/// <summary>
/// *
/// Neuroph http://neuroph.sourceforge.net Copyright by Neuroph Project (C) 2008
/// 
/// This file is part of Neuroph framework.
/// 
/// Neuroph is free software; you can redistribute it and/or modify it under the
/// terms of the GNU Lesser General Public License as published by the Free
/// Software Foundation; either version 3 of the License, or (at your option) any
/// later version.
/// 
/// Neuroph is distributed in the hope that it will be useful, but WITHOUT ANY
/// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
/// A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
/// details.
/// 
/// You should have received a copy of the GNU Lesser General Public License
/// along with Neuroph. If not, see <http://www.gnu.org/licenses/>.
/// </summary>
namespace org.neuroph.imgrec
{


	/// <summary>
	/// This class provides Iterator for the image files (jpg and png) from the
	/// specified directory
	/// next() method loads and returns File objects
	/// 
	/// @author Jon Tait
	/// </summary>
	public class ImageFilesIterator : IEnumerator<File>
	{

		private IEnumerator<File> imageIterator;
		private string currentFilename = null;

		/// <summary>
		/// Creates image files iterator for the specified directory
		/// </summary>
		/// <param name="dir"> Directory to iterate images from </param>
		/// <exception cref="java.io.IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public ImageFilesIterator(java.io.File dir) throws java.io.IOException
		public ImageFilesIterator(File dir)
		{
			if (!dir.Directory)
			{
				throw new System.ArgumentException(dir + " is not a directory!");
			}

			string[] imageFilenames = dir.list(new FilenameFilterAnonymousInnerClassHelper(this, dir));

			List<File> imageFiles = new List<File>();
			foreach (string imageFile in imageFilenames)
			{
				imageFiles.Add(new File(dir, imageFile));
			}

			imageIterator = imageFiles.GetEnumerator();
		}

		private class FilenameFilterAnonymousInnerClassHelper : FilenameFilter
		{
			private readonly ImageFilesIterator outerInstance;

			private File dir;

			public FilenameFilterAnonymousInnerClassHelper(ImageFilesIterator outerInstance, File dir)
			{
				this.outerInstance = outerInstance;
				this.dir = dir;
			}

			public virtual bool accept(File dir, string name)
			{
				if (name.Length > 4)
				{
					string fileExtension = StringHelperClass.SubstringSpecial(name, name.Length - 4, name.Length);
					return ".jpg".Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase) || ".png".Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase);
				}
				return false;
			}
		}

		public virtual bool hasNext()
		{
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			return imageIterator.hasNext();
		}

		/// <summary>
		/// Loads and returns next image
		/// </summary>
		/// <returns> Retruns next image file from directory as File object </returns>
		public virtual File next()
		{
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			File nextFile = imageIterator.next();
			currentFilename = nextFile.Name;
			return nextFile;
		}

		public virtual void remove()
		{
			imageIterator.remove();
		}

		public virtual string FilenameOfCurrentImage
		{
			get
			{
				return currentFilename;
			}
		}
	}

}
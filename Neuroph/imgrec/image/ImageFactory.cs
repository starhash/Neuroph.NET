using System;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.imgrec.image
{


	/// <summary>
	/// This class creates image instances depending on the run-time platform:
	/// ImageJ2SE - for J2SE
	/// ImageAndroid - for Android
	/// @author dmicic
	/// </summary>
	public class ImageFactory
	{

		private static Image image;
		private const string IMAGE_ANDROID_CLASS_NAME = "org.neuroph.imgrec.image.ImageAndroid";
		private const string IMAGE_J2SE_CLASS_NAME = "org.neuroph.imgrec.image.ImageJ2SE";
		private static Type imageClass;
		private static Constructor constructor;

		static ImageFactory()
		{

			try
			{
				if (System.getProperty("java.vendor").ToLower().IndexOf("android", StringComparison.Ordinal) != -1)
				{
					imageClass = Type.GetType(IMAGE_ANDROID_CLASS_NAME);
				}
				else
				{
					imageClass = Type.GetType(IMAGE_J2SE_CLASS_NAME);
				}

			}
			catch (ClassNotFoundException cnf)
			{
				Console.Error.WriteLine(cnf.Message);
			}

		}

		public static Image createImage(int? width, int? height, int? imageType)
		{
			try
			{
				constructor = imageClass.getDeclaredConstructor(new Type[]{typeof(int?), typeof(int?), typeof(int?)});
				constructor.Accessible = true;
				image = (Image) constructor.newInstance(width, height, imageType);
			}
			catch (Exception e)
			{
				handleException(e);
			}

			return image;
		}

		public static Image getImage(URL imageUrl)
		{
			try
			{
				constructor = imageClass.getDeclaredConstructor(new Type[]{typeof(URL)});
				constructor.Accessible = true;
				image = (Image) constructor.newInstance(imageUrl);
			}
			catch (Exception e)
			{
				handleException(e);
			}

			return image;
		}

		public static Image getImage(File imageFile)
		{
			try
			{
				constructor = imageClass.getDeclaredConstructor(new Type[]{typeof(File)});
				constructor.Accessible = true;
				image = (Image) constructor.newInstance(imageFile);
			}
			catch (Exception e)
			{
				handleException(e);
			}

			return image;
		}

		public static Image getImage(string filePath)
		{
			try
			{
				constructor = imageClass.getDeclaredConstructor(new Type[]{typeof(string)});
				constructor.Accessible = true;
				image = (Image) constructor.newInstance(filePath);
			}
			catch (Exception e)
			{
				handleException(e);
			}

			return image;
		}

		private static void handleException(Exception e)
		{
			Console.Error.WriteLine(e.Message);
		}

	//    public static Image resizeImage(Image image, int width, int height) {
	//        return image.resize(width, height);
	//    }

	//    public static Image cropImage(Image image, int x1, int y1, int x2, int y2) {
	//        return image.cropImage(image, x1, y1, x2, y2);
	//    }
	}

}
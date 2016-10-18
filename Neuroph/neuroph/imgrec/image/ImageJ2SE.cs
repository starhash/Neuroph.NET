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
	/// This class represents image on J2SE platform, and the Image interface is used to provide compatibility with Android platform.
	/// It is a wrapper around BufferedImage and implementation of Image interface.
	/// @author dmicic
	/// </summary>
	public class ImageJ2SE : Image
	{

		private BufferedImage bufferedImage;

		public virtual BufferedImage BufferedImage
		{
			get
			{
				return bufferedImage;
			}
			set
			{
				this.bufferedImage = value;
			}
		}


		private ImageJ2SE()
		{
		}

		private ImageJ2SE(int? width, int? height, int? imageType)
		{
			bufferedImage = new BufferedImage(width, height, checkImageType(imageType));
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private ImageJ2SE(java.io.File imageFile) throws java.io.IOException
		private ImageJ2SE(File imageFile)
		{
			bufferedImage = ImageIO.read(imageFile);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private ImageJ2SE(String filePath) throws java.io.IOException
		private ImageJ2SE(string filePath)
		{
			bufferedImage = ImageIO.read(new File(filePath));
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private ImageJ2SE(java.net.URL imageUrl) throws java.io.IOException
		private ImageJ2SE(URL imageUrl)
		{
			bufferedImage = ImageIO.read(imageUrl);
		}

		public ImageJ2SE(BufferedImage bufferedImage)
		{
			this.bufferedImage = bufferedImage;
		}

		public virtual int getPixel(int x, int y)
		{
			return bufferedImage.getRGB(x, y);
		}

		public virtual void setPixel(int x, int y, int color)
		{
			bufferedImage.setRGB(x, y, color);
		}

		public virtual int[] getPixels(int offset, int stride, int x, int y, int width, int height)
		{
			int[] pixels = new int[width * height];
			bufferedImage.getRGB(x, y, width, height, pixels, offset, stride);
			return pixels;
		}

		public virtual void setPixels(int[] pixels, int offset, int stride, int x, int y, int width, int height)
		{
			bufferedImage.setRGB(x, y, width, height, pixels, offset, stride);
		}

		public virtual int Width
		{
			get
			{
				return bufferedImage.Width;
			}
		}

		public virtual int Height
		{
			get
			{
				return bufferedImage.Height;
			}
		}

		public virtual Image resize(int width, int height)
		{
			BufferedImage resizedImage = ImageUtilities.resizeImage(bufferedImage, width, height);
			return new ImageJ2SE(resizedImage);
		}

		public virtual Image crop(int x1, int y1, int x2, int y2)
		{
			BufferedImage croppedImage = ImageUtilities.cropImage(bufferedImage, x1, y1, x2, y2);
			return new ImageJ2SE(croppedImage);
	//        ((ImageJ2SE) image).setBufferedImage(((ImageJ2SE) image).getBufferedImage().getSubimage(x1, y1, x2 - x1, y2 - y1));
	//        return image;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private int checkImageType(int imageType) throws IllegalArgumentException
		private int checkImageType(int imageType)
		{
			if (imageType < ImageType.J2SE_TYPE_CUSTOM || imageType > ImageType.J2SE_TYPE_BYTE_INDEXED)
			{
				throw new System.ArgumentException("Illegal image type, image type: " + imageType);
			}
			else
			{
				return imageType;
			}
		}

		public virtual int Type
		{
			get
			{
				return bufferedImage.Type;
			}
		}
	}

}
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

	using Bitmap = android.graphics.Bitmap;
	using BitmapFactory = android.graphics.BitmapFactory;

	/// <summary>
	/// This class represents image on Android platform, and the Image interface is used to provide compatibility with J2SE images.
	/// It is a wrapper around Bitmap and implementation of Image interface.
	/// @author dmicic
	/// </summary>
	public class ImageAndroid : Image
	{

		private Bitmap bitmap;

		public ImageAndroid(Bitmap bitmap)
		{
		   this.bitmap = bitmap;
		}

		public virtual Bitmap Bitmap
		{
			get
			{
				return bitmap;
			}
			set
			{
				this.bitmap = value;
			}
		}


		private ImageAndroid(int width, int height, int imageType)
		{
			bitmap = Bitmap.createBitmap(width, height, imageTypeToBitmapConfig(imageType));
		}

		private ImageAndroid(File imageFile)
		{
			bitmap = BitmapFactory.decodeFile(imageFile.AbsolutePath);
		}

		private ImageAndroid(string filePath)
		{
			bitmap = BitmapFactory.decodeFile(filePath);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private ImageAndroid(java.net.URL imageUrl) throws java.io.IOException
		private ImageAndroid(URL imageUrl)
		{
			bitmap = BitmapFactory.decodeStream((InputStream) imageUrl.Content);
		}

		public virtual int getPixel(int x, int y)
		{
			return bitmap.getPixel(x, y);
		}

		public virtual void setPixel(int x, int y, int color)
		{
			bitmap.setPixel(x, y, color);
		}

		public virtual int[] getPixels(int offset, int stride, int x, int y, int width, int height)
		{
			int[] pixels = new int[width * height];
			bitmap.getPixels(pixels, offset, stride, x, y, width, height);
			return pixels;
		}

		public virtual void setPixels(int[] pixels, int offset, int stride, int x, int y, int width, int height)
		{
			bitmap.setPixels(pixels, offset, stride, x, y, width, height);
		}

		public virtual int Width
		{
			get
			{
				return bitmap.Width;
			}
		}

		public virtual int Height
		{
			get
			{
				return bitmap.Height;
			}
		}

		public virtual Image resize(int width, int height)
		{
			bitmap = Bitmap.createScaledBitmap(bitmap, width, height, true);
			return new ImageAndroid(bitmap);
		}

		public virtual Image crop(int x1, int y1, int x2, int y2)
		{
			return new ImageAndroid(Bitmap.createBitmap(bitmap, x1, y1, x2 - x1, y2 - y1));
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private android.graphics.Bitmap.Config imageTypeToBitmapConfig(int imageType) throws IllegalArgumentException
		private Bitmap.Config imageTypeToBitmapConfig(int imageType)
		{
			Bitmap.Config bitmapConfig = null;
			switch (imageType)
			{
				case ImageType.ANDROID_TYPE_ALPHA_8:
					bitmapConfig = Bitmap.Config.ALPHA_8;
					break;
				case ImageType.ANDROID_TYPE_ARGB_8888:
					bitmapConfig = Bitmap.Config.ARGB_8888;
					break;
				case ImageType.ANDROID_TYPE_RGB_565:
					bitmapConfig = Bitmap.Config.RGB_565;
					break;
				default:
					throw new System.ArgumentException("Illegal image type, image type: " + imageType);
			}

			return bitmapConfig;
		}

		public virtual int Type
		{
			get
			{
				return ImageType.ANDROID_TYPE_ALPHA_8; // FIX how to get bitmap type?
			}
		}
	}

}
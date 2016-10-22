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
	/// This class is interface abstraction for images on J2SE (BufferedImage) and Android (Bitmap)
	/// @author dmicic
	/// </summary>
	public interface Image
	{

		int getPixel(int x, int y);

		void setPixel(int x, int y, int color);

		int[] getPixels(int offset, int stride, int x, int y, int width, int height);

		void setPixels(int[] pixels, int offset, int stride, int x, int y, int width, int height);

		int Width {get;}

		int Height {get;}

		Image resize(int width, int height);

		Image crop(int x1, int y1, int x2, int y2);

		int Type {get;}
	}

}
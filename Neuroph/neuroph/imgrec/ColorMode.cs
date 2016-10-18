using System.Collections.Generic;

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

namespace org.neuroph.imgrec
{

	/// <summary>
	/// Represents the color modes for image recognition.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public sealed class ColorMode
	{
		COLOR_RGB,
		public static readonly ColorMode COLOR_RGB = new ColorMode("COLOR_RGB", InnerEnum.COLOR_RGB);
		COLOR_HSL,
		public static readonly ColorMode COLOR_HSL = new ColorMode("COLOR_HSL", InnerEnum.COLOR_HSL);
		BLACK_AND_WHITE
		public static readonly ColorMode BLACK_AND_WHITE = new ColorMode("BLACK_AND_WHITE", InnerEnum.BLACK_AND_WHITE);

		private static readonly List<ColorMode> valueList = new List<ColorMode>();

		static ColorMode()
		{
			valueList.Add(COLOR_RGB);
			valueList.Add(COLOR_HSL);
			valueList.Add(BLACK_AND_WHITE);
		}

		public enum InnerEnum
		{
			COLOR_RGB,
			COLOR_HSL,
			BLACK_AND_WHITE
		}

		private readonly string nameValue;
		private readonly int ordinalValue;
		private readonly InnerEnum innerEnumValue;
		private static int nextOrdinal = 0;

		public static List<ColorMode> values()
		{
			return valueList;
		}

		public InnerEnum InnerEnumValue()
		{
			return innerEnumValue;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static ColorMode valueOf(string name)
		{
			foreach (ColorMode enumInstance in ColorMode.values())
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}
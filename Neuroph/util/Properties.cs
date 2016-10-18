using System.Collections;

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

namespace org.neuroph.util
{

	/// <summary>
	/// Represents a general set of properties for neuroph objects
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class Properties : Hashtable
	{
		private const long serialVersionUID = 1L;


			protected internal virtual void createKeys(params string[] keys)
			{
				for (int i = 0; i < keys.Length; i++)
				{
			this[keys[i]] = "";
				}
			}

		public virtual void setProperty(string key, object value)
		{
	//                if (!this.containsKey(key))
	//                    throw new RuntimeException("Unknown property key: "+key);

			this[key] = value;
		}

			public virtual object getProperty(string key)
			{
	//                if (!this.containsKey(key))
	//                        throw new RuntimeException("Unknown property key: "+key);

					return this[key];
			}

			public virtual bool hasProperty(string key)
			{
				return this.ContainsKey(key);
			}

	}

}
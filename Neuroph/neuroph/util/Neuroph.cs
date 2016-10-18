using java.io;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace org.neuroph.util {

    //import org.encog.engine.EncogEngine;



    /// <summary>
    /// This singleton holds global settings for the whole framework
    /// @author Jeff Heaton
    /// </summary>
    public class Neuroph {

        private static Neuroph instance;

        /// <summary>
        /// Flag to determine if flat network support from Encog is turned on
        /// </summary>
        private bool flattenNetworks = false;

        public static Neuroph Instance {
            get {
                if (instance == null) {
                    instance = new Neuroph();
                }
                return instance;
            }
        }

        public static string Version {
            get {
                return "2.8";
            }
        }

        /// <summary>
        /// Get setting for flatten network (from Encog engine) </summary>
        /// <returns> the flattenNetworks </returns>
        public virtual bool shouldFlattenNetworks() {
            return flattenNetworks;
        }

        /// <summary>
        /// Turn on/off flat networ support from Encog </summary>
        /// <param name="flattenNetworks"> the flattenNetworks to set </param>
        public virtual bool FlattenNetworks {
            set {
                this.flattenNetworks = value;
            }
        }

        /// <summary>
        /// Shuts down the Encog engine
        /// </summary>
        public virtual void shutdown() {
            //EncogEngine.getInstance().shutdown();
        }

        public virtual List<string> InputFunctions {
            get {
                try {//org.neuroph.core.input
                    string @namespace = "org.neuroph.core.input";

                    var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == @namespace select t; List<string> classes = q.Select((x) => x.Name).ToList();
                    classes.Remove("InputFunction");
                    return classes;
                } catch (java.io.IOException) {
                    //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                    return null;
                }
            }
        }

        public virtual List<string> TransferFunctions {
            get {
                try {
                    var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == "org.neuroph.core.transfer" select t; List<string> classes = q.Select((x) => x.Name).ToList();
                    classes.Remove("TransferFunction");
                    return classes;
                } catch (java.io.IOException) {
                    //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:

                    return null;
                }
            }
        }

        public virtual List<string> Neurons {
            get {
                try {
                    var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == "org.neuroph.nnet.comp.neuron" select t; List<string> classes = q.Select((x) => x.Name).ToList();
                    classes.Insert(0, "Neuron");
                    return classes;
                } catch (java.io.IOException) {
                    //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:

                    return null;
                }
            }
        }

        public virtual List<string> Layers {
            get {
                try {
                    var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == "org.neuroph.nnet.comp.layer" select t; List<string> classes = q.Select((x) => x.Name).ToList();
                    classes.Insert(0, "Layer");
                    return classes;
                } catch (java.io.IOException) {
                    //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:

                    return null;
                }
            }
        }

        public virtual List<string> LearningRules {
            get {
                try {
                    var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == "org.neuroph.nnet.learning" select t; List<string> classes = q.Select((x) => x.Name).ToList();
                    return classes;
                } catch (java.io.IOException) {
                    //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:

                    return null;
                }
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private static java.util.List<string><String> getClassNamesFromPackage(String packageName) throws java.io.IOException
        //private static List<string> getClassNamesFromPackage(string packageName)
        //{
        //	ClassLoader classLoader = Thread.CurrentThread.ContextClassLoader;
        //	URL packageURL;
        //	List<string> names = new List<string>();

        //packageName = packageName.Replace(".", "/");
        //packageURL = classLoader.getResource(packageName);

        //	if (packageURL.Protocol.Equals("jar"))
        //	{
        //		string jarFileName;
        //		JarFile jf;
        //		IEnumerator<JarEntry> jarEntries;
        //		string entryName;

        //		// build jar file name, then loop through zipped entries
        //		jarFileName = URLDecoder.decode(packageURL.File, "UTF-8");
        //		jarFileName = jarFileName.Substring(5, jarFileName.IndexOf("!", StringComparison.Ordinal) - 5);
        //		//System.out.println(">"+jarFileName);
        //		jf = new JarFile(jarFileName);
        //		jarEntries = jf.entries();
        //		while (jarEntries.MoveNext())
        //		{
        //			entryName = jarEntries.Current.Name;
        //			if (entryName.StartsWith(packageName, StringComparison.Ordinal) && entryName.Length > packageName.Length + 5)
        //			{
        //				entryName = StringHelperClass.SubstringSpecial(entryName, packageName.Length,entryName.LastIndexOf('.'));
        //				names.Add(entryName.Substring(1));
        //			}
        //		}

        //	// loop through files in classpath
        //	}
        //	else
        //	{
        //		File folder = new File(packageURL.File);
        //		File[] contenuti = folder.listFiles();
        //		string entryName;
        //		foreach (File actual in contenuti)
        //		{
        //			entryName = actual.Name;
        //			entryName = entryName.Substring(0, entryName.LastIndexOf('.'));
        //			names.Add(entryName);
        //		}
        //	}
        //	return names;
        //}

    }

}
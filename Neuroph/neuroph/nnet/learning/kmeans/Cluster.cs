using System.Collections.Generic;

namespace org.neuroph.nnet.learning.kmeans
{


	/// <summary>
	/// This class represents a single cluster, with corresponding centroid and assigned vectors
	/// @author Zoran Sevarac
	/// </summary>
	public class Cluster
	{

		/// <summary>
		/// Centroid for this cluster
		/// </summary>
		internal KVector centroid;

		/// <summary>
		/// Vectors assigned to this cluster during clustering
		/// </summary>
		internal List<KVector> vectors;


		public Cluster()
		{
			this.vectors = new List<KVector>();
		}

		public virtual KVector Centroid
		{
			get
			{
				return centroid;
			}
			set
			{
				this.centroid = value;
			}
		}



		public virtual void removePoint(KVector point)
		{
			vectors.Remove(point);
		}

		public virtual List<KVector> Points
		{
			get
			{
				return vectors;
			}
		}

		/// <summary>
		/// Calculate and return avg sum vector for all vectors </summary>
		/// <returns> avg sum vector  </returns>
		public virtual double[] AvgSum
		{
			get
			{
				double size = vectors.Count;
				double[] avg = new double[vectors[0].size()];
    
				foreach (KVector item in vectors)
				{
					double[] values = item.Values;
    
					for (int i = 0; i < values.Length; i++)
					{
						avg[i] += values[i] / size;
					}
    
				}
    
				return avg;
			}
		}


		/// <summary>
		/// Returns true if two clusters have same centroid </summary>
		/// <param name="obj"> </param>
		/// <returns>  </returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this.GetType() != obj.GetType())
			{
				return false;
			}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Cluster other = (Cluster) obj;
			Cluster other = (Cluster) obj;
			double[] otherValues = other.Centroid.Values;
			double[] values = other.Centroid.Values;
			// do this ina for loop here
			for (int i = 0; i < centroid.size(); i++)
			{
				if (otherValues[i] != values[i])
				{
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			int hash = 7;
			hash = 97 * hash + this.centroid.GetHashCode();
			hash = 97 * hash + this.vectors.GetHashCode();
			return hash;
		}




		/// <summary>
		/// Assignes vector to this cluster. </summary>
		/// <param name="vector"> vector to assign </param>
		public virtual void assignVector(KVector vector)
		{
			// if vector's cluster is allready set to this, save some cpu cycles
			if (vector.Cluster != this)
			{
				vector.Cluster = this;
				vectors.Add(vector);
			}
		}

		/// <summary>
		/// Returns number of vectors assigned to this cluster. </summary>
		/// <returns> number of vectors assigned to this cluster </returns>
		public virtual int size()
		{
			return vectors.Count;
		}



	}

}
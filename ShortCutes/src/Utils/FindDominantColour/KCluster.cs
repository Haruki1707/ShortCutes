using System;
using System.Collections.Generic;
using System.Drawing;

namespace FindDominantColour
{

    /// <summary>
    /// Describes a cluster in a K-Means Clustering set
    /// </summary>
    public class KCluster
    {
        private readonly List<Color> _colours;

        /// <summary>
        /// Creates a new K-Means Cluster set
        /// </summary>
        /// <param name="centre">The initial centre point for the set</param>
        public KCluster(Color centre)
        {
            Centre = centre;
            _colours = new List<Color>();
        }

        /// <summary>
        /// The current centre point of the cluster
        /// </summary>
        public Color Centre { get; set; }

        /// <summary>
        /// The number of points this cluster had before <seealso cref="RecalculateCentre"/> was called
        /// </summary>
        public int PriorCount { get; set; }

        /// <summary>
        /// Add <paramref name="colour"/> to the cluster. This means that the next time <seealso cref="RecalculateCentre"/> <paramref name="colour"/> will be considered in the centre calculation
        /// </summary>
        /// <param name="colour"></param>
        public void Add(Color colour)
        {
            _colours.Add(colour);
        }

        /// <summary>
        /// Based on all the items that have been <seealso cref="Add">Added</seealso> to this cluster calculates the centre.
        /// </summary>
        /// <param name="threshold">If the centre has moved by at least this value cluster has not yet converged and needs to be recalculated</param>
        /// <returns><c>true</c> if the recalculated centre's euclidean distance from the old centre is at least <paramref name="threshold"/>. <c>false</c> if it is less than this value</returns>
        public bool RecalculateCentre(double threshold = 0.0d)
        {
            Color updatedCentre;

            if (_colours.Count > 0)
            {
                float r = 0;
                float g = 0;
                float b = 0;

                foreach (Color color in _colours)
                {
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }

                updatedCentre = Color.FromArgb((int)Math.Round(r / _colours.Count), (int)Math.Round(g / _colours.Count), (int)Math.Round(b / _colours.Count));
            }
            else
            {
                updatedCentre = Color.FromArgb(0, 0, 0, 0);
            }

            double distance = EuclideanDistance(Centre, updatedCentre);
            Centre = updatedCentre;

            PriorCount = _colours.Count;
            _colours.Clear();

            return distance > threshold;
        }

        /// <summary>
        /// Returns the Euclidean distance of <paramref name="colour"/> from the current cluster centre point
        /// </summary>
        public double DistanceFromCentre(Color colour)
        {
            return EuclideanDistance(colour, Centre);
        }

        /// <summary>
        /// Calcultes the Euclidean distance between two colours, <paramref name="c1"/> and <paramref name="c2"/>
        /// </summary>
        public static double EuclideanDistance(Color c1, Color c2)
        {
            double distance = Math.Pow(c1.R - c2.R, 2) + Math.Pow(c1.G - c2.G, 2) + Math.Pow(c1.B - c2.B, 2);

            return Math.Sqrt(distance);
        }
    }
}
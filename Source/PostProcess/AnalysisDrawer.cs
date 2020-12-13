using System.Collections.Generic;
using System.Drawing;
using DataMiningSpotifyTop.Source.Common;
using ScottPlot;

namespace DataMiningSpotifyTop.Source.PostProcess
{
    public class AnalysisDrawer
    {
        #region Fields

        static readonly Color InterClusterDistanceColor = Color.DarkGoldenrod;
        static readonly Color InterClusterMeanDistanceColor = Color.Magenta;
        static readonly Color ClusterDividerColor = Color.Red;

        #endregion



        #region Properties

        List<ModelAnalyzer> Analyzers { get; }

        public List<Plot> IntraClusterPlots { get; private set; }

        public Plot IntraClusterCommonPlot { get; private set; }

        public Plot InterClusterPlot { get; private set; }

        #endregion



        #region Object lifecycle

        public AnalysisDrawer(List<ModelAnalyzer> analyzers)
        {
            Analyzers = analyzers;
            IntraClusterPlots = new List<Plot>(analyzers.Capacity);
        }

        #endregion



        #region Methods

        public void CreatePlots()
        {
            IntraClusterPlots.Clear();

            foreach (ModelAnalyzer modelAnalyzer in Analyzers)
            {
                Plot intraClusterPlot = CreateIntraClusterPlot(modelAnalyzer);
                IntraClusterPlots.Add(intraClusterPlot);
            }
        }


        Plot CreateIntraClusterPlot(ModelAnalyzer analyzer)
        {
            Plot intraClusterPlot = new Plot();

            int pointsCount = analyzer.AnalyzedSongs.Count;
            double[] xValues = new double[pointsCount];
            double[] yValues = new double[pointsCount];

            int currentClusterIndex = analyzer.AnalyzedSongs[0].ClusterIndex;

            for (int i = 0; i < pointsCount; i++)
            {
                AnalyzedSong point = analyzer.AnalyzedSongs[i];

                if (currentClusterIndex != point.ClusterIndex)
                {
                    double value = (i + (i - 1)) / 2.0;
                    intraClusterPlot.PlotVLine(value);
                }

                xValues[i] = i;
                yValues[i] = point.CentroidDistance;

                currentClusterIndex = point.ClusterIndex;
            }

            intraClusterPlot.PlotScatter(xValues, yValues, InterClusterDistanceColor);
            intraClusterPlot.SaveFig("test_plot");

            return intraClusterPlot;
        }

        #endregion
    }
}

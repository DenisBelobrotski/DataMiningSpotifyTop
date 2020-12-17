using System;
using System.Collections.Generic;
using System.Drawing;
using DataMiningSpotifyTop.Source.Common;
using ScottPlot;

namespace DataMiningSpotifyTop.Source.PostProcess
{
    public class AnalysisDrawer
    {
        #region Fields

        static readonly Color IntraClusterDistanceColor = Color.DarkGoldenrod;
        static readonly Color VerticalDividerColor = Color.Red;
        static readonly Color IntraClustersMeanDistanceColor = Color.Magenta;
        static readonly Color AxisColor = Color.Green;

        #endregion



        #region Properties

        List<ModelAnalyzer> Analyzers { get; }

        public List<Plot> IntraClusterPlots { get; private set; }

        public Plot IntraClusterCommonPlot { get; private set; }

        public Plot InterClusterPlot { get; private set; }
        
        public List<Plot> SilhouetteCoefPlots { get; private set; }
        
        public Plot SilhouetteCoefMeanPlot { get; private set; }

        #endregion



        #region Object lifecycle

        public AnalysisDrawer(List<ModelAnalyzer> analyzers)
        {
            Analyzers = analyzers;
            IntraClusterPlots = new List<Plot>(analyzers.Capacity);
            SilhouetteCoefPlots = new List<Plot>(analyzers.Capacity);
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
                
                Plot silhouettePlot = CreateSilhouettePlot(modelAnalyzer);
                SilhouetteCoefPlots.Add(silhouettePlot);
            }

            List<ModelAnalyzer> sortedAnalyzers = new List<ModelAnalyzer>(Analyzers);
            sortedAnalyzers.Sort((left, right) => left.ClustersCount.CompareTo(right.ClustersCount));

            IntraClusterCommonPlot = CreateIntraClusterCommonPlot(sortedAnalyzers);
            InterClusterPlot = CreateInterClusterPlot(sortedAnalyzers);

            SilhouetteCoefMeanPlot = CreateSilhouetteMeanPlot(Analyzers);
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

                xValues[i] = i;
                yValues[i] = point.CentroidDistance;

                if (currentClusterIndex != point.ClusterIndex)
                {
                    double value = (i + (i - 1)) / 2.0;
                    string label = $"cluster {currentClusterIndex}";
                    intraClusterPlot.PlotVLine(value, VerticalDividerColor, label: label);
                }

                currentClusterIndex = point.ClusterIndex;
            }

            intraClusterPlot.PlotScatter(xValues, yValues, IntraClusterDistanceColor);
            intraClusterPlot.PlotHLine(analyzer.IntraClusterMeanDistance, IntraClustersMeanDistanceColor);

            return intraClusterPlot;
        }


        Plot CreateIntraClusterCommonPlot(List<ModelAnalyzer> analyzers)
        {
            if (analyzers.Count < 1)
            {
                throw new ArgumentException();
            }
            
            Plot intraClusterCommonPlot = new Plot();

            int pointsCount = analyzers.Count;
            double[] xValues = new double[pointsCount];
            double[] yValues = new double[pointsCount];

            int currentClustersCount = analyzers[0].ClustersCount;

            for (int i = 0; i < pointsCount; i++)
            {
                ModelAnalyzer analyzer = analyzers[i];

                xValues[i] = i;
                yValues[i] = analyzer.IntraClusterMeanDistance;

                if (currentClustersCount != analyzer.ClustersCount)
                {
                    double value = (i + (i - 1)) / 2.0;
                    string label = $"clusters count {currentClustersCount}";
                    intraClusterCommonPlot.PlotVLine(value, VerticalDividerColor, label: label);
                }

                currentClustersCount = analyzer.ClustersCount;
            }
            
            intraClusterCommonPlot.PlotScatter(xValues, yValues, IntraClusterDistanceColor);

            return intraClusterCommonPlot;
        }


        Plot CreateInterClusterPlot(List<ModelAnalyzer> analyzers)
        {
            Plot interClusterPlot = new Plot();

            int pointsCount = analyzers.Count;
            double[] xValues = new double[pointsCount];
            double[] yValues = new double[pointsCount];

            int currentClustersCount = analyzers[0].ClustersCount;

            for (int i = 0; i < pointsCount; i++)
            {
                ModelAnalyzer analyzer = analyzers[i];

                xValues[i] = i;
                yValues[i] = analyzer.InterClusterMeanDistance;

                if (currentClustersCount != analyzer.ClustersCount)
                {
                    double value = (i + (i - 1)) / 2.0;
                    string label = $"clusters count {currentClustersCount}";
                    interClusterPlot.PlotVLine(value, VerticalDividerColor, label: label);
                }

                currentClustersCount = analyzer.ClustersCount;
            }
            
            interClusterPlot.PlotScatter(xValues, yValues, IntraClusterDistanceColor);

            return interClusterPlot;
        }


        Plot CreateSilhouettePlot(ModelAnalyzer analyzer)
        {
            Plot silhouettePlot = new Plot();

            int pointsCount = analyzer.AnalyzedSongs.Count;
            double[] xValues = new double[pointsCount];
            double[] yValues = new double[pointsCount];

            int currentClusterIndex = analyzer.AnalyzedSongs[0].ClusterIndex;

            for (int i = 0; i < pointsCount; i++)
            {
                AnalyzedSong point = analyzer.AnalyzedSongs[i];

                xValues[i] = i;
                yValues[i] = point.SilhouetteCoef;

                if (currentClusterIndex != point.ClusterIndex)
                {
                    double value = (i + (i - 1)) / 2.0;
                    string label = $"cluster {currentClusterIndex}";
                    silhouettePlot.PlotVLine(value, VerticalDividerColor, label: label);
                }

                currentClusterIndex = point.ClusterIndex;
            }

            silhouettePlot.PlotScatter(xValues, yValues, IntraClusterDistanceColor);
            silhouettePlot.PlotHLine(analyzer.SilhouetteCoefMean, IntraClustersMeanDistanceColor);
            silhouettePlot.PlotHLine(0.0, AxisColor);

            return silhouettePlot;
        }


        Plot CreateSilhouetteMeanPlot(List<ModelAnalyzer> analyzers)
        {
            Plot silhouetteMeanPlot = new Plot();

            int pointsCount = analyzers.Count;
            double[] xValues = new double[pointsCount];
            double[] yValues = new double[pointsCount];

            int currentClustersCount = analyzers[0].ClustersCount;

            for (int i = 0; i < pointsCount; i++)
            {
                ModelAnalyzer analyzer = analyzers[i];

                xValues[i] = i;
                yValues[i] = analyzer.SilhouetteCoefMean;

                if (currentClustersCount != analyzer.ClustersCount)
                {
                    double value = (i + (i - 1)) / 2.0;
                    string label = $"clusters count {currentClustersCount}";
                    silhouetteMeanPlot.PlotVLine(value, VerticalDividerColor, label: label);
                }

                currentClustersCount = analyzer.ClustersCount;
            }
            
            silhouetteMeanPlot.PlotScatter(xValues, yValues, IntraClusterDistanceColor);

            return silhouetteMeanPlot;
        }

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MSChart = System.Windows.Forms.DataVisualization.Charting;
using EZAPI.Toolbox.DataStructures;

namespace EZAPI.Framework.Chart.Indicators
{
    public class EaseOfMovement : Indicator
    {
        public EaseOfMovement(string name = "Ease Of Movement", string description = "", string moreInfoWebPage = "")
            : base(name)
        {
            this.description = description;
            this.moreInfoWebPage = moreInfoWebPage;
            this.indicatorType = IndicatorChartArea.BottomChart;

            // If we are passed a blank description or web page, then use our default values.
            // If 'null' is passed to the constructor for either of those values, they stay null (in case the user of this class doesn't want them displayed).
            if (description == "")
                this.description = "The ease of movement formula uses the close price and volume to measure the strength of the price trend. A value close to zero indicates that prices are not moving easily, while a high positive value indicates that prices are going up easily and a high negative value indicates that prices are going down easily.";
            if (moreInfoWebPage == "")
                this.moreInfoWebPage = "http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:ease_of_movement";

            // ADD PARAMETERS HERE

            this["Color"] = Color.Navy;
            this["Line Style"] = zChartLineStyle.Solid;
        }

        /// <summary>
        /// Override the Draw method to display the indicator.
        /// </summary>
        public override void Draw(MSChart.Chart chart1, bool enable = true)
        {
            MSChart.ChartDashStyle lineStyle = Parse.ParseEnum<MSChart.ChartDashStyle>(parameters["Line Style"].Value.ToString());
            Color indicatorColor = (Color)parameters["Color"].Value;

            string INDICATOR = "EaseOfMovement";

            if (chart1.Series.IndexOf(INDICATOR) == -1)
            {
                chart1.Series.Add(INDICATOR);
                chart1.Series[INDICATOR].IsXValueIndexed = true;
                chart1.Series[INDICATOR].ChartType = MSChart.SeriesChartType.Line;
            }
            else
            {
                chart1.Series[INDICATOR].Points.Clear();
            }
            chart1.Series[INDICATOR].BorderWidth = 2;
            chart1.Series[INDICATOR].BorderDashStyle = lineStyle;
            chart1.Series[INDICATOR].Color = indicatorColor;

            chart1.Series[INDICATOR].ChartArea = "ChartAreaBottom";

            if (enable == true) EnableOneBottomSeries(chart1, INDICATOR);

            chart1.DataManipulator.IsStartFromFirst = true;
            // OPEN=candlestickSeries:Y HIGH=candlestickSeries:Y1 LOW=candlestickSeries:Y2 CLOSE=candlestickSeries:Y3 VOLUME=candlestickVolumeSeries:Y
            // Inputs to this indicator: High, Low, Volume
            chart1.DataManipulator.FinancialFormula(MSChart.FinancialFormula.EaseOfMovement, "candlestickSeries:Y1,candlestickSeries:Y2,candlestickVolumeSeries:Y", INDICATOR + ":Y");

            AlignChartData(chart1.Series["candlestickSeries"], chart1.Series[INDICATOR]);

            RescaleAxes(chart1);
        }

    } // class
} // namespace
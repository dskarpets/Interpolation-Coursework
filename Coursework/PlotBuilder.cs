using System;
using AppKit;
using NCalc;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Mac;

namespace Coursework
{
	public class PlotBuilder
	{
        public static void PlotFunction(string function, PlotView plotView)
        {
            var model = new PlotModel { Title = "Function Plot" };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y" });

            var series = new LineSeries();

            function = PrepareFunction(function);

            Expression expression = new Expression(function, EvaluateOptions.IgnoreCase);
            expression.EvaluateFunction += (name, args) =>
            {
                if (name == "pow")
                {
                    double baseValue = Convert.ToDouble(args.Parameters[0].Evaluate());
                    double exponent = Convert.ToDouble(args.Parameters[1].Evaluate());
                    args.Result = Math.Pow(baseValue, exponent);
                }
            };

            try
            {
                for (double x = -1000; x <= 1000; x += 0.1)
                {
                    expression.Parameters["x"] = x;
                    double y = Convert.ToDouble(expression.Evaluate());
                    series.Points.Add(new DataPoint(x, y));
                }
            }
            catch (Exception ex)
            {
                var alert = new NSAlert
                {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = $"Error evaluating function: {ex.Message}",
                    MessageText = "Error"
                };
                alert.RunModal();
                return;
            }

            model.Series.Add(series);
            plotView.Model = model;
        }

        private static string PrepareFunction(string function)
        {
            while (function.Contains("^"))
            {
                int index = function.IndexOf('^');
                int start = index - 1;
                int end = index + 1;

                while (start >= 0 && (char.IsLetterOrDigit(function[start]) || function[start] == '.'))
                    start--;
                while (end < function.Length && (char.IsLetterOrDigit(function[end]) || function[end] == '.'))
                    end++;

                string baseValue = function.Substring(start + 1, index - start - 1);
                string exponent = function.Substring(index + 1, end - index - 1);

                string newExpression = $"pow({baseValue},{exponent})";
                function = function.Substring(0, start + 1) + newExpression + function.Substring(end);
            }

            return function;
        }
    }
}
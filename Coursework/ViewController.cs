using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using Foundation;
using CoreGraphics;
using OxyPlot;
using OxyPlot.Xamarin.Mac;
using System.Linq;

namespace Coursework
{
	public partial class ViewController : NSViewController
	{
        private PlotView plotView;
        private NSButton plotButton;
        private NSTextField functionInput;

        private List<Tuple<double, double>> points = new List<Tuple<double, double>>();
        private List<Tuple<double, double>> pointsToWrite = new List<Tuple<double, double>>();

        private NSTextField[] xFields;
        private NSTextField[] yFields;

        public ViewController (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            xFields = new NSTextField[] { x1, x2, x3, x4, x5, x6, x7, x8, x9, x10 };
            yFields = new NSTextField[] { y1, y2, y3, y4, y5, y6, y7, y8, y9, y10 };

            plotView = new PlotView
            {
                Frame = new CGRect(266, 219, 430, 340)
            };
            View.AddSubview(plotView);

            var emptyModel = new PlotModel { Title = "Empty Plot" };
            plotView.Model = emptyModel;

            UpdateTextFieldVisibility(2);

            systemSize.Activated += SystemSizeChanged;

        }

        partial void calculateButton(NSObject sender)
        {
            var newPoints = new List<Tuple<double, double>>();

            for (int i = 0; i < systemSize.IndexOfSelectedItem + 2; i++)
            {
                if (!double.TryParse(GetXField(i).StringValue, out double x) || !double.TryParse(GetYField(i).StringValue, out double y))
                {
                    ShowAlert("Помилка", "Введіть коректні числові значення.");
                    return;
                }

                if (x < -1000 || x > 1000 || y < -1000 || y > 1000)
                {
                    ShowAlert("Помилка", "Координати точок повинні бути у межах від -1000 до 1000.");
                    return;
                }

                x = Math.Round(x, 5);
                y = Math.Round(y, 5);

                if (newPoints.Any(point => point.Item1 == x) || points.Any(point => point.Item1 == x))
                {
                    ShowAlert("Помилка", $"Точка з X = {x} вже існує.");
                    return;
                }

                if (newPoints.Any(point => point.Item1 == x && point.Item2 != y) || points.Any(point => point.Item1 == x && point.Item2 != y))
                {
                    ShowAlert("Помилка", $"Точка з X = {x} вже існує з іншим значенням Y.");
                    return;
                }

                newPoints.Add(new Tuple<double, double>(x, y));
            }

            points.AddRange(newPoints);
            pointsToWrite.Clear();
            pointsToWrite.AddRange(newPoints);

            Tuple<string, string> tuple = PerformInterpolation();

            polynomialTextField.StringValue = tuple.Item1;
            practicalComplexityTextField.StringValue = tuple.Item2;

            PlotBuilder.PlotFunction(tuple.Item1, plotView);

            foreach (var point in points)
            {
                Console.WriteLine($"X: {point.Item1}, Y: {point.Item2}");
            }

            points.Clear();

        }

        partial void generateSystemButton(NSObject sender)
        {
            var random = new Random();
            var number = (int)systemSize.IndexOfSelectedItem + 2;
            var newPoints = new List<Tuple<double, double>>();

            points.Clear();

            for (int i = 0; i < number; i++)
            {
                int x = random.Next(-1000, 1000);
                double y = Math.Round(random.NextDouble() * 2000 - 1000, 3);

                newPoints.Add(new Tuple<double, double>(x, y));
            }

            UpdateTextFieldVisibility(number);

            for (int i = 0; i < number; i++)
            {
                GetXField(i).StringValue = newPoints[i].Item1.ToString();
                GetYField(i).StringValue = newPoints[i].Item2.ToString();
            }
        }

        private Tuple<string, string> PerformInterpolation()
        {
            
            string selectedMethod = interpolationMethod.TitleOfSelectedItem;

            if (selectedMethod == "Lagrange")
            {
                var result = LagrangeInterpolation.CalculateInterpolation(points);
                var complexity = LagrangeInterpolation.iterations.ToString();
                LagrangeInterpolation.iterations = 0;
                return new Tuple<string, string>(result, complexity);
            }
            else
            {
                var result = AitkenInterpolation.CalculateInterpolation(points);
                var complexity = AitkenInterpolation.iterations.ToString();
                AitkenInterpolation.iterations = 0;
                return new Tuple<string, string>(result, complexity);
            }

        }

        partial void clearFieldsButton(NSObject sender)
        {
            foreach (var field in xFields)
            {
                field.StringValue = "";
            }

            foreach (var field in yFields)
            {
                field.StringValue = "";
            }

            polynomialTextField.StringValue = "";
            practicalComplexityTextField.StringValue = "";
            points.Clear();
            pointsToWrite.Clear();
            plotView.Model.Series.Clear();

        }

        partial void saveToFileButton(NSObject sender)
        {
            var savePanel = NSSavePanel.SavePanel;
            savePanel.Title = "Зберегти дані";
            savePanel.AllowedFileTypes = new string[] { "txt" };

            if (savePanel.RunModal() == 1)
            {
                string filePath = savePanel.Url.Path;

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("Точки:");
                        foreach (var point in pointsToWrite)
                        {
                            writer.WriteLine(point);
                        }

                        writer.WriteLine();
                        writer.WriteLine("Поліном:");
                        writer.WriteLine(polynomialTextField.StringValue);
                    }

                    ShowAlert("Успіх", "Дані було успішно збережено у файл.");
                }
                catch (Exception ex)
                {
                    ShowAlert("Помилка", $"Не вдалося зберегти дані у файл: {ex.Message}");
                }
            }
        }

        private void SystemSizeChanged(object sender, EventArgs e)
        {
            var selectedSize = (int)systemSize.IndexOfSelectedItem + 2;

            UpdateTextFieldVisibility(selectedSize);
        }

        private void UpdateTextFieldVisibility(int selectedSize)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < selectedSize)
                {
                    xFields[i].Hidden = false;
                    yFields[i].Hidden = false;
                }
                else
                {
                    xFields[i].Hidden = true;
                    yFields[i].Hidden = true;
                }
            }
        }

        private void ShowAlert(string title, string message)
        {
            var alert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Critical,
                InformativeText = message,
                MessageText = title
            };
            alert.RunModal();
        }

        private NSTextField GetXField(int index)
        {
            switch (index)
            {
                case 0: return x1;
                case 1: return x2;
                case 2: return x3;
                case 3: return x4;
                case 4: return x5;
                case 5: return x6;
                case 6: return x7;
                case 7: return x8;
                case 8: return x9;
                case 9: return x10;
                default: throw new IndexOutOfRangeException("Index out of range for X field.");
            }
        }

        private NSTextField GetYField(int index)
        {
            switch (index)
            {
                case 0: return y1;
                case 1: return y2;
                case 2: return y3;
                case 3: return y4;
                case 4: return y5;
                case 5: return y6;
                case 6: return y7;
                case 7: return y8;
                case 8: return y9;
                case 9: return y10;
                default: throw new IndexOutOfRangeException("Index out of range for Y field.");
            }
        }

        public override NSObject RepresentedObject
        {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
			}
		}

	}
}
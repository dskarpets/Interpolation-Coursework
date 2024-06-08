using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;

namespace Coursework
{
    public class LagrangeInterpolation
    {
        public static int iterations = 0;

        public static string CalculateInterpolation(List<Tuple<double, double>> points)
        {
            var x = Expr.Symbol("x");
            var polynomial = Expr.Zero;
            const double tolerance = 1e-10;

            for (int i = 0; i < points.Count; i++)
            {
                var term = Expr.One;
                for (int j = 0; j < points.Count; j++)
                {
                    if (i != j)
                    {
                        term *= (x - Expr.Real(points[j].Item1)) / (Expr.Real(points[i].Item1) - Expr.Real(points[j].Item1));
                    }
                    iterations++;
                }
                polynomial += term * Expr.Real(points[i].Item2);
            }

            var simplifiedPolynomial = Algebraic.Expand(polynomial);

            var polynomialString = Infix.Format(simplifiedPolynomial);

            var filteredPolynomialString = FilterAndRoundCoefficients(polynomialString, tolerance);

            if (points.All(point => point.Item2 == 0))
            {
                return "0";
            }

            return filteredPolynomialString;
        }

        private static string FilterAndRoundCoefficients(string polynomial, double tolerance)
        {
            var terms = polynomial.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var filteredTerms = new List<string>();
            bool isNextTermNegative = false;

            foreach (var term in terms)
            {
                if (term == "+")
                {
                    isNextTermNegative = false;
                    continue;
                }
                if (term == "-")
                {
                    isNextTermNegative = true;
                    continue;
                }

                var processedTerm = term;
                if (isNextTermNegative)
                {
                    processedTerm = "-" + term;
                }

                if (processedTerm.Contains("x"))
                {
                    var parts = processedTerm.Split('*');
                    bool includeTerm = true;
                    var roundedParts = new List<string>();
                    foreach (var part in parts)
                    {
                        if (double.TryParse(part, NumberStyles.Any, CultureInfo.InvariantCulture, out double coefficient))
                        {
                            if (Math.Abs(coefficient) < tolerance)
                            {
                                includeTerm = false;
                                break;
                            }
                            roundedParts.Add(Math.Round(coefficient, 5).ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            roundedParts.Add(part);
                        }
                    }
                    if (includeTerm)
                    {
                        filteredTerms.Add(string.Join("*", roundedParts));
                    }
                }
                else
                {
                    if (double.TryParse(processedTerm, NumberStyles.Any, CultureInfo.InvariantCulture, out double constant))
                    {
                        if (Math.Abs(constant) >= tolerance)
                        {
                            filteredTerms.Add(Math.Round(constant, 5).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }

            return string.Join(" + ", filteredTerms).Replace("+ -", " - ");
        }

    }
}
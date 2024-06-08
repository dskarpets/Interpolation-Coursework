using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;

namespace Coursework
{
    public class AitkenInterpolation
    {
        public static int iterations = 0;

        public static string CalculateInterpolation(List<Tuple<double, double>> points)
        {
            var x = Expr.Symbol("x");
            int n = points.Count;
            var P = new Dictionary<string, Expr>();
            const double tolerance = 1e-10;

            for (int i = 0; i < n; i++)
            {
                P[$"{i},{i}"] = Expr.Real(points[i].Item2);
            }

            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < n - j; i++)
                {
                    double xi = points[i].Item1;
                    double xj = points[i + j].Item1;

                    var P0 = P[$"{i},{i + j - 1}"];
                    var P1 = P[$"{i + 1},{i + j}"];

                    var determinant = ((x - Expr.Real(xi)) * P1 - (x - Expr.Real(xj)) * P0) / Expr.Real(xj - xi);

                    iterations++;

                    P[$"{i},{i + j}"] = determinant;
                }
            }

            var polynomial = P[$"0,{n - 1}"];

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

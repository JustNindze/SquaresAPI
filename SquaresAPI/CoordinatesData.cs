using SquaresAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI
{
    public static class CoordinatesData
    {
        //HashSet naudojamas, nes nera prasmes saugoti, dvieju vienodu tasku
        private static HashSet<CartesianCoordinates> Coordinates;

        public static void FillCoordinates()
        {
            Coordinates = new HashSet<CartesianCoordinates>();
        }

        //Jei taskas yra HashSet'e, metodas grazina false reiksme
        public static bool AddPoint(CartesianCoordinates point)
        {
            return Coordinates.Add(point);
        }

        //Jei tasku nera HashSet'e, grazina null, kitu atveju, grazina jau esanciu tasku HashSet'a
        public static HashSet<CartesianCoordinates> AddPoints(HashSet<CartesianCoordinates> points)
        {
            var pointsInHashSet = new HashSet<CartesianCoordinates>();
            foreach (var point in points)
            {
                var pointIsInHashSet = !AddPoint(point);
                if (pointIsInHashSet)
                {
                    pointsInHashSet.Add(point);
                }
            }

            if (pointsInHashSet.Count != 0)
            {
                return pointsInHashSet;
            }
            else
            {
                return null;
            }
        }

        //Jei tasko nera HashSet'e, metodas grazina false reiksme
        public static bool RemovePoint(CartesianCoordinates point)
        {
            return Coordinates.Remove(point);
        }

        //Jei taskai yra HashSet'e, grazina null, kitu atveju, grazina neegzistuojanciu tasku HashSet'a
        public static HashSet<CartesianCoordinates> RemovePoints(HashSet<CartesianCoordinates> points)
        {
            var pointsNotInHashSet = new HashSet<CartesianCoordinates>();
            foreach (var point in points)
            {
                var pointIsNotInHashSet = !RemovePoint(point);
                if (pointIsNotInHashSet)
                {
                    pointsNotInHashSet.Add(point);
                }
            }

            if (pointsNotInHashSet.Count != 0)
            {
                return pointsNotInHashSet;
            }
            else
            {
                return null;
            }
        }

        public static List<HashSet<CartesianCoordinates>> GetSquares()
        {
            var squares = new List<HashSet<CartesianCoordinates>>();

            foreach (var pointA in Coordinates)
            {
                foreach(var pointB in Coordinates)
                {
                    if (pointA.X == pointB.X && pointA.Y == pointB.Y)
                    {
                        continue;
                    }

                    CartesianCoordinates braVector = new CartesianCoordinates 
                    { 
                        X = pointB.X - pointA.X,
                        Y = pointB.Y - pointA.Y
                    };

                    var ketVectors = GetNextVectors(braVector);

                    var pointC1 = new CartesianCoordinates 
                    {
                        X = pointB.X + ketVectors[0].X,
                        Y = pointB.Y + ketVectors[0].Y
                    };

                    var pointC2 = new CartesianCoordinates
                    {
                        X = pointB.X + ketVectors[1].X,
                        Y = pointB.Y + ketVectors[1].Y
                    };

                    CartesianCoordinates braVector2 = new CartesianCoordinates
                    {
                        X = -braVector.X,
                        Y = -braVector.Y
                    };

                    CartesianCoordinates? pointD1 = null;
                    CartesianCoordinates? pointD2 = null;

                    if (Coordinates.Contains(pointC1))
                    {
                        pointD1 = new CartesianCoordinates
                        {
                            X = pointC1.X + braVector2.X,
                            Y = pointC1.Y + braVector2.Y
                        };
                    }

                    if (Coordinates.Contains(pointC2))
                    {
                        pointD2 = new CartesianCoordinates
                        {
                            X = pointC2.X + braVector2.X,
                            Y = pointC2.Y + braVector2.Y
                        };
                    }

                    if (pointD1 == null && pointD2 == null)
                    {
                        continue;
                    }

                    if (pointD1 != null)
                    {
                        if (Coordinates.Contains((CartesianCoordinates)pointD1))
                        {
                            squares.Add(new HashSet<CartesianCoordinates>
                            {
                                pointA,
                                pointB,
                                pointC1,
                                (CartesianCoordinates)pointD1
                            });
                        }
                    }

                    if (pointD2 != null)
                    {
                        if (Coordinates.Contains((CartesianCoordinates)pointD2))
                        {
                            squares.Add(new HashSet<CartesianCoordinates>
                            {
                                pointA,
                                pointB,
                                pointC2,
                                (CartesianCoordinates)pointD2
                            });
                        }
                    }
                }
            }

            for (int n = 0; n < 2; n++)
            {
                for (int i = 0; i < squares.Count; i++)
                {
                    var squareA = squares[i];

                    for (int j = 0; j < squares.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        var squareB = squares[j];

                        if (squareA.SetEquals(squareB))
                        {
                            squares.Remove(squareB);
                        }
                    }
                }
            }

            return squares;
        }

        private static List<CartesianCoordinates> GetNextVectors(CartesianCoordinates braVector)
        {
            var vectors = new List<CartesianCoordinates>();

            if (braVector.X == 0)
            {
                vectors.Add(new CartesianCoordinates
                {
                    X = braVector.Y,
                    Y = 0
                }); ;

                vectors.Add(new CartesianCoordinates
                {
                    X = -braVector.Y,
                    Y = 0
                }); ;

                return vectors;
            }

            double braVectorLengthSquared = Math.Pow(braVector.X, 2) + Math.Pow(braVector.Y, 2);
            double k = -braVector.Y / (double)braVector.X;
            double y = Math.Sqrt(braVectorLengthSquared / (Math.Pow(k, 2) + 1));
            double y1 = -y;
            double y2 = y;
            double x1 = k * y1;
            double x2 = k * y2;

            vectors.Add(new CartesianCoordinates
            {
                X = (int)Math.Ceiling(x1),
                Y = (int)Math.Ceiling(y1)
            });

            vectors.Add(new CartesianCoordinates
            {
                X = (int)Math.Ceiling(x2),
                Y = (int)Math.Ceiling(y2)
            });

            return vectors;
        }
    }
}

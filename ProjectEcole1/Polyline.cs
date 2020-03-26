using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing;
using MathUtilLibrary;
namespace ProjectLibraryClass
{
    [Serializable]
    public class Polyline : CartoObj, IIsPointClose, IPointy, IComparable<Polyline>, ICartoObj
    {
        private List<ICoord> lCoordonnees;
        private Color color;
        private double opacite;
        private double epaisseur;
        public Polyline() : base()
        {
            lCoordonnees = new List<ICoord>();
            color = Color.White;
            epaisseur = 1;
            opacite = 0.5;
        }
        public Polyline(double ep, Color col,double op, List<ICoord> coord) : this()
        {
            // c = new Collection<Coordonnees>();
            for (int i = 0; i < coord.Count; i++)
                lCoordonnees.Add(coord[i]);

            epaisseur = ep;
            color = col;
            opacite = op;

        }
        public Polyline(Polyline poly) : this()
        {
            for (int i = 0; i < poly.lCoord.Count; i++)
            {
                if (poly.lCoord[i] is POI)
                    lCoordonnees.Add(new POI(poly.lCoord[i].Description, poly.lCoord[i].X, poly.lCoord[i].Y));
                else
                    lCoordonnees.Add(new Coordonnees(poly.lCoord[i].X, poly.lCoord[i].Y));

            }
            opacite = poly.Opacite;
            epaisseur = poly.Epaisseur;
            color = poly.Couleur;
        }
        ~Polyline()
        {
            for (int i = 0; i < lCoordonnees.Count; i++)
            {
                lCoordonnees.RemoveAt(i);
            }
        }
        public double Epaisseur
        {
            get { return epaisseur; }
            set { epaisseur = value; }
        }
        public Color Couleur
        {
            get { return color; }
            set { color = value; }
        }
        public void AddCoord(Coordonnees coord)
        {
            lCoordonnees.Add(coord);

        }
        public override int nbPoint
        {
            get { return lCoordonnees.Count; }
        }
        public override string ToString()
        {
            string tmp = "---------------------------------------------" + Environment.NewLine;
            tmp += "Polyline " + base.ToString() + Environment.NewLine + " " + "Epaisseur = " + epaisseur + " Couleur = " + color.ToString() + Environment.NewLine;
            for (int i = 0; i < lCoordonnees.Count(); i++)
            {
                tmp += lCoordonnees[i].ToString();
                tmp += Environment.NewLine;
            }
            tmp += "---------------------------------------------" + Environment.NewLine;
            return tmp;
        }
        public override void Draw()
        {
            base.Draw();
            this.ToString();

        }

        public bool IsPointClose(double x, double y, double precision)
        {
            double distance;
            /// Normalement un polyline est composé d'au moins une ligne (même si on parlera plus d'une line que d'un polyline)
            if (lCoordonnees.Count < 2)
                return false;
            for (int i = 0; i < lCoordonnees.Count - 1; i++)
            {
                ///Distance premier point du segment
                distance = MathUtil.DistanceDeuxPoints(x, y, lCoordonnees[i].X, lCoordonnees[i].Y);
                if (distance <= precision)
                    return true;
                ///Distance deuxieme point du segment
                distance = MathUtil.DistanceDeuxPoints(x, y, lCoordonnees[i + 1].X, lCoordonnees[i + 1].Y);
                if (distance <= precision)
                    return true;
                ///Distance points segment
                ///Calcul du coefficient directeut (y2 -y1)/(x2-x1)
                distance = MathUtil.DistancePointSegment(x, y, lCoordonnees[i].X, lCoordonnees[i].Y, lCoordonnees[i + 1].X, lCoordonnees[i + 1].Y);
                if (distance <= precision)
                    return true;

            }
            return false;

        }
        public ICoord WhichPointIsClose(double x, double y)
        {
            ICoord obj = lCoordonnees[0];
            double distancemin = -1; ;
            double[] distancetmp = new double[2];
            double[] distancemintmp = new double[2];
            for (int i = 0; i < lCoordonnees.Count - 1; i++)
            {
                ///Distance premier point du segment
                distancetmp[0] = MathUtil.DistanceDeuxPoints(x, y, lCoordonnees[i].X, lCoordonnees[i].Y);
                ///Distance deuxieme point du segment
                distancetmp[1] = MathUtil.DistanceDeuxPoints(x, y, lCoordonnees[i + 1].X, lCoordonnees[i + 1].Y);

                ///Distance points segment
                ///Calcul du coefficient directeut (y2 -y1)/(x2-x1)
                distancemintmp = GetDistanceMin(distancemin, distancetmp);
                if (distancemintmp[1] != -1)
                {
                    distancemin = distancemintmp[0];
                    switch (distancemintmp[1])
                    {
                        case 0:
                            obj = lCoord[i];
                            break;
                        case 1:
                            obj = lCoord[i + 1];
                            break;
                    }

                }
            }

            return obj;
        }
        public double[] GetDistanceMin(double distancemin, double[] distancetmp)
        {
            double[] distPos = new double[2];
            distPos[0] = distancemin;
            distPos[1] = -1;
            if (distancemin == -1)
            {
                distPos[0] = distancetmp[0];
                distPos[1] = 0;
            }

            for (int i = 0; i < distancetmp.Length; i++)
            {
                if (distancetmp[i] < distPos[0])
                {
                    distPos[0] = distancetmp[i];
                    distPos[1] = i;
                }
            }
            return distPos;
        }
        ///Implémentation IComparable<Polyline>
        public int CompareTo(Polyline poly)
        {
            if (poly == null) return 1;

            return LongueurPolyline().CompareTo(poly.LongueurPolyline());

        }

        public double LongueurPolyline()
        {
            double distance = 0;
            int i = 0;
            while (i < lCoordonnees.Count - 1)
            {
                distance += MathUtil.DistanceDeuxPoints(lCoordonnees[i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[++i].Y);
            }
            return distance;
        }

        public List<ICoord> lCoord
        {
            get { return lCoordonnees; }
            set { lCoordonnees = value; }
        }
        public bool SurfaceSame(Polyline poly)
        {
            if (CalculSurface() == poly.CalculSurface())
                return true;
            else
                return false;
        }
        public double CalculSurface()
        {
            Coordonnees[] bBox = new Coordonnees[4]
            {

                new Coordonnees(lCoord.Min(Coordonnees=> Coordonnees.X),lCoord.Max(Coordonnees=> Coordonnees.Y)),       ///(xMin,yMax)
                new Coordonnees(lCoord.Min(Coordonnees=> Coordonnees.X),lCoord.Min(Coordonnees=> Coordonnees.Y)),       ///(xMin,yMin)
                new Coordonnees(lCoord.Max(Coordonnees=> Coordonnees.X),lCoord.Min(Coordonnees=> Coordonnees.Y)),       ///(xMax,yMin)
                new Coordonnees(lCoord.Max(Coordonnees=> Coordonnees.X),lCoord.Max(Coordonnees=> Coordonnees.Y)),       ///(xMax,yMax)

            };

            return Math.Abs(MathUtil.DistanceDeuxPoints(bBox[0].X, bBox[0].Y, bBox[1].X, bBox[1].Y) * MathUtil.DistanceDeuxPoints(bBox[1].X, bBox[1].Y, bBox[2].X, bBox[2].Y));
        }

        public double Opacite
        {
            get { return opacite; }
            set { opacite = value; }
        }

    }
}

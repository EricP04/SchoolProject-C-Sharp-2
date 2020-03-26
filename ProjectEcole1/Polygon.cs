using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing;
using MathUtilLibrary;

/**
 * Créer une classe Polygon qui hérite de CartoObj et décrite par
• Une collection de coordonnées (références d’objets
Coordonnees),
• Une couleur de remplissage,
• Une couleur de contour,
• Un niveau d’opacité (double compris entre 0 et 1)
• Un constructeur par défaut
• Un constructeur d’initialisation
• La surcharge de la méthode ToString()
• La redéfinition de la méthode Draw() qui affiche les
informations concernant l’objet polygon dans la console
• TESTER LA CLASSE*/
namespace ProjectLibraryClass
{
    [Serializable]
   public  class Polygon : CartoObj,IIsPointClose,IPointy, ICartoObj
    {
        private List<ICoord> lCoordonnees;
        private Color remplissage;
        private Color contour;
        private double opacite;
        private double epaisseur;
        public Polygon() : base()
        {
            
               lCoordonnees = new List<ICoord>();

            remplissage = Color.Green;
            contour = Color.Red;
            opacite = 0.5;
            epaisseur = 0;
        }
        public Polygon(List<ICoord> co, Color rempli, Color cont, double op, double ep) : this()
        {
            for(int i=0;i<co.Count;i++)
            {
                lCoordonnees.Add(co[i]);
            }
            remplissage = rempli;
            contour = cont;
            opacite = op;
            epaisseur = ep;
        }
        public Polygon(Polygon poly) : this()
        {
            for (int i = 0; i < poly.lCoord.Count; i++)
            {
                if (poly.lCoord[i] is POI)
                    lCoordonnees.Add(new POI(poly.lCoord[i].Description, poly.lCoord[i].X, poly.lCoord[i].Y));
                else
                    lCoordonnees.Add(new Coordonnees(poly.lCoord[i].X, poly.lCoord[i].Y));

            }
            remplissage = poly.Rempli;
            contour = poly.Contour;
            opacite = poly.Opacite;
            epaisseur = poly.Epaisseur;
        }
        public Color Rempli
        {
            get { return remplissage; }
            set { remplissage = value; }
        }
        public Color Contour
        {
            get { return contour; }
            set { contour = value; }
        }
        public double Opacite
        {
            get { return opacite; }
            set { opacite = value; }
        }
        public double Epaisseur
        {
            get { return epaisseur; }
            set { epaisseur = value; }
        }
        public override string ToString()
        {  
            string tmp = "---------------------------------------------" + Environment.NewLine;
            tmp += "Polygon " + base.ToString() + Environment.NewLine+ "Couleur de remplissage : " + remplissage.ToString() + " Couleur de contour : " + contour.ToString() + " Opacite : " + opacite.ToString() + Environment.NewLine;
            for(int i=0;i<lCoordonnees.Count(); i++)
            {
                tmp += lCoordonnees[i].ToString();
                tmp += Environment.NewLine;
            }
            tmp += "---------------------------------------------" + Environment.NewLine;
            return tmp;
        }
        public override void Draw()
        {
            this.ToString();
        }
        public override int nbPoint
        {
            get { return lCoordonnees.Count; }
        }
        public bool IsPointClose(double x, double y, double precision)
        {
            ///D'abord on regarde si le point X Y appartient a la bouding box du polygone
            ///(Sinon ca ne sert à rien d'être plus précis)
            bool appartient = false;
            {
                double Xmax, Xmin, Ymax, Ymin;
                Xmax = lCoord.Max(Coordonnees => Coordonnees.X);
                Xmin = lCoord.Min(Coordonnees => Coordonnees.X);
                Ymax = lCoord.Max(Coordonnees => Coordonnees.Y);
                Ymin = lCoord.Min(Coordonnees => Coordonnees.Y);
                if (x < Xmin || x > Xmax || y > Ymax || y < Ymin)
                    return false;
            }

            ///Le point est AU MOINS dans la bouding box
            ///Maintenant l'on va être plus précis
            int i = 0;
            if(lCoordonnees.Count %2 == 0) /// ¨Pair, on repete
            {
                while(i<lCoordonnees.Count)
                {
                    if (i == lCoordonnees.Count - 2)
                    {
                        
                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X,lCoordonnees[i].Y, lCoordonnees[++i].X,lCoordonnees[i].Y, lCoordonnees[0].X,lCoordonnees[0].Y, x, y);
                        ++i;
                    }
                    else
                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X,lCoordonnees[i].Y, lCoordonnees[++i].X,lCoordonnees[i].Y, lCoordonnees[++i].X,lCoordonnees[i].Y, x,y);

                    if (appartient)
                        return appartient;
                }

                return appartient;
            }
            else /// Impair on ne repete pas
            {
                while(i<lCoordonnees.Count)
                {
                    if (i == lCoordonnees.Count - 2)
                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X,lCoordonnees[i].Y, lCoordonnees[++i].X,lCoordonnees[i].Y, lCoordonnees[0].X,lCoordonnees[0].Y, x, y);
                    else
                       appartient = MathUtil.IsInTriangle(lCoordonnees[i].X,lCoordonnees[i].Y, lCoordonnees[++i].X,lCoordonnees[i].Y, lCoordonnees[++i].X,lCoordonnees[i].Y, x, y);
                    if (appartient)
                        return appartient;
                    ++i;///On ne repete pas C
                    
                }
                return appartient;

            }

        }
        public List<ICoord> lCoord
        {
            get { return lCoordonnees; }
            set { lCoord = value; }
        }
        public ICoord WhichPointIsClose(double x, double y)
        {
            ICoord obj = lCoordonnees[0] ;

            double[] distancetmp = new double[3];
            double distancemin = -1;
            double[] distanceminTmp = new double[2];
            int i = 0, j;
            bool appartient;
            if (lCoordonnees.Count % 2 == 0) /// ¨Pair, on repete
            {
                while (i < lCoordonnees.Count)
                {
                    if (i == lCoordonnees.Count - 2)
                    {

                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[i].Y, lCoordonnees[0].X, lCoordonnees[0].Y, x, y);
                        ++i;
                    }
                    else
                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[i].Y, x, y);

                    if (appartient)
                    {
                        j = i;
                        if (i == lCoordonnees.Count - 2)
                        {
                            distancetmp[1] = MathUtil.DistanceDeuxPoints(lCoordonnees[j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[2] = MathUtil.DistanceDeuxPoints(lCoordonnees[--j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[0] = MathUtil.DistanceDeuxPoints(lCoordonnees[0].X, lCoordonnees[0].Y, x, y);
                            distanceminTmp = GetDistanceMin(distancemin, distancetmp);
                            if (distanceminTmp[1] != -1)
                            {
                                distancemin = distanceminTmp[0];

                                switch (distancetmp[1])
                                {
                                    case 0:
                                        j += 1;
                                        obj = lCoord[j];
                                        break;
                                    case 1:
                                        obj = lCoord[j];
                                        break;
                                    case 2:
                                        obj = lCoord[0];
                                        break;
                                }
                            }


                        }
                        else
                        {
                            distancetmp[0] = MathUtil.DistanceDeuxPoints(lCoordonnees[j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[1] = MathUtil.DistanceDeuxPoints(lCoordonnees[--j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[2] = MathUtil.DistanceDeuxPoints(lCoordonnees[--j].X, lCoordonnees[j].Y, x, y);
                            distanceminTmp = GetDistanceMin(distancemin, distancetmp);
                            if (distanceminTmp[1] != -1)
                            {
                                distancemin = distanceminTmp[0];

                                switch (distancetmp[1])
                                {
                                    case 0:
                                        j += 1;
                                        obj = lCoord[j];
                                        break;
                                    case 1:
                                        obj = lCoord[j];
                                        break;
                                    case 2:
                                        obj = lCoord[0];
                                        break;
                                }
                            }
                        }


                    }
                }


            }
            else /// Impair on ne repete pas
            {
                while (i < lCoordonnees.Count)
                {
                    if (i == lCoordonnees.Count - 2)
                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[i].Y, lCoordonnees[0].X, lCoordonnees[0].Y, x, y);
                    else
                        appartient = MathUtil.IsInTriangle(lCoordonnees[i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[i].Y, lCoordonnees[++i].X, lCoordonnees[i].Y, x, y);
                    if (appartient)
                    {
                        j = i;
                        if (i == lCoordonnees.Count - 2)
                        {
                            distancetmp[1] = MathUtil.DistanceDeuxPoints(lCoordonnees[j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[2] = MathUtil.DistanceDeuxPoints(lCoordonnees[--j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[0] = MathUtil.DistanceDeuxPoints(lCoordonnees[0].X, lCoordonnees[0].Y, x, y);
                            distanceminTmp = GetDistanceMin(distancemin, distancetmp);
                            if (distanceminTmp[1] != -1)
                            {
                                distancemin = distanceminTmp[0];

                                switch (distancetmp[1])
                                {
                                    case 0:
                                        j += 1;
                                        obj = lCoord[j];
                                        break;
                                    case 1:
                                        obj = lCoord[j];
                                        break;
                                    case 2:
                                        obj = lCoord[0];
                                        break;
                                }
                            }


                        }
                        else
                        {
                            distancetmp[0] = MathUtil.DistanceDeuxPoints(lCoordonnees[j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[1] = MathUtil.DistanceDeuxPoints(lCoordonnees[--j].X, lCoordonnees[j].Y, x, y);
                            distancetmp[2] = MathUtil.DistanceDeuxPoints(lCoordonnees[--j].X, lCoordonnees[j].Y, x, y);
                            distanceminTmp = GetDistanceMin(distancemin, distancetmp);
                            if (distanceminTmp[1] != -1)
                            {
                                distancemin = distanceminTmp[0];
                                switch (distancetmp[1])
                                {
                                    case 0:
                                        j += 1;
                                        obj = lCoord[j];
                                        break;
                                    case 1:
                                        obj = lCoord[j];
                                        break;
                                    case 2:
                                        obj = lCoord[0];
                                        break;
                                }
                            }
                        }
                        ++i;///On ne repete pas C

                    }

                }

            }
            return obj;
        }
        public double[] GetDistanceMin(double distancemin ,double[] distancetmp)
        {
            double[] distPos = new double[2];
            distPos[0] = distancemin;
            distPos[1] = -1;
            if (distancemin == -1)
            {
                distPos[0] = distancetmp[0];
                distPos[1] = 0;
            }

            for (int i=0;i<distancetmp.Length;i++)
            {
                if(distancetmp[i]<distPos[0])
                {
                    distPos[0] = distancetmp[i];
                    distPos[1] = i;
                }
            }
            return distPos;
        }


    }
}

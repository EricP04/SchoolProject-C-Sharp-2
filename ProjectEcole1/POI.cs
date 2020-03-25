using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathUtilLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace ProjectLibraryClass
{
    [Serializable]
    public class POI : Coordonnees, IIsPointClose, ICartoObj, ICoord
    {
        private string description;
        public POI(double x = 50.610869, double y = 5.510435) : base(x, y)
        {

            description = "Hepl life";
        }
        public POI(string nDes, double x, double y) : this(x, y)
        {
            description = nDes;
        }
        public POI(POI po) : this(po.X, po.Y)
        {
            description = po.Description;
        }
        public override double X
        {
            get { return base.X; }
            set { base.X = value; }
        }
        public override double Y
        {
            get { return base.Y; }
            set { base.Y = value; }
        }
        public override string Description
        {
            get { return description; }
            set { description = value; }
        }

        public override string ToString()
        {
            ///string buf = String.Format(base.ToString()+" " +"Nom : " + description);
            return "POI : " + base.ToString() + " Nom : " + description;
        }
        public bool IsPointClose(double x, double y, double precision)
        {
            bool isClose = false;
            double distance = MathUtil.DistanceDeuxPoints(X, Y, x, y);
            Console.WriteLine("Distance = " + distance + " Precision  = " + precision);
            if (distance <= precision)
                isClose = true;


            return isClose;
        }
        public override int nbPoint
        {
            get { return 1; }
        }

        public List<ICoord> lCoord
        {
            get
            {
                return new List<ICoord>() { this };
            }
            set { base.X = value.Last().X; base.Y = value.Last().Y; description = value.Last().Description; }
        }
        public ICoord WhichPointIsClose(double x, double y)
        {
            return this;

        }
        public double[] GetDistanceMin(double distancemin, double[] distancetmp)
        {
            double[] distPos = new double[2];
            distPos[0] = distancemin;
            distPos[1] = 0;
            return distPos;
            
        }



    }
}

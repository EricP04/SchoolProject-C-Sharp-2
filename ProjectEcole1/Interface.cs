using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
namespace ProjectLibraryClass
{
    public interface IIsPointClose
    {
        bool IsPointClose(double x, double y, double precision);
    }
    public interface IPointy
    {
        int nbPoint
        {
            get;
        }
        
    }
    public interface ICartoObj : IIsPointClose
    {
        List<ICoord> lCoord
        {
            get;
            set;
        }
        ICoord WhichPointIsClose(double x, double y);
        double[] GetDistanceMin(double distancemin, double[] distancetmp);
        int ID
        { get;
        }


    }
    public interface ICoord
    {
        double X
        {
            get;
            set;
        }
        double Y
        {
            get;
            set;
        }
        string Description
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathUtilLibrary;
namespace ProjectLibraryClass
{
    public class CartoObjComparer: IComparer<CartoObj>
    {
        public int Compare(CartoObj co1, CartoObj co2)
      {
        if (co1 == null || co2 == null)
            return 0;
            if (co1.nbPoint > co2.nbPoint)
                return 1;
            if (co1.nbPoint == co2.nbPoint)
                return 0;

       

            return -1;

     }
    }
}

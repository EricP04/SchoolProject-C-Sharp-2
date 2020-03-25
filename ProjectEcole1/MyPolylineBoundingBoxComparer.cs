using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryClass
{
    public class MyPolylineBoundingBoxComparer : IComparer<Polyline>
    {
        public int Compare(Polyline poly1, Polyline poly2)
        {
            if (poly1 == null || poly2 == null)
                return 0;

            return poly1.CalculSurface().CompareTo(poly2.CalculSurface());

        }
        
    }
}

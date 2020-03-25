using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathUtilLibrary;
namespace ProjectLibraryClass
{
    [Serializable]
    public abstract class CartoObj
    {
        protected int id;
        protected static int compteur;

        public CartoObj()
        {
            id = ++compteur; ///Je commence a 1

        }
        public override string ToString()
        {
            return "Id = " + id;
        }
        virtual public void Draw()
        {
            Console.WriteLine(this.ToString());
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        virtual public int nbPoint
        {
            get { return 1; }
        }

    }
}

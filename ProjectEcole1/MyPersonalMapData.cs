using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace ProjectLibraryClass
{
    [Serializable]
    public class MyPersonalMapData
    {
        private string nom;
        private string prenom;
        private string email;
        private ObservableCollection<ICartoObj> cartoCollection;
        public double lineEp;
        private double opacity;
        private double precision;
        private System.Drawing.Color contour;
        private System.Drawing.Color remplissage;

        public MyPersonalMapData()
        {
            nom = "default";
            prenom = "prenomdefault";
            email = "default@default.com";
            cartoCollection = new ObservableCollection<ICartoObj>();
            contour = System.Drawing.Color.Red;
            remplissage = System.Drawing.Color.White;
            lineEp = 5;
            opacity = 0.5;
            precision = 10;
            
    }
        public MyPersonalMapData(string n,string p, string em) : this()
        {
            nom = n;
            prenom = p;
            email = em;
        }
        public MyPersonalMapData(string n, string p, string em, ObservableCollection<ICartoObj> iCartoCollection) : this()
        {
            nom = n;
            prenom = p;
            email = em;
            cartoCollection = iCartoCollection;
        }

        /// Méthode ajoutant un objet ajoutant un objet dans la collection, objet qui implémente l'interface ICartoObj
        public void AddCartObj(ICartoObj cartObj)
        {
            Console.WriteLine(cartObj.ToString());
            cartoCollection.Add(cartObj);
            Console.WriteLine("Test : " + cartoCollection.Last().ToString());
        }
        public void Reinitialiser()
        {
            cartoCollection.Clear();
        }
        public ObservableCollection<ICartoObj> CartoCollection
        {
            get { return cartoCollection; }
        }

        public double Precision { get => precision; set => precision = value; }
        public double Opacity { get => opacity; set => opacity = value; }

        public double LineEp { get => lineEp; set => lineEp = value; }

        public System.Drawing.Color Contour { get => contour; set => contour = value; }
        
        public System.Drawing.Color Remplissage { get => remplissage; set => remplissage = value; }
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }
        public string Email
        {
            get { return email; }
            
        }

    }
}

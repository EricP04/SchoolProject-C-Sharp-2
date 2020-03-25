using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectLibraryClass;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl;
namespace ApplicationWPF
{
    /// <summary>
    /// Interaction logic for WindowItemProperty.xaml
    /// </summary>
    public partial class WindowItemProperty : Window
    {

        Pushpin pin;
        MapPolygon maPolygon;
        MapPolyline maPolyline;
        int id;
        List<double> xBase;
        List<double> yBase;
        List<string> descBase;
        private System.Drawing.Color remplissageBase;
        private System.Drawing.Color contourBase;
        ProjectLibraryClass.Polyline polyTmp;
        ProjectLibraryClass.Polygon polTmp;

        public WindowItemProperty(int index)
        {
            
            id = index;
            InitializeComponent();
            tIContour.Visibility = Visibility.Hidden;
            tIRemplissage.Visibility = Visibility.Hidden;
            ListBoxPropertyX.ItemsSource = MainWindow.mP.CartoCollection[index].lCoord;
            ListBoxPropertyY.ItemsSource = MainWindow.mP.CartoCollection[index].lCoord;
            ListBoxPropertyDesc.ItemsSource = MainWindow.mP.CartoCollection[index].lCoord;
              xBase = new List<Double>(MainWindow.mP.CartoCollection[index].lCoord.Select(x => x.X));
              yBase = new List<Double>(MainWindow.mP.CartoCollection[index].lCoord.Select(x => x.Y));
              descBase = new List<string>(MainWindow.mP.CartoCollection[index].lCoord.Select(x => x.Description));
            if (MainWindow.mP.CartoCollection[index] is POI)
            {
                pin = new Pushpin();
                pin.Location = new Location(MainWindow.mP.CartoCollection[index].lCoord[0].X, MainWindow.mP.CartoCollection[index].lCoord[0].Y);
                myMapProperty.Children.Add(pin);

            }
            if(MainWindow.mP.CartoCollection[index] is ProjectLibraryClass.Polygon)
            {
                polTmp = MainWindow.mP.CartoCollection[index] as ProjectLibraryClass.Polygon;
                maPolygon = new MapPolygon();
                myMapProperty.Children.Add(maPolygon);
                maPolygon.Locations = new LocationCollection();
                for(int i =0;i< MainWindow.mP.CartoCollection[index].lCoord.Count();i++)
                {
                    maPolygon.Locations.Add(new Location(MainWindow.mP.CartoCollection[index].lCoord[i].X, MainWindow.mP.CartoCollection[index].lCoord[i].Y));
                }
                
                maPolygon.Fill = MainWindow.ToBrush(polTmp.Rempli);
                maPolygon.Stroke = MainWindow.ToBrush(polTmp.Contour);
                remplissageBase = polTmp.Rempli;
                contourBase = polTmp.Contour;
                maPolygon.Opacity = polTmp.Opacite;
                maPolygon.StrokeThickness = polTmp.Epaisseur;
                tIContour.Visibility = Visibility.Visible;
                tIRemplissage.Visibility = Visibility.Visible;

                SliderContourR.Value = polTmp.Contour.R;
                SliderContourG.Value = polTmp.Contour.G;
                SliderContourB.Value = polTmp.Contour.B;
                PreviewColorContour.DataContext = MainWindow.ToBrush(polTmp.Contour);

                SliderRemplirR.Value = polTmp.Rempli.R;
                SliderRemplirG.Value = polTmp.Rempli.G;
                SliderRemplirB.Value = polTmp.Rempli.B;
                PreviewColorRemplissage.DataContext = MainWindow.ToBrush(polTmp.Rempli);



            }
            if (MainWindow.mP.CartoCollection[index] is ProjectLibraryClass.Polyline)
            {
                polyTmp = MainWindow.mP.CartoCollection[index] as ProjectLibraryClass.Polyline;

                contourBase = polyTmp.Couleur;

                maPolyline = new MapPolyline();
                myMapProperty.Children.Add(maPolyline);
                maPolyline.Locations = new LocationCollection();
                for (int i = 0; i < MainWindow.mP.CartoCollection[index].lCoord.Count(); i++)
                {
                    maPolyline.Locations.Add(new Location(MainWindow.mP.CartoCollection[index].lCoord[i].X, MainWindow.mP.CartoCollection[index].lCoord[i].Y));
                }
                maPolyline.StrokeThickness = polyTmp.Epaisseur;
                maPolyline.Stroke = MainWindow.ToBrush(polyTmp.Couleur);
                maPolyline.Opacity = 1;
                myMapProperty.Center = new Location(MainWindow.mP.CartoCollection[index].lCoord[0].X, MainWindow.mP.CartoCollection[index].lCoord[0].Y);
                myMapProperty.ZoomLevel = 2;
                tIContour.Visibility = Visibility.Visible;

                SliderContourR.Value = polyTmp.Couleur.R;
                SliderContourG.Value = polyTmp.Couleur.G;
                SliderContourB.Value = polyTmp.Couleur.B;
                PreviewColorContour.DataContext = MainWindow.ToBrush(polyTmp.Couleur);

            }

        }
        private void SliderContourR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                polyTmp.Couleur = System.Drawing.Color.FromArgb(Convert.ToByte(SliderContourR.Value), polyTmp.Couleur.G, polyTmp.Couleur.B);
                PreviewColorContour.DataContext = MainWindow.ToBrush(polyTmp.Couleur);
            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                polTmp.Contour = System.Drawing.Color.FromArgb(Convert.ToByte(SliderContourR.Value), polTmp.Contour.G, polTmp.Contour.B);
                PreviewColorContour.DataContext = MainWindow.ToBrush(polTmp.Contour);
            }


        }

        private void SliderContourG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                polyTmp.Couleur = System.Drawing.Color.FromArgb(polyTmp.Couleur.R, Convert.ToByte(SliderContourG.Value), polyTmp.Couleur.B);
                PreviewColorContour.DataContext = MainWindow.ToBrush(polyTmp.Couleur);
            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                polTmp.Contour = System.Drawing.Color.FromArgb(polTmp.Contour.R, Convert.ToByte(SliderContourG.Value), polTmp.Contour.B);
                PreviewColorContour.DataContext = MainWindow.ToBrush(polTmp.Contour);
            }

        }

        private void SliderContourB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                polyTmp.Couleur = System.Drawing.Color.FromArgb(polyTmp.Couleur.R, polyTmp.Couleur.G, Convert.ToByte(SliderContourB.Value));
                PreviewColorContour.DataContext = MainWindow.ToBrush(MainWindow.mP.Contour);
            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                polTmp.Contour = System.Drawing.Color.FromArgb(polTmp.Contour.R, polTmp.Contour.G, Convert.ToByte(SliderContourB.Value));
                PreviewColorContour.DataContext = MainWindow.ToBrush(polTmp.Contour);
            }

        }



        private void SliderRemplirB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            polTmp.Rempli= System.Drawing.Color.FromArgb(polTmp.Rempli.R, polTmp.Rempli.G, Convert.ToByte(SliderRemplirB.Value));
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(polTmp.Rempli);
        }

        private void SliderRemplirG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            polTmp.Rempli = System.Drawing.Color.FromArgb(polTmp.Rempli.R, Convert.ToByte(SliderRemplirG.Value), polTmp.Rempli.B);
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(polTmp.Rempli);
        }

        private void SliderRemplirR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            polTmp.Rempli = System.Drawing.Color.FromArgb(Convert.ToByte(SliderRemplirR.Value), polTmp.Rempli.G, polTmp.Rempli.B);
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(polTmp.Rempli);
        }
        private void AnnulerBouton_Click(object sender, RoutedEventArgs e)
        {
            for(int i =0;i<MainWindow.mP.CartoCollection[id].lCoord.Count();i++)
            {
                MainWindow.mP.CartoCollection[id].lCoord[i].X = xBase[i];
                MainWindow.mP.CartoCollection[id].lCoord[i].Y = yBase[i];
                MainWindow.mP.CartoCollection[id].lCoord[i].Description = descBase[i];

            }
            if(MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                polTmp.Rempli = remplissageBase;
                polTmp.Contour = contourBase;
                MainWindow.mP.CartoCollection[id] = polTmp;
            }
            if(MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                polyTmp.Couleur = contourBase;
                MainWindow.mP.CartoCollection[id] = polyTmp;
            }
            
            Close();

        }

        private void AppliquerModif_Click(object sender, RoutedEventArgs e)
        {
            if(MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                MainWindow.mP.CartoCollection[id] = polyTmp;
            } 
            if(MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                MainWindow.mP.CartoCollection[id] = polTmp;
            }
         
            Close();
        }

        private void TextProperty_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("Count = " + ListBoxPropertyDesc.Items.Count);
            int index = 0;
            /*foreach(TextBox item in ListBoxPropertyDesc.Items)
            {
                Console.WriteLine("Var : "+ item.ToString());  
            }*/
            Console.WriteLine("Position de l'objet : " + index);
            TextBox lb = sender as TextBox;
            Console.WriteLine("Test : " + lb.Text) ;
            Console.WriteLine("Coucou :  " + sender.ToString()); ;
            Console.WriteLine("TextPropertyChanged");
            
            Console.WriteLine("Object type = " + e.GetType());
            Console.WriteLine("ItemChanged : " + e.ToString() );
        }

        private void Effacer_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mP.CartoCollection.Remove(MainWindow.mP.CartoCollection[id]);
            Close();
        }

        private void Prévisualiser_Click(object sender, RoutedEventArgs e)
        {
            myMapProperty.Children.Clear();
            if (MainWindow.mP.CartoCollection[id] is POI)
            {
                pin = new Pushpin();
                pin.Location = new Location(MainWindow.mP.CartoCollection[id].lCoord[0].X, MainWindow.mP.CartoCollection[id].lCoord[0].Y);
                myMapProperty.Children.Add(pin);

            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                maPolygon = new MapPolygon();
                myMapProperty.Children.Add(maPolygon);
                maPolygon.Locations = new LocationCollection();
                for (int i = 0; i < MainWindow.mP.CartoCollection[id].lCoord.Count(); i++)
                {
                    maPolygon.Locations.Add(new Location(MainWindow.mP.CartoCollection[id].lCoord[i].X, MainWindow.mP.CartoCollection[id].lCoord[i].Y));
                }

                maPolygon.Fill = MainWindow.ToBrush(polTmp.Rempli);
                maPolygon.Stroke = MainWindow.ToBrush(polTmp.Contour);
                maPolygon.Opacity =polTmp.Opacite;
                maPolygon.StrokeThickness = polTmp.Epaisseur;



            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                maPolyline = new MapPolyline();
                myMapProperty.Children.Add(maPolyline);
                maPolyline.Locations = new LocationCollection();
                for (int i = 0; i < MainWindow.mP.CartoCollection[id].lCoord.Count(); i++)
                {
                    maPolyline.Locations.Add(new Location(MainWindow.mP.CartoCollection[id].lCoord[i].X, MainWindow.mP.CartoCollection[id].lCoord[i].Y));
                }
                maPolyline.StrokeThickness = polyTmp.Epaisseur;
                maPolyline.Stroke = MainWindow.ToBrush(polyTmp.Couleur);
                maPolyline.Opacity = MainWindow.mP.Opacity;
            }
            myMapProperty.Center = new Location(MainWindow.mP.CartoCollection[id].lCoord[0].X, MainWindow.mP.CartoCollection[id].lCoord[0].Y);
            myMapProperty.ZoomLevel = 2;
            Console.WriteLine("Position changé normalement");
            myMapProperty.UpdateLayout();
        }


    }
}

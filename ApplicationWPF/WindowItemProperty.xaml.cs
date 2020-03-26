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
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Textbox = System.Windows.Controls.TextBox;
using TextBox = System.Windows.Controls.TextBox;

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

        private System.Drawing.Color remplissageBase;
        private System.Drawing.Color contourBase;
        ProjectLibraryClass.Polyline polyTmp;
        ProjectLibraryClass.Polygon polTmp;
        List<ICoord> lCoordBase;
        public WindowItemProperty(int index)
        {
            
            id = index;
            InitializeComponent();
            tIContour.Visibility = Visibility.Hidden;
            tIRemplissage.Visibility = Visibility.Hidden;
            
            ListBoxPropertyX.ItemsSource = MainWindow.mP.CartoCollection[index].lCoord;
            ListBoxPropertyY.ItemsSource = MainWindow.mP.CartoCollection[index].lCoord;
            ListBoxPropertyDesc.ItemsSource = MainWindow.mP.CartoCollection[index].lCoord;
            lCoordBase = new List<ICoord>(MainWindow.mP.CartoCollection[index].lCoord);

            if (MainWindow.mP.CartoCollection[index] is POI)
            {
                pin = new Pushpin();
                pin.Location = new Location(MainWindow.mP.CartoCollection[index].lCoord[0].X, MainWindow.mP.CartoCollection[index].lCoord[0].Y);
                myMapProperty.Children.Add(pin);
                tiMore.Visibility = Visibility.Hidden;

            }
            else
            {
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

                    SliderEpaisseur.Value = ((ProjectLibraryClass.Polygon)MainWindow.mP.CartoCollection[index]).Epaisseur;
                    SliderOpacite.Value = ((ProjectLibraryClass.Polygon)MainWindow.mP.CartoCollection[index]).Opacite;
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
                    maPolyline.Opacity = polyTmp.Opacite;
                    myMapProperty.Center = new Location(MainWindow.mP.CartoCollection[index].lCoord[0].X, MainWindow.mP.CartoCollection[index].lCoord[0].Y);
                    myMapProperty.ZoomLevel = 2;
                    tIContour.Visibility = Visibility.Visible;

                    SliderContourR.Value = polyTmp.Couleur.R;
                    SliderContourG.Value = polyTmp.Couleur.G;
                    SliderContourB.Value = polyTmp.Couleur.B;
                    PreviewColorContour.DataContext = MainWindow.ToBrush(polyTmp.Couleur);
                    SliderEpaisseur.Value = ((ProjectLibraryClass.Polyline)MainWindow.mP.CartoCollection[index]).Epaisseur;
                    SliderOpacite.Value = ((ProjectLibraryClass.Polyline)MainWindow.mP.CartoCollection[index]).Opacite;


                }
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
            // for(int i =0;i<MainWindow.mP.CartoCollection[id].lCoord.Count();i++)
            //{
            /*MainWindow.mP.CartoCollection[id].lCoord[i].X = xBase[i];
            MainWindow.mP.CartoCollection[id].lCoord[i].Y = yBase[i];
            MainWindow.mP.CartoCollection[id].lCoord[i].Description = descBase[i];*/
            MainWindow.mP.CartoCollection[id].lCoord = lCoordBase;

           // }
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
                maPolyline.Opacity = polyTmp.Opacite;
            }
            myMapProperty.Center = new Location(MainWindow.mP.CartoCollection[id].lCoord[0].X, MainWindow.mP.CartoCollection[id].lCoord[0].Y);
            myMapProperty.ZoomLevel = 2;
            Console.WriteLine("Position changé normalement");
            myMapProperty.UpdateLayout();
        }



        private void TextPropertyDesc_GotFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Item sélectionné");
            TextBox tb = sender as TextBox;

            if (!(tb.DataContext is POI))
            {
                Console.WriteLine("C est une coordonnée");
                Console.WriteLine("Test : " + tb.DataContext);
                MessageBoxResult result = System.Windows.MessageBox.Show("Voulez-vous modifier la description de la coordonnée séléctionné ?", "Changement demandé",MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                {

                    Coordonnees co = tb.DataContext as Coordonnees;
                    ///Pour essayer de respecter les premières consignes, je n'ai pas voulu rajouter de description aux coordonnées, juste une valeur en lecture seule pour un meilleur rendu visuel, mais
                    ///du coup quand le texte change, s'il s'agit d'une coordonnée, je dois le transformer en POI(qui , contrairement aux coordonnées, ont une description modifiable)
                    int i = 0;
                    foreach (ICoord obj in MainWindow.mP.CartoCollection[id].lCoord)
                    {
                        if (((CartoObj)obj).ID == co.ID)
                            break;
                        else
                            i++;
                    }
                    if (i < MainWindow.mP.CartoCollection[id].lCoord.Count)
                    {
                        Console.WriteLine("I = " + i);
                        POI p = new POI(MainWindow.mP.CartoCollection[id].lCoord[i].X, MainWindow.mP.CartoCollection[id].lCoord[i].Y);
                        MainWindow.mP.CartoCollection[id].lCoord.Remove(MainWindow.mP.CartoCollection[id].lCoord[i]);
                        MainWindow.mP.CartoCollection[id].lCoord.Insert(i, p);
                        Console.WriteLine("TEST = ");
                        ListBoxPropertyDesc.ItemsSource = MainWindow.mP.CartoCollection[id].lCoord;
                        ListBoxPropertyDesc.Items.Refresh();

                    }
                    tb.GotFocus -= TextPropertyDesc_GotFocus;

                }
                else
                {
                    tb.IsReadOnly = true;
                }



            }
            else
                Console.WriteLine("C EST UN POI");

        }

        private void SliderOpacite_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                polTmp.Opacite = SliderOpacite.Value;
                myMapProperty.UpdateLayout();
            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                polyTmp.Opacite = SliderOpacite.Value;
                myMapProperty.UpdateLayout();
            }
        }


        private void SliderEpaisseur_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polygon)
            {
                polTmp.Epaisseur = SliderEpaisseur.Value;
                myMapProperty.UpdateLayout();
            }
            if (MainWindow.mP.CartoCollection[id] is ProjectLibraryClass.Polyline)
            {
                polyTmp.Epaisseur = SliderEpaisseur.Value;
                myMapProperty.UpdateLayout();
            }
        }
    }
}

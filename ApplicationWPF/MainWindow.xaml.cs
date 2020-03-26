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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectLibraryClass;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl;
using System.Drawing;
using MathUtilLibrary;
using Brushes = System.Windows.Media.Brushes;
using System.ComponentModel;
using Window = System.Windows.Window;
using ListBox = System.Windows.Controls.ListBox;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.ObjectModel;


using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// Longitude=Y, Latitude=X.
/// </summary>
namespace ApplicationWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string status;
        List<ICoord> coordTmp;

        LocationCollection locCol;
        MapPolyline maPolyline;
        MapPolygon maPolygon;
        private static System.Windows.Media.Brush _lbBackgroundColor;
        public delegate void BackgroundColorD(object sender, BackgroundColorEvent e);
        public static event BackgroundColorD BgColorChanged; 
        private static System.Windows.Media.Brush _lbForegroundColor;
        public delegate void ForegroundColorD(object sender, ForegroundColorEvent e);
        public static event ForegroundColorD FgColorChanged;

        public static System.Windows.Media.Brush BackgroundColor
        {
            get { return _lbBackgroundColor; }
            set { _lbBackgroundColor = value; }
        }
        public static System.Windows.Media.Brush ForegroundColor
        {
            get { return _lbForegroundColor; }
            set { _lbForegroundColor = value; }
        }

        // On mettra les données de login que l'on encodera dans le premier écran (écran de login)
        static public MyPersonalMapData mP;
        public MainWindow(MyPersonalMapData m)
        {
            mP = new MyPersonalMapData(m.Nom,m.Prenom,m.Email);
            foreach (ICartoObj obj in m.CartoCollection)
            {
                if (obj is POI)
                    mP.AddCartObj(new POI((POI)obj));
                if (obj is ProjectLibraryClass.Polygon)
                    mP.AddCartObj(new ProjectLibraryClass.Polygon((ProjectLibraryClass.Polygon)obj));
                if (obj is ProjectLibraryClass.Polyline)
                    mP.AddCartObj(new ProjectLibraryClass.Polyline((ProjectLibraryClass.Polyline)obj));
            }
            coordTmp = new List<ICoord>();
            _lbBackgroundColor = new SolidColorBrush(System.Windows.Media.Colors.White);
            _lbForegroundColor = new SolidColorBrush(Colors.Black);
            status = null;
            InitializeComponent();
            ListBoxMyPersonalData.ItemsSource = mP.CartoCollection;
            UpdateStatusBar("Zoom level = " +myMap.ZoomLevel.ToString());
            ListBoxMyPersonalData.Background = BackgroundColor;
            ListBoxMyPersonalData.Foreground = ForegroundColor;
            BgColorChanged += BackgroundColorHasChanged;
            FgColorChanged += ForegrounddColorHasChanged;
            if(mP.CartoCollection.Count>0)
            {
                
                RefreshMap();
            }

        }

        private void ButtonCreer_Click(object sender, RoutedEventArgs e)
        {
            status = "Créer";
            UpdateStatusBar("BoutonCreer click");
            ActuallySelected();
            if(RBPolyline.IsChecked == true || RBPolygon.IsChecked == true)
            {
                BouttonFinirTrace.Visibility = Visibility.Visible;
            }

        }
        private void ButtonModifier_Click(object sender, RoutedEventArgs e)
        {
            status = "Modifier";
            UpdateStatusBar("Bouton Modifier click");
            ActuallySelected();
        }

        private void ButtonSupprimer_Click(object sender, RoutedEventArgs e)
        {
            status = "Supprimer";
            UpdateStatusBar("Bouton Supprimer click");
            ActuallySelected();
            RBPoi.IsChecked = false;
            RBPolygon.IsChecked = false;
            RBPolyline.IsChecked = false;
        }

        /// Va gérer les clicks sur la map en fonction du mode dans lequel on se trouve
        private void myMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateStatusBar("MouseLeftButtonDown");
            if(status != null)
            {
                System.Windows.Point mousePosition = e.GetPosition(myMap);
                ICoord obj;
                bool trouve = false;
                List<int> index = new List<int>();
                double distancemin,distanceTmp;

                Location loc = new Location();
                loc = myMap.ViewportPointToLocation(mousePosition);

                int i = 0, distPos=0;
                ///ISPointClose
                ///On cherche les CartoObj proche
                foreach (ICartoObj cartoObj in mP.CartoCollection)
                {
                    Console.WriteLine("Test : cartoObj = " + cartoObj);
                    Console.WriteLine("location = " + loc.Latitude + " / " + loc.Longitude);
                    if (cartoObj.IsPointClose(loc.Latitude,loc.Longitude, mP.Precision) == true)
                    {
                        Console.WriteLine("IS POINT CLOSE TRUE");
                        trouve = true;
                        index.Add(i);
                        //obj = cartoObj.WhichPointIsClose(e.GetPosition(myMap).X,e.GetPosition(myMap).Y);
                    }
                    i++;
                }

                if(trouve)
                {
                    Console.WriteLine("ON A TROUVE UN POINT PROCHE");
                    ///On va devoir trouver quel CartoObj est le plus proche
                    ICoord[] objtmp = new ICoord[index.Count];
                
                    i = 0;
                   for(int j = 0;j<index.Count;j++)
                    {

                        objtmp[i] = mP.CartoCollection[index[j]].WhichPointIsClose(loc.Latitude,loc.Longitude);
                        i += 1;
                    }
                    ///Comparer la distance entre chaque
                    i = 0;
                    distancemin = -1;

                    foreach (ICoord coord in objtmp)
                    {
                        Console.WriteLine("Comparaison avec le point : " + coord.X+"/"+coord.Y) ;
                        Console.WriteLine("Point click : " + loc.Latitude+"/"+ loc.Longitude);
                        distanceTmp = MathUtil.DistanceDeuxPoints(coord.X, coord.Y, loc.Latitude, loc.Longitude);
                        if (distancemin == -1)
                        {
                            distancemin = distanceTmp;
                            distPos = i;
                        }
                        if (distancemin > distanceTmp)
                        {
                            distancemin = distanceTmp;
                            distPos = i;
                        }
                        i += 1;

                    }
                    Console.WriteLine("Point le plus proche : " + objtmp[distPos].X + "/" + objtmp[distPos].Y);
                    ///Arriver ici, nous avons les coordonnées du point le plus proche
                    if(objtmp[distPos] is POI)
                    {
                        if(RBPoi.IsChecked == true)
                        {
                            ///Pas de superposition de pushpin
                            loc = new Location();
                            loc = myMap.ViewportPointToLocation(mousePosition);
                            obj = new POI(loc.Latitude, loc.Longitude);

                        }
                        else
                        {
                            obj = objtmp[distPos] ;
                            loc = new Location(obj.X, obj.Y);

                        }
                    }
                    else
                    {
                        if(RBPoi.IsChecked == true)
                        {
                            obj = new POI(objtmp[distPos].X, objtmp[distPos].Y);
                        }
                        else
                            obj = objtmp[distPos];
                
                    loc = new Location(obj.X, obj.Y);
            
                    }
                
                }
                else
                {
                    Console.WriteLine("ON A PAS TROUVE DE POINT PROCHE");
                    loc = new Location();
                    loc = myMap.ViewportPointToLocation(mousePosition);
                    Console.WriteLine("Loc = " + loc.Latitude + "/ " + loc.Longitude);
                    if (RBPoi.IsChecked == true)
                    {
                        obj = new POI(loc.Latitude, loc.Longitude);
                    }
                    else
                        obj = new Coordonnees(loc.Latitude, loc.Longitude) ;
               

                }
                index.Clear();
            
                Console.WriteLine("Location = ", loc.Longitude+ " / "+ loc.Latitude);
                switch(status)
                {
                    case "Créer":
                        {
                            UpdateStatusBar("MouseLeftButtonDownCréer");

                            if (RBPoi.IsChecked == true)
                            {
                                Pushpin pin = new Pushpin();
                                pin.Location = loc;
                                myMap.Children.Add(pin);
                                mP.AddCartObj(obj as POI);
                                Console.WriteLine("Test après add : "+ mP.CartoCollection.Last().lCoord.Last());

                                UpdateStatusBar(mP.CartoCollection.Last().ToString()) ;
                                //ListBoxMyPersonalData.Items.Add(new POI(loc.Latitude,loc.Longitude));
                            }
                            if(RBPolyline.IsChecked == true)
                            {
                                if(coordTmp.Count == 0)
                                {
                                    maPolyline = new MapPolyline();
                                    myMap.Children.Add(maPolyline);
                                    locCol = new LocationCollection();
                                    maPolyline.Locations = locCol;
                                    maPolyline.StrokeThickness = mP.lineEp;
                                    maPolyline.Stroke = ToBrush(mP.Contour);
                                    maPolyline.Opacity = mP.Opacity;

                                }


                                if (coordTmp.Count>2)
                                {
                                    if((coordTmp[0].X <= obj.X + mP.Precision && coordTmp[0].X >= obj.X - mP.Precision) && (coordTmp[0].Y <= obj.Y + mP.Precision && coordTmp[0].Y >= obj.Y- mP.Precision))
                                    {
                                        ///Notre polyline est revenu a son point de départ -> Polygon ?
                                        MessageBoxResult res = MessageBox.Show("Voulez-vous en faire un polygon ? ", "Polygon", MessageBoxButton.YesNo);
                                        if(res == MessageBoxResult.Yes)
                                        {
                                            ///On va enfaire un polygon
                                            
                                            maPolygon = new MapPolygon();
                                            myMap.Children.Add(maPolygon);
                                            maPolygon.Locations = locCol;
                                            maPolygon.StrokeThickness = mP.lineEp;
                                            maPolygon.Stroke = ToBrush(mP.Contour);
                                            maPolygon.Fill = ToBrush(mP.Remplissage);
                                            maPolygon.Opacity = mP.Opacity;
                                            maPolygon.Locations = maPolyline.Locations;
                                            RBPolygon.IsChecked = true;
                                            RBPolyline.IsChecked = false;
                                            ButtonFinirTrace_Click(null, null);

                                            return;
                                        }
                                    }
                                }
                                maPolyline.Locations.Add(loc);

                                coordTmp.Add(obj);

                            }
                            if (RBPolygon.IsChecked == true)
                            {
                                if (coordTmp.Count == 0)
                                {
                                    maPolygon = new MapPolygon();
                                    myMap.Children.Add(maPolygon);
                                    locCol = new LocationCollection();
                                    maPolygon.Locations = locCol;
                                    maPolygon.StrokeThickness = mP.lineEp;
                                    maPolygon.Stroke = ToBrush(mP.Contour);
                                    maPolygon.Fill = ToBrush(mP.Remplissage);
                                    maPolygon.Opacity = mP.Opacity;

                                }
                                coordTmp.Add(obj);

                                maPolygon.Locations.Add(loc);
                            }
                            break;
                        }
                    case "Supprimer":
                        {
                            Console.WriteLine("Supprimer");
                            if (trouve)
                            { 
                                bool find = false;
                                int j = 0, a = 0;
                        
                                ///On sait sur quel point/figure on clique, on doit retrouver la position de chaque
                                while(j<mP.CartoCollection.Count && find == false)
                                {
                                    a = 0;
                                    while(a<mP.CartoCollection[j].lCoord.Count && find == false)
                                    {
                                        if (mP.CartoCollection[j].lCoord[a].X == obj.X && mP.CartoCollection[j].lCoord[a].Y == obj.Y)
                                            find = true;
                                        else
                                            a++;
                                    }
                                    if (!find)
                                        j++;
                                }
                                if (find)
                                {
                                    Console.WriteLine("TROUVE");
                                    // J contient l'index de la figure contenant le point sélectionné
                                    myMap.Children.Remove(myMap.Children[j]);
                                    mP.CartoCollection.Remove(mP.CartoCollection[j]);
                                    //ListBoxMyPersonalData.Items.Refresh();
                                }                        
                            }

                            break;
                        }
                    case "Modifier":
                        {
                            ///On va récupérer l'index
                            if(trouve)
                            {
                                bool find = false;
                                int j = 0, a = 0;
                                while (j < mP.CartoCollection.Count && find == false)
                                {
                                    a = 0;
                                    while (a < mP.CartoCollection[j].lCoord.Count && find == false)
                                    {
                                        if (mP.CartoCollection[j].lCoord[a].X == obj.X && mP.CartoCollection[j].lCoord[a].Y == obj.Y)
                                            find = true;
                                        else
                                            a++;
                                    }
                                    if (!find)
                                        j++;
                                }
                                if(find)
                                {
                                    WindowItemProperty wItem = new WindowItemProperty(j);
                                    wItem.ShowDialog();

                                    ListBoxMyPersonalData.Items.Refresh();
                                    RefreshMap();
                                }
                            }


                            break;
                        }
                }
            }

        }

        private void UpdateStatusBar(string s)
        {
            statusBar.Text = s;
        }
        private void ActuallySelected()
        {
            switch(status)
            {
                case "Créer":
                    {
                        BouttonCreer.Background = Brushes.Blue;
                        BouttonModifier.Background = default;
                        BouttonSupprimer.Background = default;
                        break;
                    }
                case "Modifier":
                    {
                        BouttonCreer.Background = default;
                        BouttonModifier.Background = Brushes.Blue;
                        BouttonSupprimer.Background = default;
                        break;
                    }
                case "Supprimer":
                    {
                        BouttonCreer.Background = default;
                        BouttonModifier.Background = default;
                        BouttonSupprimer.Background = Brushes.Blue;
                        break;
                    }
                default:
                    {
                        BouttonCreer.Background = default;
                        BouttonModifier.Background = default;
                        BouttonSupprimer.Background = default;
                        BouttonFinirTrace.Visibility = Visibility.Hidden;
                        break;
                    }
            }
        }

        private void ButtonFinirTrace_Click(object sender, RoutedEventArgs e)
        {

            if (RBPolyline.IsChecked == true)
            {

                mP.AddCartObj(new ProjectLibraryClass.Polyline(mP.lineEp, mP.Contour,mP.Opacity, coordTmp));


            }
            if(RBPolygon.IsChecked == true)
            {
                mP.AddCartObj(new ProjectLibraryClass.Polygon(coordTmp, mP.Remplissage, mP.Contour, mP.Opacity,mP.LineEp));
            }
         //   locCol.Clear();
            coordTmp.Clear();
            reset();


        }

        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            status = default;
            ActuallySelected();

        }
        private void reset()
        {
            status = default;
            ActuallySelected();
        }
        public static System.Windows.Media.Brush ToBrush(System.Drawing.Color color)
        {
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void ListBoxMyPersonalData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = sender as ListBox;
            int oldCount = lb.Items.Count;
            if(lb.SelectedItem != null)
            {
                int index = lb.SelectedIndex;
                UpdateStatusBar("Item sélectionné" + index.ToString() + " :) ");
                Console.WriteLine(mP.CartoCollection[index].ToString());
                WindowItemProperty wItem = new WindowItemProperty(index);
                wItem.ShowDialog();
                
                lb.Items.Refresh();
                RefreshMap();

                
            }
        }

        private void RefreshMap()
        {
            myMap.Children.Clear();
            foreach (ICartoObj carto in mP.CartoCollection)
            {
                if(carto is POI)
                {
                    AjouterPushPin(carto);
                }
                if(carto is ProjectLibraryClass.Polyline)
                {
                    AjouterPolyline(carto);
                }
                if(carto is ProjectLibraryClass.Polygon)
                {
                    AjouterPolygon(carto);
                }
            }
        }
        private void AjouterPushPin(ICartoObj carto)
        {
            Pushpin pin = new Pushpin();
            pin.Location = new Location(carto.lCoord[0].X, carto.lCoord[0].Y);
            myMap.Children.Add(pin);
        }
        private void AjouterPolyline(ICartoObj carto)
        {
            MapPolyline poly = new MapPolyline();
            poly.Locations = new LocationCollection();
            foreach (ICoord coord in carto.lCoord)
            {
                poly.Locations.Add( new Location(coord.X, coord.Y));
            }
            ProjectLibraryClass.Polyline polTmp = carto as ProjectLibraryClass.Polyline;
            poly.StrokeThickness = polTmp.Epaisseur;
            poly.Stroke = ToBrush(polTmp.Couleur);
            poly.Opacity = polTmp.Opacite;
            myMap.Children.Add(poly);


        }
        private void AjouterPolygon(ICartoObj carto)
        {
            MapPolygon poly = new MapPolygon();
            poly.Locations = new LocationCollection();
            foreach (ICoord coord in carto.lCoord)
            {
                poly.Locations.Add(new Location(coord.X, coord.Y));
            }
            ProjectLibraryClass.Polygon polTmp = carto as ProjectLibraryClass.Polygon;
            poly.StrokeThickness = polTmp.Epaisseur;
            poly.Stroke = ToBrush(polTmp.Contour);
            poly.Fill = ToBrush(polTmp.Rempli);
            poly.Opacity = polTmp.Opacite;
            myMap.Children.Add(poly);

        }

        private void MenuItemOption_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("BoutonOptionClick");
            WindowOption wOption = new WindowOption(BgColorChanged,FgColorChanged);
            wOption.Show();
            RefreshMap();
        }

        public void BackgroundColorHasChanged(object sender, BackgroundColorEvent e)
        {
            Console.WriteLine("BackgroundColorHasChanged");
            BackgroundColor = e.bg;
            ListBoxMyPersonalData.Background = BackgroundColor;


        } 
        public void ForegrounddColorHasChanged(object sender, ForegroundColorEvent e)
        {
            Console.WriteLine("BackgroundColorHasChanged");
            ForegroundColor = e.fg;
            ListBoxMyPersonalData.Foreground = ForegroundColor;


        }

        private void MenuItemPoiExport_Click(object sender, RoutedEventArgs e)
        {
            WindowExport wExport = new WindowExport(new POI());
            wExport.ShowDialog();
        }

        private void MenuItemTrajetExport_Click(object sender, RoutedEventArgs e)
        {
            WindowExport wExport = new WindowExport(null);
            wExport.ShowDialog();
        }

        private void MenuItemPOIImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.DefaultExt = ".csv";
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                Excel.Application oExcel = new Excel.Application();

                Excel.Workbook WB = oExcel.Workbooks.Open(dlg.FileName);
                string ExcelWorkbookName = WB.Name;
                int worksheetcount = WB.Worksheets.Count;
                Excel.Worksheet wks = (Excel.Worksheet)WB.Worksheets[1];

                string firstworksheetname = wks.Name;

                var firestcellValue = ((Excel.Range)wks.Cells[1, 1]).Value;
                Console.WriteLine("FIRST CELL VALUE = " + firestcellValue);
                Console.WriteLine("TEST = ", wks.Cells[2, 1]);
                Console.WriteLine("TEST = ", wks.Cells[2, 2]);
                Console.WriteLine("TEST = ", wks.Cells[2, 3]);
                Console.WriteLine("On a ouvert : " + dlg.FileName);
                POI p = new POI(new POI((string)((Excel.Range)wks.Cells[2, 3]).Value, (double)((Excel.Range)wks.Cells[2, 1]).Value, (double)((Excel.Range)wks.Cells[2, 2]).Value));
                mP.AddCartObj(p);
                AjouterPushPin(p);

                releaseObject(oExcel);
                releaseObject(WB);
                releaseObject(wks);
                
            }
        }

        private void MenuItemTrajetImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.DefaultExt = ".csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Excel.Application oExcel = new Excel.Application();

                Excel.Workbook WB = oExcel.Workbooks.Open(dlg.FileName);
                string ExcelWorkbookName = WB.Name;
                int worksheetcount = WB.Worksheets.Count;
                Excel.Worksheet wks = (Excel.Worksheet)WB.Worksheets[1];

                string firstworksheetname = wks.Name;
                Excel.Range range = wks.UsedRange;
                
                var firestcellValue = ((Excel.Range)wks.Cells[1, 1]).Value;
                Console.WriteLine("Test row : " + range.Rows.Count);
                Console.WriteLine("Test col : " + range.Columns.Count);
                Console.WriteLine("Test val last : " + ((Excel.Range)wks.Cells[range.Rows.Count, 1]).Value);
                List<ICoord> lCoord = new List<ICoord>();
                for(int i =1;i<=range.Rows.Count;i++)
                {
                    if(((Excel.Range)wks.Cells[i,3]).Value == "/")
                    {
                        lCoord.Add(new Coordonnees((double)((Excel.Range)wks.Cells[i, 1]).Value, ((Excel.Range)wks.Cells[i, 2]).Value));
                    }
                    else
                    {
                        lCoord.Add(new POI((string)((Excel.Range)wks.Cells[i,3]).Value,(double)((Excel.Range)wks.Cells[i, 1]).Value, ((Excel.Range)wks.Cells[i, 2]).Value));
                    }
                }
                ICartoObj poly;
                ///On va tester si le premier point est égale au dernier -> Si oui : Polygon    ->Sinon : Polyline
                if(((Excel.Range)wks.Cells[range.Rows.Count,1]).Value ==( (Excel.Range)wks.Cells[1, 1]).Value && 
                    ((Excel.Range)wks.Cells[range.Rows.Count, 2]).Value == ((Excel.Range)wks.Cells[1, 2]).Value &&
                        ((Excel.Range)wks.Cells[range.Rows.Count, 3]).Value == ((Excel.Range)wks.Cells[1, 3]).Value)
                    {
                    Console.WriteLine("POLYGON");
                    lCoord.Remove(lCoord.Last<ICoord>());
                    poly = new ProjectLibraryClass.Polygon(lCoord, mP.Remplissage,mP.Contour,mP.Opacity,mP.LineEp) ;
                    AjouterPolygon(poly);
                 }
                else
                {
                    poly = new ProjectLibraryClass.Polyline(mP.LineEp, mP.Contour,mP.Opacity, lCoord);
                    AjouterPolyline(poly);
                }
                mP.AddCartObj(poly);



                releaseObject(oExcel);
                releaseObject(WB);
                releaseObject(wks);



            }
        }

        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception lors de la libération de l'objet " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void MenuItemAboutBox_Click(object sender, RoutedEventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.DefaultExt = ".txt";
            dlg.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename;
                filename = dlg.FileName;
                Console.WriteLine("Filename = " + filename);
                Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                IFormatter fo = new BinaryFormatter();
                ObservableCollection<ICartoObj> iCartoObjRead = (ObservableCollection<ICartoObj>)fo.Deserialize(stream);
                foreach (ICartoObj carto in iCartoObjRead)
                {
                    mP.AddCartObj(carto);
                    if (carto is POI)
                        AjouterPushPin(carto);
                    else
                    {
                        if (carto is ProjectLibraryClass.Polyline)
                            AjouterPolyline(carto);
                        else
                            AjouterPolygon(carto);
                    }
                }
            }

            
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            string filenametmp = mP.Prenom + mP.Nom + ".txt";
            Console.WriteLine("Filename = " + filenametmp);
            if (File.Exists(filenametmp))
                File.Delete(filenametmp);
            Stream fStream = new FileStream(filenametmp, FileMode.Create,FileAccess.Write) ;
            IFormatter fo = new BinaryFormatter();
            fo.Serialize(fStream, mP.CartoCollection);
            fStream.Close();
            MessageBox.Show("Sauvegarde effectué !", "Sauvegarde", MessageBoxButton.OK);

        }
    }

    public class BackgroundColorEvent : EventArgs
    {
        public System.Windows.Media.Brush bg { get; set; }
         
       public BackgroundColorEvent(System.Windows.Media.Brush b)
        {
           bg = b;

        }
    }
    public class ForegroundColorEvent : EventArgs
    {
        public System.Windows.Media.Brush fg { get; set; }
        public ForegroundColorEvent(System.Windows.Media.Brush b)
        {
            fg = b;
        }
    }

}

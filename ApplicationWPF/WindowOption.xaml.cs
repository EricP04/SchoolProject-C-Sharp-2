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
namespace ApplicationWPF
{
    /// <summary>
    /// Logique d'interaction pour WindowOption.xaml
    /// </summary>
    public partial class WindowOption : Window
    {
        public Brush color;
        public static event MainWindow.BackgroundColorD BgColorChanged;
        private static string filename;
        public WindowOption(MainWindow.BackgroundColorD d)
        {
            InitializeComponent( );
            SliderOpacite.Value = MainWindow.mP.Opacity;
            SliderPrecision.Value = MainWindow.mP.Precision;
            SliderEpaisseur.Value = MainWindow.mP.LineEp;

            SliderContourR.Value = MainWindow.mP.Contour.R;
            SliderContourG.Value = MainWindow.mP.Contour.G;
            SliderContourB.Value = MainWindow.mP.Contour.B;
            PreviewColorContour.DataContext = MainWindow.ToBrush(MainWindow.mP.Contour); 

            SliderRemplirR.Value = MainWindow.mP.Remplissage.R;
            SliderRemplirG.Value = MainWindow.mP.Remplissage.G;
            SliderRemplirB.Value = MainWindow.mP.Remplissage.B;
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(MainWindow.mP.Remplissage) ;

            tbCouleurSelect.Visibility = Visibility.Hidden;
            RectanglePreviewBgColor.Visibility = Visibility.Hidden;
            lbAppliquerButton.Visibility = Visibility.Hidden;
            lbAnnulerAppliquerButton.Visibility = Visibility.Hidden;
            RectangleActuallySelected.Fill = MainWindow.BackgroundColor;
            ListboxBackgroundDisplayer.ItemsSource = typeof(Brushes).GetProperties();
            BgColorChanged += d;
            tbFileActuallySelected.Text = filename;


        }

        private void SliderOpacite_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine("Avant Opacity = " + MainWindow.mP.Opacity);

            MainWindow.mP.Opacity = SliderOpacite.Value;
            Console.WriteLine("Après Opacity = " + MainWindow.mP.Opacity);
            TbOpacite.DataContext = MainWindow.mP;

        }

        private void SliderPrecision_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Precision = SliderPrecision.Value;
            TbPrecision.DataContext = MainWindow.mP;

        }


        private void SliderContourR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Contour = System.Drawing.Color.FromArgb(Convert.ToByte( SliderContourR.Value), MainWindow.mP.Contour.G, MainWindow.mP.Contour.B);
            PreviewColorContour.DataContext = MainWindow.ToBrush(MainWindow.mP.Contour);

        }

        private void SliderContourG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Contour = System.Drawing.Color.FromArgb(MainWindow.mP.Contour.R, Convert.ToByte(SliderContourG.Value), MainWindow.mP.Contour.B);
            PreviewColorContour.DataContext = MainWindow.ToBrush(MainWindow.mP.Contour);
        }

        private void SliderContourB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Contour = System.Drawing.Color.FromArgb(MainWindow.mP.Contour.R, MainWindow.mP.Contour.G, Convert.ToByte(SliderContourB.Value));
            PreviewColorContour.DataContext = MainWindow.ToBrush(MainWindow.mP.Contour);
        }



        private void SliderRemplirB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Remplissage = System.Drawing.Color.FromArgb(MainWindow.mP.Remplissage.R, MainWindow.mP.Remplissage.G, Convert.ToByte(SliderRemplirB.Value));
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(MainWindow.mP.Remplissage);
        }

        private void SliderRemplirG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Remplissage = System.Drawing.Color.FromArgb(MainWindow.mP.Remplissage.R, Convert.ToByte(SliderRemplirG.Value), MainWindow.mP.Remplissage.B);
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(MainWindow.mP.Remplissage);
        }

        private void SliderRemplirR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.Remplissage = System.Drawing.Color.FromArgb(Convert.ToByte(SliderRemplirR.Value), MainWindow.mP.Remplissage.G, MainWindow.mP.Remplissage.B);
            PreviewColorRemplissage.DataContext = MainWindow.ToBrush(MainWindow.mP.Remplissage);
        }

        private void ListboxBackgroundDisplayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("e.Source = " + e.Source.ToString());
            ListBox lb = sender as ListBox;
            Console.WriteLine("Selected item = " + lb.SelectedItem.ToString());
            int index = lb.SelectedIndex;
            Console.WriteLine("Index = " + index);
            System.Reflection.PropertyInfo[] properties = typeof(Brushes).GetProperties();
            Console.WriteLine("Item sélectionné = " + properties[index].Name);
            var selectedItem = (System.Reflection.PropertyInfo)lb.SelectedItem;
            color = (Brush)selectedItem.GetValue(null, null);
            tbCouleurSelect.Visibility = Visibility.Visible;
            RectanglePreviewBgColor.Visibility = Visibility.Visible;
            lbAppliquerButton.Visibility = Visibility.Visible;
            lbAnnulerAppliquerButton.Visibility = Visibility.Visible;
            RectanglePreviewBgColor.Fill = color;
            
        }

        private void lbAppliquerButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("AppliquerClick");
            BgColorChanged(this, new BackgroundColorEvent(color));
        }

        private void lbAnnulerAppliquerButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SliderEpaisseur_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.mP.LineEp = SliderEpaisseur.Value;
            TbPrecision.DataContext = MainWindow.mP;
        }

        private void ButtonFichierSelect_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                filename = fbd.SelectedPath;
                Console.WriteLine("Filename = " + filename);
                tbFileActuallySelected.Text = filename;
            }

        }
        public static string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
    }
}

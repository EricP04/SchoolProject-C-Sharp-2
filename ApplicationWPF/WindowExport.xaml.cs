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
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Window = System.Windows.Window;
using ListBox = System.Windows.Controls.ListBox;
using Excel = Microsoft.Office.Interop.Excel;
namespace ApplicationWPF
{
    /// <summary>
    /// Logique d'interaction pour WindowExport.xaml
    /// </summary>
    public partial class WindowExport : Window
    {
        public ObservableCollection<ICartoObj> type;
        int index;
        private Excel.Application MyApp = null;
        private Excel.Workbook MyBook = null;
        private Excel.Worksheet MySheet = null;

        public WindowExport(ICartoObj carto)
        {
            
            index = -1;
            type = new ObservableCollection<ICartoObj>();
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            InitializeComponent();
            if (carto is POI)
            {
                foreach(ICartoObj p in MainWindow.mP.CartoCollection)
                {
                    if(p is POI)
                        type.Add(p);
                }
                tbVeuillezSelectionner.Text = "Veuillez selectionner un POI";

            }
            else
            {
                foreach(ICartoObj c in MainWindow.mP.CartoCollection)
                {
                    if (!(c is POI))
                    {
                        type.Add(c);
                    }
                }
                if (carto is ProjectLibraryClass.Polyline)
                    tbVeuillezSelectionner.Text = "Veuillez sélectionner un Polyline";
                else
                    tbVeuillezSelectionner.Text = "Veuillez sélectionner un Polygon";
            }

            lbCartoObjExport.ItemsSource = type;

        }

        private void lbCartoObjExport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = sender as ListBox;
            index = lb.SelectedIndex;
            Console.WriteLine("Type = " + type[index].GetType());
            Console.WriteLine("ITEM SELECTED " + type[index].ToString());
            Console.WriteLine("ID TROUVE = " + type[index].ID);


        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbfileName.Text))
            {
                MessageBox.Show("Veuillez saisir un nom !", "Message Erreur", MessageBoxButton.OK);
            }
            else
            {
                if(string.IsNullOrEmpty(WindowOption.Filename))
                {
                    MessageBox.Show("Veuillez choisir le fichier où exporter vos fichier");
                    System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                    fbd.ShowNewFolderButton = true;
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if(result == System.Windows.Forms.DialogResult.OK)
                        WindowOption.Filename = fbd.SelectedPath;
                }

                string filenameExport = WindowOption.Filename + "/" + tbfileName.Text + ".csv";
                if (index != -1)
                {
                    MyBook = MyApp.Workbooks.Add();
                    /* MyBook = MyApp.Workbooks.Open("C:/Users/ericp/Desktop/Bloc2Offline/C#/Labo/Projet2/LaboC#2.14/testExport");*/
                    MySheet = (Excel.Worksheet)MyBook.Sheets[1];
                    MySheet.Cells.ClearContents();

                    int i = 1;
                    foreach (ICoord c in type[index].lCoord)
                    {
                        MySheet.Cells[i, 1] = c.X;
                        MySheet.Cells[i, 2] = c.Y;
                        MySheet.Cells[i, 3] = c.Description;
                        i++;
                    }
                    if(type[index] is ProjectLibraryClass.Polygon)
                    {
                        MySheet.Cells[i, 1] = type[index].lCoord[0].X;
                        MySheet.Cells[i, 2] = type[index].lCoord[0].Y;
                        MySheet.Cells[i, 3] = type[index].lCoord[0].Description;
                    }
                    try
                    {
                        MyApp.DisplayAlerts = false;
                        if (File.Exists(filenameExport))
                        {
                            File.Delete(filenameExport);
                        }
                        MyBook.SaveAs(filenameExport);
                        MyApp.DisplayAlerts = true;
                        MyBook.Close();
                    }
                    catch (Exception f)
                    {
                        MyApp.Quit();

                        MainWindow.releaseObject(MyApp);
                        MainWindow.releaseObject(MyBook);
                        MainWindow.releaseObject(MyApp);
                        MessageBox.Show("Error export : " + f.Message);
                        Close();
                    }
                    MyApp.Quit();

                    MainWindow.releaseObject(MyApp);
                    MainWindow.releaseObject(MyBook);
                    MainWindow.releaseObject(MyApp);
                    MessageBox.Show("Export succès !", "Succes", MessageBoxButton.OK);
                    Close();

                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un Item", "Erreur", MessageBoxButton.OK);

                }
            }
        }
    }
}

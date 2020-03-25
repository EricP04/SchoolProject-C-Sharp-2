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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using ProjectLibraryClass;
using System.Runtime.Serialization.Formatters.Binary;
namespace ApplicationWPF
{
    /// <summary>
    /// Logique d'interaction pour WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        public WindowLogin()
        {
            this.SizeToContent = SizeToContent.Height;
            InitializeComponent();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(tbEmail.Text) || string.IsNullOrEmpty(tbNom.Text) || string.IsNullOrEmpty(tbPrenom.Text))
            {
                tbBadSaisie.Text = "Veuillez saisir tous les champs !";
            }
            else
            {
                ///On va vérifier le format d'email
                if (!IsValidEmail(tbEmail.Text))
                    tbBadSaisie.Text = "Mauvais format d'adresse mail";
                else
                {
                    ///Tout est bon
                    MyPersonalMapData mp = new MyPersonalMapData(tbNom.Text,tbPrenom.Text,tbEmail.Text);
                    /// On regarde si un fichier existe
                    if(File.Exists(tbPrenom.Text + tbNom.Text + ".txt"))
                        {
                        Console.WriteLine("FICHIER EXISTE");
                        ///Le fichier existe, on va importer
                        Stream s = new FileStream(tbPrenom.Text + tbNom.Text + ".txt", FileMode.Open, FileAccess.Read);
                        IFormatter formatter = new BinaryFormatter();
                        ObservableCollection<ICartoObj> iCartoObjRead = (ObservableCollection<ICartoObj>)formatter.Deserialize(s);
                        foreach(ICartoObj carto in iCartoObjRead)
                        {
                            mp.AddCartObj(carto);
                        }
                        s.Close();
                    }
                    Console.WriteLine("ON CREE MAINWINDOW");
                    MainWindow mw = new MainWindow(mp);
                    mw.Show();
                    Close();

                }
            }
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

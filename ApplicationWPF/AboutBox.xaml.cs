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
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection;
namespace ApplicationWPF
{
    /// <summary>
    /// Logique d'interaction pour AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            InitializeComponent();
            Assembly asm = Assembly.GetExecutingAssembly();
            tbCopyright.Text = "Copyright : " + ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            tbVersion.Text = "Version : " + ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;
            tbNomProjet.Text = "Nom du projet : " + ((AssemblyTitleAttribute)asm.GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title;
            tbDescription.Text = ((AssemblyDescriptionAttribute)asm.GetCustomAttribute(typeof(AssemblyDescriptionAttribute))).Description;
            tbSociete.Text = ((AssemblyCompanyAttribute)asm.GetCustomAttribute(typeof(AssemblyCompanyAttribute))).Company;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

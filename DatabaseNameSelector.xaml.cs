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

namespace TestMakerWPF_FirstInstance
{
    /// <summary>
    /// Логика взаимодействия для DatabaseNameSelector.xaml
    /// </summary>
    public partial class DatabaseNameSelector : Window
    {
        public DatabaseNameSelector()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if(MainWindow.DataBases.ContainsKey(TextBox.Text))
                {
                    MessageBoxResult error = MessageBox.Show("Database with the same name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataBaseWindow dbWindow = new DataBaseWindow();
                this.Close();
                dbWindow.InitializeDatabase(TextBox.Text);
                dbWindow.ShowDialog();
      
            }
        }

    }
}

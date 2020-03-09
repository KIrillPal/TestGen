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
using System.Data;

namespace TestMakerWPF_FirstInstance
{
    /// <summary>
    /// Логика взаимодействия для DataBase.xaml
    /// </summary>
    public partial class DataBaseWindow : Window
    {
        DataBase targetDB;
        public DataBaseWindow()
        {
            InitializeComponent();
        }

        public void InitializeDatabase(string dbname)
        {
            DataBaseLabel.Content = dbname;
            if(!MainWindow.DataBases.ContainsKey(dbname))
            {
                MainWindow.DataBases.Add(dbname, null);
            }
            targetDB = MainWindow.DataBases[dbname];
            if(targetDB == null)
            {
                targetDB = new DataBase();
                MainWindow.DataBases[dbname] = targetDB;
            }
            var dataTable = new DataTable();
            for (int i = 0; i < targetDB.ColumnHeaders.Count; ++i)
            {
                dataTable.Columns.Add(i.ToString(), typeof(string)); // columnheader used for size
            }
            var headerRow = dataTable.NewRow();
            for(int i = 0; i < targetDB.ColumnHeaders.Count; ++i)
            {
                headerRow[i] = targetDB.ColumnHeaders[i];
            }
          
            dataTable.Rows.Add(headerRow);

            for (int i = 0; i < targetDB.Data.Count; ++i)
            {
                var newRow = dataTable.NewRow();
                for(int j = 0; j < targetDB.Data[i].Count; ++j)
                {
                    newRow[j] = targetDB.Data[i][j];
                }
                dataTable.Rows.Add(newRow);
            }
           
            DataGrid.ItemsSource = dataTable.DefaultView;
        }

        public void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var row = e.Row;
            if(row.GetIndex() == 0)
            {
                row.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = ((DataView)DataGrid.ItemsSource).Table;
            targetDB.ColumnHeaders = new List<string>();
            targetDB.Data = new List<List<string>>();
            for(int i = 0; i < dataTable.Rows[0].ItemArray.Length; ++i)
            {
                targetDB.ColumnHeaders.Add((string)dataTable.Rows[0][i]);
            }
            for (int i = 1; i < dataTable.Rows.Count; ++i)
            {
                var Row = dataTable.Rows[i];
                targetDB.Data.Add(new List<string>());
                for (int j = 0; j < dataTable.Rows[i].ItemArray.Length; ++j)
                {
                    if(Row[j].GetType() == typeof(System.DBNull))
                    {
                        continue;
                    }
                    targetDB.Data[i - 1].Add((string)Row[j]);
                }
            }
            targetDB.Name = (string)DataBaseLabel.Content;
            MainWindow.DataBases[(string)DataBaseLabel.Content] = targetDB;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var curdataTable = ((DataView)DataGrid.ItemsSource).Table;
            curdataTable.Columns.Add(curdataTable.Columns.Count.ToString(), typeof(string)); // columnheader used for size
            var headerRow = curdataTable.Rows[0];
            headerRow[headerRow.ItemArray.Length - 1] = "newColumn";
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = curdataTable.DefaultView;
            DataGrid.Items.Refresh();
        }

        private void DataGrid_DragOver(object sender, DragEventArgs e)
        {

        }
    }
}

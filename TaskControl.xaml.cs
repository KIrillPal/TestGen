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
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : Window
    {
        Task targetTask;
        TaskView targetTaskView;
        public TaskControl(Task task, TaskView taskView)
        {
            targetTask = task;
            targetTaskView = taskView;
            InitializeComponent();

            DB_Dropdown.ItemsSource = MainWindow.DataBases.Keys;
            if(targetTask.DB != null)
                DB_Dropdown.SelectedItem = targetTask.DB.Name;
            TitleTB.Text = task.Title;
            TaskTestTB.Document.Blocks.Add(new Paragraph(new Run(task.Text)));
            AnswerFieldTB.Text = task.AnswerField;
        }


        private void ApplyB_Click(object sender, RoutedEventArgs e)
        {
            targetTask.Title = TitleTB.Text;
            targetTask.Text = new TextRange(TaskTestTB.Document.ContentStart, TaskTestTB.Document.ContentEnd).Text;
            targetTask.AnswerField = AnswerFieldTB.Text;
            if (DB_Dropdown.Text.Length == 0)
            {
                MessageBox.Show("Database is not selected, expect build errors!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                targetTask.DB = MainWindow.DataBases[DB_Dropdown.Text];
            }
            targetTaskView.TitleLabel.Content = targetTask.Title;
            this.Close();
        }
    }
}

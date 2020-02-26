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

            TitleTB.Text = task.Title;
            TaskTestTB.Text = task.Text;
            AnswerFieldTB.Text = task.AnswerField;
        }


        private void ApplyB_Click(object sender, RoutedEventArgs e)
        {
            targetTask.Title = TitleTB.Text;
            targetTask.Text = TaskTestTB.Text;
            targetTask.AnswerField = AnswerFieldTB.Text;
            targetTaskView.TitleLabel.Content = targetTask.Title;
            this.Close();
        }
    }
}

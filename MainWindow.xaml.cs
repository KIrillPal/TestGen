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
namespace TestMakerWPF_FirstInstance
{

public class Task
{
    public string Title;
    public string Text;
    public string AnswerField;
    public DataBase DB;

    public IPreview Previewer;
    public Task(string title, string text, string answer)
    {
        Title = title;
        Text = text;
        AnswerField = answer;
    }
}

public interface IPreview
{
    Control Preview();
}


public class DataBase
{
    public List<List<string>> Data;
    public List<string> ColumnHeaders;

    public DataBase()
    {
        Data = new List<List<string>> { new List<string>() { "smth", "smth" } };
        ColumnHeaders = new List<string>{ "newColumn", "anothernewColumn" };
    }
}


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public static List<Task> Tasks;
        public static Dictionary<string, DataBase> DataBases;
        public MainWindow()
        {
            InitializeComponent();
            Tasks = new List<Task>();
            DataBases = new Dictionary<string, DataBase>();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {

            Task newTask = new Task("Title", "Text", "AnswerField");
            TaskView newTaskB = new TaskView(newTask);

            newTaskB.EditButton.Click += (s,ev) =>
            {
                TaskControl TaskMenu = new TaskControl(newTask,newTaskB);
                TaskMenu.ShowDialog();
            };
            TaskStackPanel.Children.Add(newTaskB);
            TaskControl taskMenu = new TaskControl(newTask, newTaskB);

            Tasks.Add(newTask);
            taskMenu.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBaseWindow dbWindow = new DataBaseWindow();
            dbWindow.InitializeDatabase("SampleName");
            dbWindow.ShowDialog();
        }
    }
}

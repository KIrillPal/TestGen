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
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace TestMakerWPF_FirstInstance
{

public class Task
{
    public string Title;
    public string Text;
    public string AnswerField;
    public DataBase DB;

    public IAnswerGenerator answerGenerator;
    public IAnswerVisualizer answerVisualizer;

    public Task(string title, string text, string answer)
    {
        Title = title;
        Text = text;
        AnswerField = answer;
    }
}

public interface IAnswerGenerator
{
    object Generate(int selectedRow, Task task);
}

public interface IAnswerVisualizer
{
    void Visualize(Task task, object answer, XTextFormatterEx2 tf, XGraphics gfx, double endTextY);
}


public class TestAnswerVisualizer : IAnswerVisualizer
{
    public int Padding = 40;
    private readonly XFont ansFont = new XFont("Times New Roman", 16);
    public void Visualize(Task task, object answer, XTextFormatterEx2 tf, XGraphics gfx, double endTextY)
    {
        string[] unboxAns = (string[])answer;
        Random rng = new Random();
        unboxAns = unboxAns.OrderBy(x => rng.Next()).ToArray();
        for(int i = 0; i < unboxAns.Length; ++i)
        {
            XRect newRect = new XRect((gfx.PageSize.Width / unboxAns.Length) * i, endTextY + Padding, (gfx.PageSize.Width / unboxAns.Length), 0);
            tf.PrepareDrawString((char)(i + 65) + ")" + " " + unboxAns[i], ansFont, newRect, out int lastFChar, out double neededheight);
            newRect.Height = neededheight;
            tf.DrawString((char)(i + 65) + ")" + " " + unboxAns[i], ansFont, XBrushes.Black, newRect); 
        }
    }
}
public class TestAnswerGenerator : IAnswerGenerator
{
    // Principle: Correct answer is the first element
    public int answerCount = 4;
    public object Generate(int selectedRow, Task task)
    {
        if(task.DB.Data.Count < answerCount)
        {
            Console.Error.WriteLine("Not enough rows in database to generate answer!");
            return null;
        }
        string[] answers = new string[answerCount];
        int ansFieldIndex = task.DB.ColumnHeaders.IndexOf(task.AnswerField);
        if(ansFieldIndex == -1)
        {
                Console.Error.WriteLine("Cannot find answerfield!");
                return null;
        }
        answers[0] = task.DB.Data[selectedRow][ansFieldIndex];

        int[] usedIndexes = new int[answerCount];
        usedIndexes[0] = selectedRow;
        Random rng = new Random();
        for (int i = 1; i < answerCount; ++i)
        {
            int randInd = rng.Next(0, task.DB.Data.Count);
            while(usedIndexes.Contains(randInd))
            {
                randInd = rng.Next(0, task.DB.Data.Count);
            }
            usedIndexes[i] = randInd;
            answers[i] = task.DB.Data[randInd][ansFieldIndex]; 
        }
        return answers;
    }
}



public class DataBase
{
    public string Name;
    public List<List<string>> Data;
    public List<string> ColumnHeaders;

    public DataBase()
    {
        Data = new List<List<string>> { new List<string>() { "smth", "smth" } };
        ColumnHeaders = new List<string>{ "new11Column", "anothernewColumn" };
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
            DB_ListBox.ItemsSource = DataBases.Keys;
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

        private void DB_AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dbNameSelect = new DatabaseNameSelector();
            dbNameSelect.ShowDialog();
            DB_ListBox.Items.Refresh();
        }

        private void DB_ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = DB_ListBox.SelectedIndex;
            if(index != -1)
            {
                var DBWindow = new DataBaseWindow();
                DBWindow.InitializeDatabase((string)DB_ListBox.SelectedItem);
                DBWindow.ShowDialog();
            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument doc = null;
            try
            {
                doc = new PdfDocument("D:/pdftests/doc.pdf");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Seems like document is busy(it is opened in pdfviewer or processed by other programm)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatterEx2 tf = new XTextFormatterEx2(gfx);
            XFont TitleFont = new XFont("Times New Roman", 16, XFontStyle.Bold);
            XFont TaskTextFont = new XFont("Times New Roman", 12, XFontStyle.Regular);
            const int TitlePadding = 50;
            const int StartPadding = 20;
            const int TaskPadding = 30;
            PackedTask prevPT = default;
            for (int i = 0; i < Tasks.Count; ++i)
            {
                PackedTask PT = TaskPacker.PackTask(Tasks[i]);
                if (i == 0)
                {
                    XRect rect = new XRect(TitlePadding, StartPadding + TaskPadding, gfx.PageSize.Width - TitlePadding, 1000);
                    int lastfittingchar = 0;
                    double neededheight = 0;
                    tf.PrepareDrawString(PT.Text, TaskTextFont, rect, out lastfittingchar, out neededheight);
                    rect.Height = neededheight;
                    tf.DrawString(Tasks[i].Title, TitleFont, XBrushes.Black, new XRect(new XPoint(TitlePadding, StartPadding), gfx.MeasureString(Tasks[i].Title, TaskTextFont)));
                    tf.DrawString(PT.Text, TaskTextFont, XBrushes.Black, rect);
                    if (Tasks[i].answerVisualizer != null)
                    {
                        Tasks[i].answerVisualizer.Visualize(Tasks[i], PT.Answer, tf, gfx, StartPadding);
                    }
                } else
                {
                    XRect rect = new XRect(TitlePadding, StartPadding * (i * gfx.MeasureString(prevPT.Text, TaskTextFont).Height) + TaskPadding, gfx.PageSize.Width - TitlePadding, 1000);
                    int lastfittingchar = 0;
                    double neededheight = 0;
                    tf.PrepareDrawString(PT.Text, TaskTextFont, rect, out lastfittingchar, out neededheight);
                    rect.Height = neededheight;
                    tf.DrawString(Tasks[i].Title, TitleFont, XBrushes.Black, new XRect(new XPoint(TitlePadding, StartPadding * (i * gfx.MeasureString(prevPT.Text, TaskTextFont).Height)), gfx.MeasureString(Tasks[i].Title, TaskTextFont)));
                    tf.DrawString(PT.Text, TaskTextFont, XBrushes.Black, rect);
                    if (Tasks[i].answerVisualizer != null)
                    {
                        Tasks[i].answerVisualizer.Visualize(Tasks[i], PT.Answer, tf, gfx, (int)(StartPadding * (i * gfx.MeasureString(prevPT.Text, TaskTextFont).Height)));
                    }
                }
                prevPT = PT;
            }
            doc.Close();
        }
    }
}

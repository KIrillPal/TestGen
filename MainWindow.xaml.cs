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
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Microsoft.Win32;

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
    public int TextYPadding = 80;
    public int StartXPadding = 30;
    private readonly XFont ansFont = new XFont("Times New Roman", 16);
    public void Visualize(Task task, object answer, XTextFormatterEx2 tf, XGraphics gfx, double endTextY)
    {
        string[] unboxAns = (string[])answer;
        Random rng = new Random();
        unboxAns = unboxAns.OrderBy(x => rng.Next()).ToArray();
        for(int i = 0; i < unboxAns.Length; ++i)
        {
            XRect newRect = new XRect(StartXPadding + ((gfx.PageSize.Width - StartXPadding) / unboxAns.Length) * i, endTextY + TextYPadding, ((gfx.PageSize.Width - StartXPadding) / unboxAns.Length), 0);
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

public class LinkAnswerVisualizer : IAnswerVisualizer
{
    public int TextYPadding = 80;
    public int AnswerYPadding = 20;
    public int StartXPadding = 30;
    private readonly XFont ansFont = new XFont("Times New Roman", 16);
    public void Visualize(Task task, object answer, XTextFormatterEx2 tf, XGraphics gfx, double endTextY)
    {
        string[,] unboxAns = (string[,])answer;
        Random rng = new Random();
        for(int i = 0; i < unboxAns.GetLength(0); ++i)
        {
            for(int j = 1; j < unboxAns.GetLength(1); ++j)
            {
                if(rng.Next() <= rng.Next())
                {
                    string temp = unboxAns[i, j];
                    unboxAns[i, j] = unboxAns[i, j - 1];
                    unboxAns[i, j - 1] = temp;
                }
            }
        }
        for (int i = 0; i < unboxAns.GetLength(0); ++i)
        {
            for (int j = 0; j < unboxAns.GetLength(1); ++j)
            {
                XRect newRect = new XRect(StartXPadding + ((gfx.PageSize.Width - StartXPadding) / unboxAns.Length) * i, endTextY + TextYPadding + AnswerYPadding * j, ((gfx.PageSize.Width - StartXPadding) / unboxAns.Length), 0);
                tf.PrepareDrawString(unboxAns[i,j], ansFont, newRect, out int lastFChar, out double neededheight);
                newRect.Height = neededheight;
                tf.DrawString(unboxAns[i,j], ansFont, XBrushes.Black, newRect);
            }
        }
    }
}
public class LinkAnswerGenerator : IAnswerGenerator
{
    int answerCount = 4;
    public object Generate(int selectedRow, Task task)
    {
        string[] keyWords = task.AnswerField.Split(',').ToArray();
        if(keyWords.Length < 2)
        {
            Console.Error.WriteLine("Error! need two or more keywords!");
            return null;
        }

        int[] usedIndexes = new int[answerCount];
        usedIndexes[0] = selectedRow;
        string[,] answer = new string[keyWords.Length, answerCount];
        int[] answerFieldIndexes = new int[keyWords.Length];
        for(int i = 0; i < keyWords.Length; ++i)
        {
            answerFieldIndexes[i] = task.DB.ColumnHeaders.IndexOf(keyWords[i]);
        }
        Random rng = new Random();
        for (int i = 1; i < answerCount; ++i)
        {
            int randInd = rng.Next(0, task.DB.Data.Count);
            while (usedIndexes.Contains(randInd))
            {
                randInd = rng.Next(0, task.DB.Data.Count);
            }
            usedIndexes[i] = randInd;
        }
        for(int i = 0; i < keyWords.Length; ++i)
        {
            for (int j = 0; j < answerCount; ++j)
            {
                answer[i, j] = task.DB.Data[usedIndexes[j]][answerFieldIndexes[i]];
            }
        }
        return answer;
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
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Title = "Select file",
                DefaultExt = ".pdf",
                Filter = "PDF Documents (.pdf)|*.pdf"
            };


            if (fileDialog.ShowDialog() == true)
            {

                PdfDocument doc = null;
                try
                {
                    doc = new PdfDocument(fileDialog.FileName);
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Seems like document is busy(it is opened in pdfviewer or processed by other programm)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                PdfPage page = doc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatterEx2 tf = new XTextFormatterEx2(gfx);
                XFont TitleFont = new XFont("Times New Roman", 32, XFontStyle.Bold);
                XFont TaskTextFont = new XFont("Times New Roman", 12, XFontStyle.Regular);
                const int TitleXPadding = 50;
                const int StartYPadding = 40;
                const int TaskYPadding = 50;
                double prevYPos = 0;
                XRect rect;
                for (int i = 0; i < Tasks.Count; ++i)
                {
                    PackedTask PT = TaskPacker.PackTask(Tasks[i]);
                    if (i == 0)
                    {
                        rect = new XRect(TitleXPadding, StartYPadding + TaskYPadding, gfx.PageSize.Width - TitleXPadding, 1000);
                        int lastfittingchar = 0;
                        double neededheight = 0;
                        tf.PrepareDrawString(PT.Text, TaskTextFont, rect, out lastfittingchar, out neededheight);
                        rect.Height = neededheight;
                        tf.DrawString(Tasks[i].Title, TitleFont, XBrushes.Black, new XRect(new XPoint(TitleXPadding, StartYPadding), gfx.MeasureString(Tasks[i].Title, TaskTextFont)));
                        tf.DrawString(PT.Text, TaskTextFont, XBrushes.Black, rect);
                        if (Tasks[i].answerVisualizer != null)
                        {
                            Tasks[i].answerVisualizer.Visualize(Tasks[i], PT.Answer, tf, gfx, StartYPadding);
                        }
                    }
                    else
                    {
                        rect = new XRect(TitleXPadding, StartYPadding + prevYPos + TaskYPadding, gfx.PageSize.Width - TitleXPadding, 1000);
                        int lastfittingchar = 0;
                        double neededheight = 0;
                        tf.PrepareDrawString(PT.Text, TaskTextFont, rect, out lastfittingchar, out neededheight);
                        rect.Height = neededheight;
                        tf.DrawString(Tasks[i].Title, TitleFont, XBrushes.Black, new XRect(new XPoint(TitleXPadding, StartYPadding + prevYPos), gfx.MeasureString(Tasks[i].Title, TaskTextFont)));
                        tf.DrawString(PT.Text, TaskTextFont, XBrushes.Black, rect);
                        if (Tasks[i].answerVisualizer != null)
                        {
                            Tasks[i].answerVisualizer.Visualize(Tasks[i], PT.Answer, tf, gfx, (int)(StartYPadding + prevYPos));
                        }
                    }
                    prevYPos = rect.Bottom + (int)Tasks[i].answerVisualizer.GetType().GetField("TextYPadding").GetValue(Tasks[i].answerVisualizer);
                }
                doc.Close();
            }
        }
    }
}

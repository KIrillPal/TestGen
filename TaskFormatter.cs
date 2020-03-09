using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Charting;
using PdfSharp.Drawing.Layout;

namespace TestMakerWPF_FirstInstance
{
    /// <summary>
    ///  TaskFormatter uses TaskText and DB, connected to it, to generate final text and answer, if needed
    /// </summary>
    
    public struct PackedTask
    {
        public string Text;
        public object Answer;
    }
    class TaskPacker
    {
        static public PackedTask PackTask(Task in_task)
        {
            var DB = in_task.DB;
            PackedTask result = new PackedTask();
            var resultString = in_task.Text;
            var keyWords = in_task.Text.Split('{', '}');
            var rng = new Random();
            var selectedRow = rng.Next(0, DB.Data.Count - 1);
            if (keyWords != null)
            {
                foreach (var word in keyWords)
                {
                    if(DB.ColumnHeaders.IndexOf(word) == -1)
                    {
                        continue;
                    }
                    var dictWord = DB.Data[selectedRow][DB.ColumnHeaders.IndexOf(word)];
                    resultString = resultString.Replace(word, dictWord);
                }
            }
            resultString = resultString.Replace("{", "");
            resultString = resultString.Replace("}", "");
            result.Text = resultString;
            if (in_task.answerGenerator != null)
            {
                result.Answer = in_task.answerGenerator.Generate(selectedRow, in_task);
            }
            return result;
        }

    }
}

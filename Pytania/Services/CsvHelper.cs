using CsvHelper;
using Pytania.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pytania.Services
{
    public class CsvHelper
    {
        private bool HasHeader;
        private string delimeter;
        public CsvHelper(bool HasHeader = true, string delimeter = ";") 
        { 
            this.delimeter= delimeter;
            this.HasHeader = HasHeader;
        }
        public async Task<List<QuestionsTemplate>?> CsvReader(StreamReader streamReader) 
        {
            if (this.HasHeader)
            {
                streamReader.ReadLine();
            }
            string? line;
            List<QuestionsTemplate>? ToBeReturned = new();
            while ((line= await streamReader.ReadLineAsync()) != null) 
            {
                ToBeReturned.Add(new QuestionsTemplate
                {
                    Index = Convert.ToInt32(line.Split(this.delimeter).ElementAt(0)),
                    Question = line.Split(this.delimeter).ElementAt(1),
                    Answer = line.Split(this.delimeter).ElementAt(2)
                });
            }
            if (ToBeReturned.Any()) return ToBeReturned; return null;
        }

        public async Task<bool> CsvWriter(StreamWriter streamWriter, List<QuestionsTemplate> values, List<string>? Headers = null)
        {
            try
            {
                if (Headers != null)
                    streamWriter.WriteLine(Headers);
                foreach (QuestionsTemplate question in values)
                {
                    await streamWriter.WriteLineAsync($"{question.Index.ToString()}{this.delimeter}{question.Question}{this.delimeter}{question.Answer}");
                }
                return true;
            } 
            catch(Exception ex)
            {
                return false;
            }
        } 
    }
}

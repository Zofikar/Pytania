using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using Pytania.Models;
using System.Globalization;

namespace Pytania.Services
{
    public class Quiz
    {
        private string File;
        private CsvConfiguration csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter=";" };
        public Quiz(string File) 
        {
            this.File = File;
        }

        private QuestionsTemplate GetRandom()
        {
            List<QuestionsTemplate> records;
            using (FileStream fs = new FileStream(this.File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs))
            using (var csv = new CsvReader(reader, csvconfig))
            {
                records = csv.GetRecords<QuestionsTemplate>().ToList();
            }
            int randomNumer = new Random().Next(0, records.Count);
            QuestionsTemplate question = records.ElementAt(randomNumer);
            records.RemoveAt(randomNumer);
            Task.Run(() => 
            {
                using (FileStream fs = new FileStream(this.File, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writter = new StreamWriter(fs))
                using (var csv = new CsvWriter(writter, csvconfig))
                {
                    csv.WriteRecords(records);
                }
            });
            return question;
        }

        public async Task<QuestionsTemplate> GetRandomQuestion()
        {
            return await Task.Run(GetRandom);
        }
    }
}

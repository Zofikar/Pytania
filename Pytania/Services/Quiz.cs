using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using Pytania.Models;
using System.Text;
using System.Globalization;

namespace Pytania.Services
{
    public class Quiz
    {
        private string FilePath;
        private Random rnd;
        public Quiz(string FilePath) 
        {
            this.FilePath = FilePath;
            Load();
            rnd= new Random();
            foreach(QuestionsTemplate qst in questions)
            {
                questionsSave.Add(qst);
            }
        }

        private List<QuestionsTemplate> questions = new();
        private List<QuestionsTemplate> questionsSave = new();

        public int QuestionsLeft
        {
            get { return questions.Count; }
        }

        public async void Reload()
        {
            questions = new List<QuestionsTemplate>();
            using (FileStream fs = new FileStream(this.FilePath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writter = new StreamWriter(fs, Encoding.UTF8))
            using (var csv = new CsvWriter(writter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter = ";" }))
            {
                foreach (QuestionsTemplate question in questionsSave)
                {
                    csv.WriteRecord(question);
                    csv.NextRecord();
                    questions.Add(question);
                }
            }
        }

        public async void Delete()
        {
            File.Delete(this.FilePath);
        }


        private Task Load()
        {
            using (FileStream fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter = ";" }))
            {
                questions = csv.GetRecords<QuestionsTemplate>().ToList();
            }
            return Task.CompletedTask;
        }

        private async Task<QuestionsTemplate> GetRandom()
        {
            if(QuestionsLeft <= 0)
                return new QuestionsTemplate()
                {
                    Question = " ",
                    Index = 0,
                    Answer = " "
                };
            int randomNumer = rnd.Next(0, QuestionsLeft);
            QuestionsTemplate question = questions.ElementAt(randomNumer);
            questions.RemoveAt(randomNumer);
            return question;
        }

        private Task Save()
        {
            using (FileStream fs = new FileStream(this.FilePath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writter = new StreamWriter(fs, Encoding.UTF8))
            using (var csv = new CsvWriter(writter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter = ";" }))
            {
                foreach(QuestionsTemplate question in questions) 
                {
                    csv.WriteRecord(question);
                    csv.NextRecord();
                }
            }
            return Task.CompletedTask;
        }

        public async Task<QuestionsTemplate> GetRandomQuestion()
        {
            return await Task.Run(GetRandom);
        }

        public async Task ReloadQuestions()
        {
            await Task.Run(Load);
        }

        public async Task SaveQuestions()
        {
            await Task.Run(Save);
        }
    }
}

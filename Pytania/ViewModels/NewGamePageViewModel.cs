using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using Pytania.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pytania.ViewModels
{
    public partial class NewGamePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        string fileName = "Wybierz plik z pytaniami";

        [ObservableProperty]
        string filePath = null;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SaveFileName))]
        string gameName = null;

        [ObservableProperty]
        string separator = Separators.SemiColon;

        [ObservableProperty]
        List<string> header = new() { Headers.Question, Headers.Answer , Headers.Null};

        [ObservableProperty]
        bool hasHeader = true;

        string savesDir = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "saves");

        string SaveFileName => this.GameName + ".csv";

        [ObservableProperty]
        string[] headerslist = new string[4] { Headers.Index, Headers.Question, Headers.Answer, Headers.Null};

        class Separators
        {
            public const string Coma = ",";
            public const string Tab = "\t";
            public const string SemiColon = ";";
            public const string Colon = ":";
            public const string Space = " ";
            public const string Pipe = "|";
        }

        class Headers
        {
            public const string Question = "Pytanie";
            public const string Answer = "Odpowiedź";
            public const string Index = "Liczba porządkowa";
            public const string Null = "Ignoruj";
        }

        private bool isDigitOnly(string str)
        {
            foreach (char c in str)
            {
                if ((int)c < (int)'0' || (int)c > (int)'9')
                    return false;
            }
            return true;
        }

        private async Task<bool> ValidateFile()
        {
            List<string> records;
            bool separetorError = true;
            bool questionError = true;
            bool indexError = true;
            using (FileStream fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs))
            {
                if (this.HasHeader) reader.ReadLine();
                records = reader.ReadLine().Split(this.Separator).ToList();
                //await Shell.Current.DisplayAlert("ok",records.ToString(), "OK");
                if (records.Count < 2)
                {
                    separetorError = true;
                }
                else
                {
                    separetorError = false;
                }
                if (records.Count > 0 && Header.ElementAt(0) == Headers.Question && !records.ElementAt(0).Contains("?"))
                {
                    questionError = true;
                }
                else if (records.Count > 1 && Header.ElementAt(1) == Headers.Question && !records.ElementAt(1).Contains("?"))
                {
                    questionError = true;
                }
                else if (records.Count > 2 && Header.ElementAt(2) == Headers.Question && !records.ElementAt(2).Contains("?"))
                {
                    questionError = true;
                }
                else
                {
                    questionError = false;
                }
                if (records.Count > 0 && Header.ElementAt(0) == Headers.Index && !isDigitOnly(records.ElementAt(0)))
                {
                    indexError = true;
                }
                else if (records.Count > 1 && Header.ElementAt(1) == Headers.Index && !isDigitOnly(records.ElementAt(1)))
                {
                    indexError = true;
                }
                else if (records.Count > 2 && Header.ElementAt(2) == Headers.Index && !isDigitOnly(records.ElementAt(2)))
                {
                    indexError = true;
                }
                else
                {
                    indexError = false;
                }
            }
            if (questionError || separetorError || indexError)
                return await Shell.Current.DisplayAlert("Błąd", String.Join(separetorError ? "Upewnij się że separator się zgadza!\nUpewnij się że kolumny się zgadzają!(Ilość się nie zgadza)\n" : null, questionError ? "Upewnij się że kolumny się zgadzają!(Brak znaku '?' w pytaniu)\n" : null, indexError ? "Upewnij się że kolumny się zgadzają!(Lp nie jest liczbą)" : null), "Ignoruj", "OK");
            return true;
        }

        private async Task ParseCsv()
        {
            List<QuestionsTemplate> templates = new();
            QuestionsTemplate temp;
            int lastIndex = 0;
            using (FileStream fs = new(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new(fs))
            {
                string line;
                if (HasHeader)
                {
                    line = reader.ReadLine();
                }
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    temp = new QuestionsTemplate() { Index = null , Answer = null};
                    if (line.Trim() != null)
                    {
                        for (int i = 0; i < Header.Count; i++)
                        {
                            switch (Header.ElementAt(i))
                            {
                                case Headers.Index:
                                    temp.Index = Convert.ToInt32(line.Split(this.Separator).ElementAt(i).Replace("\"\"", @"\\\").Replace('"', ' ').Replace(@"\\\", "\""));
                                    break;
                                case Headers.Question:
                                    temp.Question = line.Split(this.Separator).ElementAt(i).Replace("\"\"", @"\\\").Replace('"', ' ').Replace(@"\\\", "\"");
                        break;
                                case Headers.Answer:
                                    temp.Answer = line.Split(this.Separator).ElementAt(i).Replace("\"\"", @"\\\").Replace('"', ' ').Replace(@"\\\", "\"");
                        break;
                                default: break;
                            }
                        }
                    }

                    lastIndex++;
                    if (temp.Index == null)
                        temp.Index = lastIndex;
                    templates.Add(temp);
                }
            }
            using (FileStream fs = new FileStream(System.IO.Path.Combine(this.savesDir, this.SaveFileName), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writter = new StreamWriter(fs, Encoding.UTF8))
            using (var csv = new CsvWriter(writter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter=";"}))
            {
                csv.WriteRecords(templates);
            }
            try
            {
                await Shell.Current.GoToAsync("GamePage", true, new Dictionary<string, object>
            {
                {"GameFile", new FileNamesConnection(this.FilePath)},
            });
            } catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        [RelayCommand]
        public async Task SelectFile() 
        {
            if(this.IsBusy) return;
            this.IsBusy = true;
            try
            {
                var csvFile = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".csv" } }
                });
#nullable enable
                FileResult? resoult = await FilePicker.PickAsync(new PickOptions()
#nullable disable
                {
                    FileTypes = csvFile,
                    PickerTitle = "Wybierz plik z pytaniami"
                });
                if (resoult == null)
                {
                    await Toast.Make("Nie wybrano pliku").Show();
                    this.IsBusy = false;
                    return;
                }
                this.FilePath = resoult.FullPath;
                this.FileName = resoult.FileName;
                await Toast.Make($"Wybrano plik {this.FileName}").Show();
            }
            catch
            {
                await Toast.Make("Nieznany błąd").Show();
            }
            this.IsBusy = false;
        }

        [RelayCommand]
        public async Task CreateNewQuiz()
        {
            if (this.IsBusy) return;
            this.IsBusy = true;
            Task<bool> validate = this.ValidateFile();
            if (this.FilePath == null)
            {
                await Toast.Make("Nie wybrano pliku").Show();
                this.IsBusy = false;
                return;
            }
            if (this.GameName == null)
            {
                await Toast.Make("Nie nadano grze nazwy").Show();
                this.IsBusy = false;
                return;
            }
            if (!await validate)
            {
                await Toast.Make("Wykryto błąd podczas weryfikacji pliku").Show();
                this.IsBusy = false;
                return;
            }
            if (!Directory.Exists(this.savesDir))
                Directory.CreateDirectory(this.savesDir);
            if (File.Exists(Path.Combine(this.savesDir, this.SaveFileName)))
            {
                if (!await Shell.Current.DisplayAlert("Błąd", "Gra o tej nazwie już istnieje czy napewno chcesz ją nadpisać?", "Tak", "Nie"))
                {
                    this.IsBusy=false;
                    return;
                }
            }
            await ParseCsv();
            this.IsBusy = false;
        }
    }
}

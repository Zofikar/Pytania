using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pytania.Models;
using Pytania.Services;
using System.Timers;

namespace Pytania.ViewModels
{
    [QueryProperty(nameof(GameFile), "GameFile")]
    public partial class GamePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MyQuiz))]
        FileNamesConnection gameFile;

        private System.Timers.Timer waitBefore;

        private System.Timers.Timer actualCloack;
        public Quiz MyQuiz { get; set; }

        [ObservableProperty]
        string formatedTime;

        [ObservableProperty]
        bool clock;

        [ObservableProperty]
        private int sekundy = 0;


        partial void OnClockChanged(bool value)
        {
            if (value)
            {
                waitBefore.Stop();
                actualCloack.Start();
            }
            else
            {
                this.Sekundy = 0;
                actualCloack.Stop();
            }
        }

        partial void OnGameFileChanged(FileNamesConnection value)
        {
            if (value != null)
                this.MyQuiz = new(value.FilePath);
        }


        [ObservableProperty]
        QuestionsTemplate question = new()
        {
            Question = " ",
            Index = 0,
            Answer = " "
        };

        [ObservableProperty]
        int questionsLeft;

        [ObservableProperty]
        string buttonText = "Rozpocznij Quiz";

        [ObservableProperty]
        bool isAnswerVisible = false;

        [ObservableProperty]
        bool isAutoSave = true;

        private bool Saved = true;

        [ObservableProperty]
        bool isQuestionVisible = false;

        partial void OnSekundyChanged(int value)
        {
            this.FormatedTime = sek2str(value);
        }

        private string sek2str(int value)
        {
            return String.Format("{0:D2}:{1:D2}", (int)value / 60, value % 60);
        }

        public void TimeChange(Object source, ElapsedEventArgs e)
        {
            Sekundy++;
        }

        partial void OnQuestionChanged(QuestionsTemplate value)
        {
            if (this.MyQuiz != null)
            {
                this.QuestionsLeft = this.MyQuiz.QuestionsLeft;
                this.IsQuestionVisible = true;
                this.Clock = false;
            }
        }
        public GamePageViewModel() 
        {
            waitBefore = new System.Timers.Timer(5000);
            waitBefore.Elapsed += ((Object source, ElapsedEventArgs e) => this.Clock = true);
            actualCloack = new System.Timers.Timer(1000);
            actualCloack.Elapsed += TimeChange;
            actualCloack.AutoReset= true;
        }

        private async Task NextQuestion()
        {
            this.IsAnswerVisible = false;
            this.Question = await this.MyQuiz.GetRandomQuestion();
            this.ButtonText = "Pokaż odpowiedź";
            if (this.IsAutoSave)
            {
                await this.MyQuiz.SaveQuestions();
                Saved = true;
            }
            else
            {
                Saved = false;
            }
            waitBefore.Start();
        }

        private void ShowAnswer()
        {
            this.IsAnswerVisible = true;
            this.ButtonText = "Następne Pytanie";
            this.actualCloack.Stop();
        }

        [RelayCommand]
        public async Task ButtonClicked()
        {
            if(IsBusy) return;
            IsBusy= true;
            if (this.ButtonText == "Rozpocznij Quiz" || this.ButtonText == "Następne Pytanie")
                await NextQuestion();
            else if (this.ButtonText == "Zakończ")
            {
                if (await Shell.Current.DisplayAlert("Koniec!", $"Gra {GameFile.FileNameWithoutExtension} zakończyła się!\nCzy chcesz przywrócić zapis z początku tej sesji?", "Tak", "Nie"))
                {
                    this.MyQuiz.Reload();
                }
                else
                {
                    this.MyQuiz.Delete();
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                ShowAnswer();
                if (this.QuestionsLeft == 0)
                    this.ButtonText = "Zakończ";
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task Save()
        {
            await MyQuiz.SaveQuestions();
            this.Saved= true;
        }

        [RelayCommand]
        public async Task Return()
        {
            if (!this.Saved)
                if (await Shell.Current.DisplayAlert("Uwaga!", "Nie zapisano zmian.\nCzy chcesz je zapisać?", "Tak", "Nie"))
                {
                    await MyQuiz.SaveQuestions();
                }
            await Shell.Current.GoToAsync("..");
        }
    }
}

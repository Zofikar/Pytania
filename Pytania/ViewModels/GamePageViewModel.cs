using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pytania.Models;
using Pytania.Services;
using Pytania.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pytania.ViewModels
{
    [QueryProperty(nameof(GameFile), "GameFile")]
    public partial class GamePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MyQuiz))]
        FileNamesConnection gameFile;
        public Quiz MyQuiz { get; set; }


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

        partial void OnQuestionChanged(QuestionsTemplate value)
        {
            if (this.MyQuiz != null)
                this.QuestionsLeft = this.MyQuiz.QuestionsLeft;
        }
        public GamePageViewModel() 
        {

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
        }

        private void ShowAnswer()
        {
            this.IsAnswerVisible = true;
            this.ButtonText = "Następne Pytanie";
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

using CommunityToolkit.Mvvm.ComponentModel;
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
        public Quiz MyQuiz => new(this.GameFile.FilePath);


        public GamePageViewModel() 
        {
        }
    }
}

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Pytania.Models;
using Pytania.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pytania.ViewModels
{
    public partial class LoadPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        List<FileNamesConnection> gameNames = new();

        [ObservableProperty]
        FileNamesConnection gameName = null;


        private string savesDir = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "saves");

        public LoadPageViewModel()
        {
            this.IsBusy = true;
            savesDir = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "saves");
            this.GameNames = new List<FileNamesConnection>();
            if (!Directory.Exists(savesDir))
            {
                Directory.CreateDirectory(savesDir);
            }
            else if (Directory.EnumerateFiles(savesDir).Any())
            {
                foreach (string name in Directory.GetFiles(savesDir, "*.csv"))
                {
                    this.GameNames.Add(new FileNamesConnection(name));
                }
            }
            this.IsBusy = false;
        }

        [RelayCommand]
        public async Task Load()
        {
            if (this.IsBusy)
            {
                return;
            }
            this.IsBusy = true;
            if (this.GameName == null) 
            {
                await Toast.Make("Wybierz plik gry").Show();
                this.IsBusy = false;
                return;
            }
            else if (!this.GameNames.Contains(this.GameName))
            {
                await Toast.Make("Wybierz poprawny plik gry").Show();
                this.IsBusy = false;
                return;
            }
            this.IsBusy = false;
            await Shell.Current.GoToAsync("GamePage", true, new Dictionary<string, object>
            {
                {"GameFile", this.GameName},
            });
        }
    }
}

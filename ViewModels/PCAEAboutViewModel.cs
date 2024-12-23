using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notes.ViewModels
{
    internal class PCAEAboutViewModel
    {
        public string Title => AppInfo.Name;
        public string Version => AppInfo.VersionString;
        public string MoreInfoUrl => "https://aka.ms/maui";
        public string Message => "Esta app es creada por PABLO CRIOLLO ESCOBAR ";
        public ICommand ShowMoreInfoCommand { get; }

        public PCAEAboutViewModel()
        {
            ShowMoreInfoCommand = new AsyncRelayCommand(ShowMoreInfo);
        }


        async Task ShowMoreInfo() =>
            await Launcher.Default.OpenAsync(MoreInfoUrl);
    }
}

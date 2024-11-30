using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace nutrIApp.de.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    [RelayCommand]
    public async Task Navigate()
    {
        await Shell.Current.GoToAsync("recipepage");
    }
}

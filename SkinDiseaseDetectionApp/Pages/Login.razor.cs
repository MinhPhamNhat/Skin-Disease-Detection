
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;

namespace SkinDiseaseDetectionApp.Pages;

public partial class Login
{

    [Inject]
    public IJSRuntime _jSRuntime { get; set; }

    [Inject]
    public ISkinDetectionClient _skinDetectionClient { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    bool success;
    string[] errors = { };

    MudTextField<string> pwField1;
    MudForm form;
    string Email { get; set; }
    string Password { get; set; }


    private async Task OnLogin()
    {
        var user = await _skinDetectionClient.Get<UserDto>($"{UrlPath.Login}?username={Email}&password={Password}&");
        if (user != null)
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "userId", user.Id);
            NavigationManager.NavigateTo("/", forceLoad: true);
            success = true;
        }
        else
        {
            success = false;
            errors = new string[] { "Invalid username or password" };
        }
    }
}
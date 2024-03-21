
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SkinDiseaseDetectionApp.Constants;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;

namespace SkinDiseaseDetectionApp.Pages;

public partial class Register
{
    bool success;
    string[] errors = { };
    MudTextField<string> pwField1;
    MudForm form;
    [Inject]
    public IJSRuntime _jSRuntime { get; set; }

    [Inject]
    public ISkinDetectionClient _skinDetectionClient { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    private UserDto user = new UserDto();

    private IEnumerable<string> PasswordStrength(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Password is required!";
            yield break;
        }
        if (pw.Length < 8)
            yield return "Password must be at least of length 8";
        if (!Regex.IsMatch(pw, @"[A-Z]"))
            yield return "Password must contain at least one capital letter";
        if (!Regex.IsMatch(pw, @"[a-z]"))
            yield return "Password must contain at least one lowercase letter";
        if (!Regex.IsMatch(pw, @"[0-9]"))
            yield return "Password must contain at least one digit";
    }

    private string PasswordMatch(string arg)
    {
        if (pwField1.Value != arg)
            return "Passwords don't match";
        return null;
    }

    private async Task OnRegister()
    {
        var result = await _skinDetectionClient.Post($"{UrlPath.Register}", new Dictionary<string, string>()
        {
            { "Email", user.Email },
            { "Password", user.Password },
            { "FullName", user.FullName }
        });
        NavigationManager.NavigateTo("/login");
    }
}
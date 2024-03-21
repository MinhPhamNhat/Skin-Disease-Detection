
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SkinDiseaseDetectionApp.Constants;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;
using SkinDiseaseDetectionApp.Services.Interfaces;

namespace SkinDiseaseDetectionApp.Pages;

public partial class UserHistories
{

    [Inject]
    public ISkinDetectionClient _skinDetectionClient { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public IUserService userService { get; set; }

    private List<UserHistory> _UserHistories { get; set; } = new List<UserHistory>();
    public Dictionary<string, string> SkinDiseasesDictionary { get; set; }


    protected override async Task OnInitializedAsync()
    {
        SkinDiseasesDictionary = GetSkinDiseasesDictionary();
        var userId = await userService.GetUserId();
        _UserHistories = await _skinDetectionClient.Get<List<UserHistory>>($"{UrlPath.UserHistories}/{userId}");
    }
    public static Dictionary<string, string?> GetSkinDiseasesDictionary()
    {
        return typeof(SkinDiseases)
              .GetFields(BindingFlags.Public | BindingFlags.Static)
              .Where(f => f.FieldType == typeof(string))
              .ToDictionary(f => f.Name,
                            f => (string)f.GetValue(null));
    }
}
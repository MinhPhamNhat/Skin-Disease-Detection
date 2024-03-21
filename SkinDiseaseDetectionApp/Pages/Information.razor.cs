
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkinDiseaseDetectionApp.Constants;

namespace SkinDiseaseDetectionApp.Pages;

public partial class Information
{
    [Inject]
    public IJSRuntime _jSRuntime { get; set; }
    public Dictionary<string, string?> SkinDiseases;
    public Dictionary<string, (bool, string)> DiseaseState = new Dictionary<string, (bool, string)>();
    protected override void OnInitialized()
    {
        SkinDiseases = GetSkinDiseasesDictionary();
        DiseaseState = SkinDiseases.ToDictionary(x => x.Key, x => (false, ""));
        base.OnInitialized();
    }

    private async Task getOverview(string key)
    {
        var overview = await _jSRuntime.InvokeAsync<string>("generateContent", Prompt.Overview(SkinDiseases[key]));
        var a = DiseaseState[key];
        a.Item1 = true;
        a.Item2 = overview;
        DiseaseState[key] = a;
        StateHasChanged();
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
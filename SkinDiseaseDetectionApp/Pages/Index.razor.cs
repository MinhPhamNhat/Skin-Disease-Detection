
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SkinDiseaseDetectionApp.Constants;

namespace SkinDiseaseDetectionApp.Pages;

public partial class Index
{
    [Inject]
    public IJSRuntime _jSRuntime { get; set; }

    public bool IsFactLoading { get; set; }
    public string Fact { get; set; }
    public string Overview { get; set; }
    public Dictionary<string, string> SkinDiseasesDictionary { get; set; }
    public string SelectedDiseaseKey { get; set; }
    protected override void OnInitialized()
    {
        SkinDiseasesDictionary = GetSkinDiseasesDictionary();
        int randomIndex = new Random().Next(SkinDiseasesDictionary.Count);
        SelectedDiseaseKey = SkinDiseasesDictionary.ElementAt(randomIndex).Key;
        Series = SkinDiseasesDictionary.Take(5).Select(x => new ChartSeries()
        {
            Name = x.Value,
            Data = generateRandomNumber()
        }).ToList();

        base.OnInitialized();
    }

    public static Dictionary<string, string?> GetSkinDiseasesDictionary()
    {
        return typeof(SkinDiseases)
              .GetFields(BindingFlags.Public | BindingFlags.Static)
              .Where(f => f.FieldType == typeof(string))
              .ToDictionary(f => f.Name,
                            f => (string)f.GetValue(null));
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await generateFact();
            Overview = await _jSRuntime.InvokeAsync<string>("generateContent", Prompt.Overview(SkinDiseasesDictionary[SelectedDiseaseKey]));
        }
        await base.OnAfterRenderAsync(firstRender);
    }


    private async Task generateFact()
    {
        IsFactLoading = true;
        Fact = await _jSRuntime.InvokeAsync<string>("generateContent", Prompt.Fact);
        IsFactLoading = false;
        StateHasChanged();
    }

    private double[] generateRandomNumber()
    {
        List<double> randomNumbers = new List<double>();
        Random random = new Random();

        // Generate 23 random numbers
        for (int i = 0; i < 3; i++)
        {
            // Generate a random number between 30 and 100 (inclusive)
            double randomNumber = random.Next(30, 101);
            randomNumbers.Add(randomNumber);
        }

        return randomNumbers.ToArray();
    }

    private int Indx = -1; //default value cannot be 0 -> first selectedindex is 0.
    public ChartOptions Options = new ChartOptions();

    public List<ChartSeries> Series = new List<ChartSeries>();
    public string[] XAxisLabels = { "Model A", "Model B", "Model C" };
}
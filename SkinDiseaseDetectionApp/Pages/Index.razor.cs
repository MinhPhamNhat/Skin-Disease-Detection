
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using SkinDiseaseDetectionApp.Constants;
using SkinDiseaseDetectionApp.Dto;
using SkinDiseaseDetectionApp.HttpClients.Interfaces;

namespace SkinDiseaseDetectionApp.Pages;

public partial class Index
{
    [Inject]
    public IJSRuntime _jSRuntime { get; set; }
    [Inject]
    public ISkinDetectionClient _skinDetectionClient { get; set; }
    [Inject]
    public IDialogService DialogService { get; set; }

    public string Overview { get; set; }
    public Dictionary<string, string> SkinDiseasesDictionary { get; set; }
    public string SelectedDiseaseKey { get; set; }
    public string Result { get; set; }
    public string Base64Image { get; set; }
    public List<ChartSeries> Series = new List<ChartSeries>();
    public bool IsLoading = false;

    public IBrowserFile File { get; set; }
    public PredictionDto PredictionResult { get; set; }
    public string[] XAxisLabels = new string[] { };

    public List<string> ModelTypes { get; set; } = new List<string> {
        "vgg16",
        "resnet50",
        "efficientnetb0",
        "vgg16_resnet50_efficientnetb0"
     };

    public string SelectedModelType { get; set; }

    protected override void OnInitialized()
    {
        SkinDiseasesDictionary = GetSkinDiseasesDictionary();
        SelectedModelType = ModelTypes.First();
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



    private async Task OnInputFileChange(InputFileChangeEventArgs args)
    {
        var parameters = new DialogParameters()
        {
            { "File", args.File }
        };

        var dialog = await DialogService.ShowAsync<CropDialog>("Vui lòng chọn kích cỡ ảnh là 150x200", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            Base64Image = result.Data.ToString();
        }
    }

    private async Task OnDiagnosticButtonClick()
    {
        IsLoading = true;
        var image = Regex.Replace(Base64Image, @"^data:image\/\w+;base64,", "");
        PredictionResult = await _skinDetectionClient.Post<PredictionDto>($"/api/Diagnostic?modelType={SelectedModelType}", new Dictionary<string, string>() { { "Image", image } });

        SelectedDiseaseKey = PredictionResult.Result;
        Overview = await _jSRuntime.InvokeAsync<string>("generateContent", Prompt.Overview(SkinDiseasesDictionary[SelectedDiseaseKey]));
        Series = new List<ChartSeries>()
        {
            new ChartSeries() {
                Name = SelectedModelType, Data = new double[] {
                    PredictionResult.Predictions.akiec * 100,
                    PredictionResult.Predictions.bcc * 100,
                    PredictionResult.Predictions.bkl * 100,
                    PredictionResult.Predictions.df * 100,
                    PredictionResult.Predictions.mel * 100,
                    PredictionResult.Predictions.nv * 100,
                    PredictionResult.Predictions.vasc * 100,
                }
            }
        };
        XAxisLabels = new string[] {
            "akiec",
            "bcc",
            "bkl",
            "df",
            "mel",
            "nv",
            "vasc",
         };

        IsLoading = false;
        StateHasChanged();
    }

    private async Task OnContactClick()
    {
        var dialog = await DialogService.ShowAsync<SaveProfileDialog>("Vui lòng điền thông tin");
        var result = await dialog.Result;
    }
}
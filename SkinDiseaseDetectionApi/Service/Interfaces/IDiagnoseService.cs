using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkinDiseaseDetectionApi.Dto;

public interface IDiagnoseService
{
    Task<PredictionDto> DiagnoseSkinDisease(string modelType, PredictRequest request);
    Task<byte[]> GenerateReport();
}
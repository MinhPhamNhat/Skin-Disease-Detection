namespace SkinDiseaseDetectionApp.Constants;

public static class Prompt
{
    public static string Fact = "generate a fact about skin disease, this fact should only have 1 to 2 sentence in Vietnamese";
    public static string Overview(string disease) => $@"Overview about {disease} in Vietnamese";
    public static string OverviewName(string disease) => $@"{disease} in Vietnamese";
}
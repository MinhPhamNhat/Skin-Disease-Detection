using System;

namespace SkinDiseaseDetectionApp.Dto
{
    public class PredictionDto
    {
        public Prediction Predictions { get; set; }
        public string Result { get; set; }
    }

    public class Prediction
    {
        public double akiec { get; set; }
        public double bcc { get; set; }
        public double bkl { get; set; }
        public double df { get; set; }
        public double mel { get; set; }
        public double nv { get; set; }
        public double vasc { get; set; }
    }

    public class PredictRequest
    {
        public string Image { get; set; }
    }
}
using RunModel.Models;

namespace RunModel
{
    internal class Program
    {
        static string _inputModelPath = Path.Combine(Environment.CurrentDirectory, "Inputs/model.onnx");
        static string _inputLabelPath = Path.Combine(Environment.CurrentDirectory, "Inputs/labels.json");

        static void Main(string[] args)
        {
            // Init predictor
            var predictor = new Predictor(_inputModelPath);
            
            // Load input file and create prediction
            var input = new PenguinData { 
                BillLengthMM = 39.1f, 
                BillDepthMM = 18.7f, 
                FlipperLengthMM = 181.0f, 
                BodyMassG = 3750.0f 
            };

            // Predict
            var prediction = predictor.Predict(input);

            // Print prediction
            Console.WriteLine(prediction.GetName(_inputLabelPath));
            
            // It should output 'Adelie';
        }
    }
}

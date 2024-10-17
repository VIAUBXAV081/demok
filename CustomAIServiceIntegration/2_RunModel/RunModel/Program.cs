using RunModel.Models;

namespace RunModel
{
    internal class Program
    {
        static string _inputsFolderPath = Path.Combine(Environment.CurrentDirectory, "Inputs");
        static string _inputFilePath = Path.Combine(_inputsFolderPath, "DR_V01A.rmf");
        static string _inputModelPath = Path.Combine(_inputsFolderPath, "example_cnn_model.onnx");

        static void Main(string[] args)
        {
            // Init predictor
            var predictor = new Predictor(_inputModelPath);
            
            // Load input file and create prediction
            var input = RamanMap.FromFile(_inputFilePath);
            
            var prediction = predictor.Predict(input);

            // Print prediction
            foreach (var value in prediction.Curve)
            {
                Console.WriteLine(value.ToString());
            }
        }
    }
}

using Microsoft.ML;
using RunModel.Models;

namespace RunModel
{
    public class Predictor
    {
        public string ModelPath { get; }
        public string InputName { get; } = "input";
        public string OutputName { get; } = "output";

        public MLContext Context { get; }

        private Lazy<PredictionEngine<PenguinData, PenguinType>> PredictionEngine => new Lazy<PredictionEngine<PenguinData, PenguinType>>(InitPredictor);

        public Predictor(string modelPath)
        {
            Context = new MLContext(seed: 0);
            ModelPath = modelPath;
        }

        private PredictionEngine<PenguinData, PenguinType> InitPredictor()
        {

            var estimator = Context.Transforms.ApplyOnnxModel(outputColumnNames: [OutputName], inputColumnNames: [InputName], ModelPath);
           
            IEstimator<ITransformer> pipeline = Context.Transforms.CopyColumns(InputName, nameof(PenguinData.Data))
                .Append(estimator)
                .Append(Context.Transforms.CopyColumns(nameof(PenguinType.Type), OutputName));

            var dataview = Context.Data.LoadFromEnumerable(new List<PenguinData>() { });

            var transformer = pipeline.Fit(dataview);

            return Context.Model.CreatePredictionEngine<PenguinData, PenguinType>(transformer);
        }

        public PenguinType Predict(PenguinData input)
        {
            var engine = PredictionEngine.Value;
            
            return engine.Predict(input);
        }
    }
}

using Microsoft.ML;
using RunModel.Models;

namespace RunModel
{
    public class Predictor
    {
        public string ModelPath { get; }
        public string InputName { get; } = "raman_map";
        public string OutputName { get; } = "dissolution_curve";

        public MLContext Context { get; }

        private Lazy<PredictionEngine<RamanMap, DissolutionCurve>> PredictionEngine => new Lazy<PredictionEngine<RamanMap, DissolutionCurve>>(InitPredictor);

        public Predictor(string modelPath)
        {
            Context = new MLContext(seed: 0);
            ModelPath = modelPath;
        }

        private PredictionEngine<RamanMap, DissolutionCurve> InitPredictor()
        {

            var estimator = Context.Transforms.ApplyOnnxModel(outputColumnNames: [OutputName], inputColumnNames: [InputName], ModelPath);
           
            IEstimator<ITransformer> pipeline = Context.Transforms.CopyColumns(InputName, nameof(RamanMap.HPMC))
                .Append(estimator)
                .Append(Context.Transforms.CopyColumns(nameof(DissolutionCurve.Curve), OutputName));

            var dataview = Context.Data.LoadFromEnumerable(new List<RamanMap>() { });

            var transformer = pipeline.Fit(dataview);

            return Context.Model.CreatePredictionEngine<RamanMap, DissolutionCurve>(transformer);
        }

        public DissolutionCurve Predict(RamanMap input)
        {
            var engine = PredictionEngine.Value;
            
            return engine.Predict(input);
        }
    }
}

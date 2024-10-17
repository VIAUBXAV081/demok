using Microsoft.ML.Data;

namespace RunModel.Models
{
    public class DissolutionCurve
    {
        [ColumnName(@"Curve")]
        [VectorType(37)]
        public float[] Curve { get; set; }
    }
}

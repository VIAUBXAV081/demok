using Microsoft.ML.Data;

namespace RunModel.Models
{
    public class PenguinData
    {
        [VectorType(4)]
        [ColumnName(@"Data")]
        public float[] Data { get; set; }
    }
}

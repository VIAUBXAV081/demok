using Microsoft.ML.Data;

namespace RunModel.Models
{
    public class PenguinData
    {
        [VectorType(4)]
        [ColumnName(@"Data")]
        public float[] Data { get => [BillLengthMM, BillDepthMM, FlipperLengthMM, BodyMassG]; }

        public float BillLengthMM { get; set; }
        public float BillDepthMM { get; set; }
        public float FlipperLengthMM { get; set; }
        public float BodyMassG { get; set; }
    }
}

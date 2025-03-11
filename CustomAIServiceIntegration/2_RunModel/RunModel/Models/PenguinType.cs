using Microsoft.ML.Data;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RunModel.Models
{
    public class PenguinType
    {
        [VectorType(3)]
        [ColumnName(@"Type")]
        public float[] Type { get; set; }

        public string GetName(string labelNames = null)
        {
            var maxIndex = Type.ToList().IndexOf(Type.Max());

            if (labelNames == null)
            {
                return $"Penguin {maxIndex}";
            }
            else
            {
                string jsonString = File.ReadAllText(labelNames);
                var labels = JsonSerializer.Deserialize<Label>(jsonString);
                return labels.Species.Where(kvp => kvp.Value == maxIndex).First().Key;
            }
        }

        private class Label
        {
            [JsonPropertyName("species")]
            public Dictionary<string, int> Species { get; set; } = new();
        }

    }


}

using Microsoft.ML.Data;
using System.IO.Compression;

namespace RunModel.Models
{
    public class RamanMap
    {
        [VectorType(31 * 31)]
        [ColumnName(@"HPMC")]
        public float[] HPMC { get; set; }

        public static RamanMap FromFile(string path)
        {
            using (var file = File.OpenRead(path))
            using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    // Read only the HPMC map
                    if (entry.Name == "component-2.csv")
                    {
                        using (StreamReader sr = new StreamReader(entry.Open()))
                        {
                            var map = new float[31, 31];

                            for (int row = 0; row < 31; row++)
                            {
                                var line = sr.ReadLine();
                                var values = line.Split(',').Select(float.Parse).ToArray();
                                for (int col = 0; col < 31; col++)
                                {
                                    map[row, col] = values[col];
                                }
                            }
                            // We have to flatten map to be compatible with ML.Net
                            return new RamanMap() { HPMC = map.Cast<float>().ToArray() };
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}

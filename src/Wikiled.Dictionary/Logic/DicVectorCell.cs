using Wikiled.MachineLearning.Mathematics.Vectors;

namespace Wikiled.Dictionary.Logic
{
    public class DicVectorCell : ICell
    {
        public DicVectorCell(string text)
        {
            Name = text;
            Value = 1;
        }

        public string Name { get; }

        public double Value { get; }

        public object Clone()
        {
            return new DicVectorCell(Name);
        }
    }
}

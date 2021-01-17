using System;

namespace IISManager.Implementations
{
    public class DiagnosticValue : IEquatable<DiagnosticValue>
    {
        public DiagnosticValue(double value, string stringPattern)
        {
            Value = value;
            ValueFormatted = string.Format(stringPattern, value);
        }

        public double Value { get; }
        public string ValueFormatted { get; }

        public override string ToString()
        {
            return ValueFormatted;
        }

        public bool Equals(DiagnosticValue other)
        {
            return Math.Abs(this.Value - other.Value) < 0.01;
        }
    }
}

using System;

namespace IISManager.Implementations
{
    public class DiagnosticValue
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
    }
}

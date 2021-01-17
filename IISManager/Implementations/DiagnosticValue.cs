using System;

namespace IISManager.Implementations
{
    public class DiagnosticValue : IEquatable<DiagnosticValue>
    {
        private readonly double value;
        public DiagnosticValue(double value, double mediumThreshold, double highThreshold, string stringPattern)
        {
            this.value = value;
            if (value > highThreshold)
            {
                State = DiagnosticValueState.High;
            }
            else if (value > mediumThreshold)
            {
                State = DiagnosticValueState.Medium;
            }

            Value = string.Format(stringPattern, value);
        }

        public DiagnosticValueState State { get; }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }
        public bool Equals(DiagnosticValue other)
        {
            return Math.Abs(this.value - other.value) < 0.01;
        }
    }
}

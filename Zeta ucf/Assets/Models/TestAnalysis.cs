using System;

namespace Limping.Api.Models
{
    public class TestAnalysis
    {
        public Guid Id { get; set; }
        public Double EndValue { get; set; }
        public string Description { get; set; }
        public LimpingSeverityEnum LimpingSeverity { get; set; }
        public Guid LimpingTestId { get; set; }
        public virtual LimpingTest LimpingTest { get; set; }
    }

    public enum LimpingSeverityEnum
    {
        Low,
        Medium,
        High
    }
}

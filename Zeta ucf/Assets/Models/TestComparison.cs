using System;
using System.Collections.Generic;

namespace Limping.Api.Models
{
    public class TestComparison
    {
        public Guid Id { get; set; }
        public DateTime ComparisonDate { get; set; }
        public string ComparisonResults { get; set; }
        public virtual List<LimpingTestTestComparison> LimpingTestTestComparisons { get; set; }
    }
}

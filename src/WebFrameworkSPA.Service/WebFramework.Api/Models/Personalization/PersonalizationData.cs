using System.Collections.Generic;

namespace Web.Models
{
    public class PersonalizationData
    {
        public IEnumerable<FeatureItem> Features { get; set; }
        public UiClaimsData UiClaims { get; set; }
    }
}
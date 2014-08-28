using System.Collections.Generic;

namespace Web.Models
{
    public class UiClaimsData
    {
        public string UserName { get; set; }
        public List<string> Capabilities { get; set; }
        public List<Constraint> Constraints { get; set; }
        public List<NameValueClaim> NameValueClaims { get; set; }
    }


    public class Constraint
    {
        public string Name { get; set; }
    }

    public class NumericConstraint : Constraint
    {
        public double UpperLimit { get; set; }
        public double LowerLimit { get; set; }
    }


    public class NameValueClaim
    {
        public NameValueClaim()
        {
        }

        public NameValueClaim(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
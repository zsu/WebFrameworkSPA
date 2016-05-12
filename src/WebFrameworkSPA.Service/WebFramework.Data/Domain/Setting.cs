using System;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.Data.Domain
{
    public class Setting
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public virtual Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [Required]
        public virtual string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

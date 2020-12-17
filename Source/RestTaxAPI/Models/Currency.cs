namespace RestTaxAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A Currency based in the ISO 4217
    /// </summary>
    public record Currency
    {
        /// <summary>
        /// The ISO 4217 3-letter code for this currency
        /// </summary>
        /// <example>USD</example>
        [Required]
        public string Code { get; init; }

        /// <summary>
        /// The ISO 4217 Numeric code for this currency
        /// </summary>
        /// <example>840</example>
        public short Number { get; init; }

        /// <summary>
        /// The ISO 4217 Full currency name
        /// </summary>
        /// <example>United States dollar</example>
        [Required]
        public string Name { get; init; }
    }
}

namespace RestTaxAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Invoice Response data
    /// </summary>
    public record InvoiceResponse
    {
        /// <summary>
        /// The ISO 4217 3-letter code for the currency used for all values in this response
        /// </summary>
        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; init; }

        /// <summary>
        /// The Pre-Tax Total in cents in the returned Currency (Unit: Cents)
        /// </summary>
        [Required]
        [Range(0, long.MaxValue)]
        public long? PreTaxTotalInCents { get; init; }

        /// <summary>
        /// The Tax Amount in cents in the returned Currency (Unit: Cents)
        /// </summary>
        [Required]
        [Range(0, long.MaxValue)]
        public long? TaxAmountInCents { get; init; }

        /// <summary>
        /// The Grand Total in cents in the returned Currency (Unit: Cents)
        /// </summary>
        [Required]
        [Range(0, long.MaxValue)]
        public long? GrandTotalInCents { get; init; }

        /// <summary>
        /// The Exchange Rate used for the calculation
        /// </summary>
        [Required]
        [Range(0, long.MaxValue)]
        public decimal? ExchangeRate { get; init; }
    }
}

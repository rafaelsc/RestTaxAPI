namespace RestTaxAPI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Invoice Request data
    /// </summary>
    public record InvoiceRequest
    {
        /// <summary>
        /// The invoice date
        /// </summary>
        [Required]
        public DateTime? Date { get; init; } //TODO: Use DateTimeOffset

        /// <summary>
        /// The Pre-Tax Amount in cents (Unit: Cents)
        /// </summary>
        [Required]
        [Range(0, long.MaxValue)]
        public long? PreTaxAmountInCents { get; init; }

        /// <summary>
        /// The ISO 4217 3-letter code for the currency used in the the PreTax Amount
        /// </summary>
        [Required]
        [StringLength(3)]
        public string PreTaxAmountCurrencyCode { get; init; }

        /// <summary>
        /// The ISO 4217 3-letter code for the currency used in the the PreTax Amount
        /// </summary>
        [Required]
        [StringLength(3)]
        public string PaymentCurrencyCode { get; init; }
    }
}

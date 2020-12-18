namespace RestTaxAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    /// <summary>
    /// The Invoice Request data
    /// </summary>
    public record InvoiceRequest : IValidatableObject
    {
        /// <summary>
        /// The invoice date. ISO Day format (YYYY-MM-DD).
        /// Date should be from 2000-01-01 to Today
        /// </summary>
        /// <example>2020-03-04</example>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; init; } //TODO: Use DateTimeOffset

        /// <summary>
        /// The Pre-Tax Amount in cents (Unit: Cents)
        /// Should be positive
        /// </summary>
        [Required]
        [Range(0, long.MaxValue)]
        public long? PreTaxAmountInCents { get; init; }

        /// <summary>
        /// The ISO 4217 3-letter code for the currency used in the the PreTax Amount
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string PreTaxAmountCurrencyCode { get; init; }

        /// <summary>
        /// The ISO 4217 3-letter code for the currency used in the the PreTax Amount
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string PaymentCurrencyCode { get; init; }

        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Debug.Assert(this.Date != null, nameof(this.Date) + " != null");

            if (this.Date.Value.Date < new DateTime(2000, 1, 1))
                yield return new ValidationResult("Invalid Date. Date should be bigger than 2000-01-01.", new[] { nameof(this.Date) });
            if (this.Date.Value.Date > DateTime.Today)
                yield return new ValidationResult("Invalid Date. Date should not be a future date.", new[] { nameof(this.Date) });
        }
    }
}

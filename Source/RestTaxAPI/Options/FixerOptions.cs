namespace RestTaxAPI.Options
{
    using System.ComponentModel.DataAnnotations;

    public class FixerOptions
    {
        [Required(ErrorMessage = "Fixer APiKey NOT found, Add a new secret configuration 'Fixer:ApiKey' with the API Key.")]
        public string ApiKey { get; set; }
    }
}

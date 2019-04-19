namespace GreatQuotes.Data
{
    public enum Gender
    {
        Male,
        Female,
    }

    public class GreatQuote
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string QuoteText { get; set; }
        public Gender Gender { get; set; }

        public GreatQuote()
            : this(string.Empty, string.Empty, Gender.Male, string.Empty)
        {
        }

        public GreatQuote(string firstName, string lastName, Gender gender, string quoteText)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            QuoteText = quoteText;
        }
	}
}
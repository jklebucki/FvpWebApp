using System.Text.RegularExpressions;

namespace FvpWebApp.Infrastructure
{
    public class AddressFromJPKFA
    {
        private string _address { get; set; }
        public string street { get; protected set; }
        public string city { get; protected set; }
        public string postalCode { get; protected set; }
        public string country { get; protected set; }
        public AddressFromJPKFA(string address)
        {
            _address = address;
            ParseAddress();
        }

        private void ParseAddress()
        {
            if (!string.IsNullOrEmpty(_address))
            {
                string[] result = Regex.Split(_address, @"\d{2}-\d{3}", RegexOptions.IgnoreCase);
                if (result.Length > 0)
                    street = result[0].TrimEnd().Replace("ul. ", "").Replace("ul.", "");
                if (result.Length > 1)
                {
                    city = result[1].TrimStart();
                    postalCode = Regex.Match(_address, @"\d{2}-\d{3}").Value;
                    country = "PL";
                }
            }
        }
    }
}

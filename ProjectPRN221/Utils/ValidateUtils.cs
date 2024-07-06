using System.Text.RegularExpressions;

namespace ProjectPRN221.Utils
{
	public static class ValidateUtils
	{
		public static bool validatePassword(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
			{
				return false;
			}

			var hasMinimum8Chars = new Regex(@".{8,}");
			var hasLetter = new Regex(@"[a-zA-Z]");
			var hasDigit = new Regex(@"\d");

			return hasMinimum8Chars.IsMatch(password) && hasLetter.IsMatch(password) && hasDigit.IsMatch(password);
		}
	}
}

namespace CoolCatCollects.Core
{
	public static class PostageHelper
	{
		public static string FriendlyPostageName(string method)
		{
			switch (method)
			{
				case "UK_RoyalMailSecondClassStandard":
					return "RM 2nd";
				case "UK_RoyalMailFirstClassStandard":
					return "RM 1st";
				case "UK_RoyalMailAirmailInternational":
					return "RM Intl Standard";
				case "UK_eBayDeliveryPacklinkIntl":
					return "eBay Packlink International";
				case "UK_RoyalMailSecondClassRecorded":
					return "RM Second Class Recorded";
				case "UK_RoyalMailFirstClassRecorded":
					return "RM First Class Recorded";
				case "UK_myHermesDoorToDoorService":
					return "MyHermes";
				case "RoyalMail - Standard Parcel":
					return "RM Intl Standard - Parcel";
				case "RoyalMail - Standard Large Letter":
					return "RM Intl Standard - Large Letter";
				case "RoyalMail - Standard 2nd Class Large Letter":
					return "RM 2nd - Large Letter";
				case "RoyalMail - Standard 2nd Class Small Parcel":
					return "RM 2nd - Small Parcel";
				case "RoyalMail - Standard 1st Class Large Letter":
					return "RM 1st - Large Letter";
				case "RoyalMail - Standard 1st Class Small Parcel":
					return "RM 1st - Small Parcel";
			}

			return method;
		}

		public static string GetServiceCode(string method)
		{
			switch (method)
			{
				case "UK_RoyalMailSecondClassStandard":
				case "RoyalMail - Standard 2nd Class Large Letter":
				case "RoyalMail - Standard 2nd Class Small Parcel":
					return "CRL48";
				case "UK_RoyalMailFirstClassStandard":
				case "RoyalMail - Standard 1st Class Large Letter":
				case "RoyalMail - Standard 1st Class Small Parcel":
					return "CRL24";
				case "UK_RoyalMailAirmailInternational":
				case "UK_eBayDeliveryPacklinkIntl":
				case "RoyalMail - Standard Parcel":
				case "RoyalMail - Standard Large Letter":
					return "OLA";
				case "UK_RoyalMailSecondClassRecorded":
				case "UK_myHermesDoorToDoorService":
					return "BPR2";
				case "UK_RoyalMailFirstClassRecorded":
					return "BPR1";
			}

			return string.Empty;
		}

		public static string GetPackageSize(string method)
		{
			if (string.IsNullOrWhiteSpace(method))
			{
				return "";
			}

			if (method.ToLower().Contains("large letter"))
			{
				return "large letter";
			}
			else if (method.ToLower().Contains("parcel"))
			{
				return "parcel";
			}
			else if (method.ToLower().Contains("letter"))
			{
				return "letter";
			}

			return "";
		}

		public static bool IsInternational(string method)
		{
			switch (method)
			{
				case "UK_RoyalMailSecondClassStandard":
				case "RoyalMail - Standard 2nd Class Large Letter":
				case "RoyalMail - Standard 2nd Class Small Parcel":
				case "UK_RoyalMailFirstClassStandard":
				case "RoyalMail - Standard 1st Class Large Letter":
				case "RoyalMail - Standard 1st Class Small Parcel":
				case "UK_RoyalMailFirstClassRecorded":
				case "UK_RoyalMailSecondClassRecorded":
				case "UK_myHermesDoorToDoorService":
					return false;
				case "UK_RoyalMailAirmailInternational":
				case "UK_eBayDeliveryPacklinkIntl":
				case "RoyalMail - Standard Parcel":
				case "RoyalMail - Standard Large Letter":
					return true;
			}

			return false;
		}

		/// <summary>
		/// Round the weight up to the nearest threshold. Not actually used by RM, only used as a guide for us.
		/// </summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		public static string FormatWeight(string weight)
		{
			if (string.IsNullOrEmpty(weight))
			{
				return "0";
			}

			var d = decimal.Parse(weight);

			if (d <= 80)
			{
				return "0.1";
			}
			if (d <= 230)
			{
				return "0.25";
			}
			if (d <= 480)
			{
				return "0.5";
			}
			if (d <= 730)
			{
				return "0.75";
			}
			if (d <= 980)
			{
				return "1";
			}
			if (d <= 1230)
			{
				return "1.25";
			}
			if (d <= 1480)
			{
				return "1.5";
			}
			if (d <= 1730)
			{
				return "1.75";
			}
			if (d <= 1980)
			{
				return "2";
			}

			return (d / 1000).ToString();
		}
	}
}

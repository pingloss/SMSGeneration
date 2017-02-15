using System.IO;
using System.Text.RegularExpressions;


namespace SMSGeneration
{
    public class GenerateSMS
    {
        public string Smsnumber { get; set; }

        public string CreateSmsNumber(string emailBody)
        {
            // Convert HTML Body Text into plain text
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);

            //Extract SMS number from email
            var mobileregex = new Regex(@"Usage for\s(.\d+)");
            var mobilematch = mobileregex.Match(emailBodyText);

            // Convert UK national SMS number into international format
            string pattern = "^0";
            string replacement = "44";
            string nationalSms = (mobilematch.Groups[1].ToString());
            Regex convertRgx = new Regex(pattern);
            Smsnumber = convertRgx.Replace(nationalSms, replacement);

            //Return SMS number back to MiCC Enterprise script
            return Smsnumber;
        }

        public string CreateSmsText(string emailBody)
        {
            // Convert HTML Body Text into plain text
            HtmlToText htt = new HtmlToText();
            string emailBodyText = htt.ConvertHtml(emailBody);

            //Extract usage limit from email
            var costregex = new Regex(@"exceeded\s£(.\d\..\d)");
            var costmatch = costregex.Match(emailBodyText);

            //Calculate customer usage limit
            decimal saleprice = decimal.Parse(costmatch.Groups[1].Value) * 2;

            //Create SMS Text
            string smstext = ($"Dear Customer, Usage Limit for {Smsnumber} has exceeded £{saleprice} Total Cost");

            //Return SMS text back to MiCC Enterprise
            return smstext;
        }

        


    }
}

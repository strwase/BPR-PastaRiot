using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace PastaRiot
{
    public static class MailHelper
    {
        internal static void SendMail(Order order)
        {
            var body = CreateBody(order);

            MailMessage mail = new MailMessage("pasta@beardedpunk.com", order.Email);
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "send.one.com",
                Credentials = new NetworkCredential("pasta@beardedpunk.com", "N&kY7ji2&FNq&Q6"),
                EnableSsl = true
            };
            mail.Subject = "Inschrijving Bearded Pasta Riot";
            mail.Body = body;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }

        private static string CreateBody(Order order)
        {
            var number = order.Id;
            var amount = "€ " + order.Amount;
            var uur = order.TimeArrival;
            var orderDetails = GetOrderDetails(order.Choices);

            return $"<div style='font-family: verdana'>Beste, <br> <br> we hebben je inschrijving goed ontvangen.<br> Thnx!<br> Je inschrijvingsnummer is: { number }<br> <br> <br> Let wel op, deze is pas definitief als je betaald hebt.<br> Gelieve het bedrag van { amount } zo snel mogelijk over te maken op <br> <strong>BE20 0015 0221 5556</strong><br> met de vermeldig van 'bearded pasta riot' en je inschrijvingsnummer of naam.<br> <br> Bestelling: {orderDetails}<br> Uur: {uur}<br> <br> <H4>Dank je wel voor je steun en tot zondag 17 februari!<br> <br> Much love!<br> <br> Het hele BPR team<br> <br> </H4> <a href='www.beardedpunk.com'>www.beardedpunk.com</a><br> <a href='www.facebook.com/beardedpunkrecords'>www.facebook.com/beardedpunkrecords</a><br>";
        }

        private static string GetOrderDetails(List<Choice> choices)
        {
            var bol = "";
            var veggie = "";
            var vegan = "";

            if (choices.Any(x => x.Type == PastaChoice.Bolognese))
                bol = choices.Count(x => x.Type == PastaChoice.Bolognese) + " x Bolognaise";
            if (choices.Any(x => x.Type == PastaChoice.Veggie))
                veggie = choices.Count(x => x.Type == PastaChoice.Veggie) + " x Veggie";
            if (choices.Any(x => x.Type == PastaChoice.Vegan))
                vegan = choices.Count(x => x.Type == PastaChoice.Vegan) + " x Vegan";

            return string.Join(',', new string[] { bol, veggie, vegan }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
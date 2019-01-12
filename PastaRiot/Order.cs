using System;
using System.Collections.Generic;
using System.Linq;

namespace PastaRiot
{
    internal class Order
    {
        internal Order()
        {
            var rnd = new Random();
            Id = Convert.ToInt64(rnd.Next(100, 999).ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year);
        }

        private List<Choice> choices;

        public long Id { get; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool TakeAway { get; set; }
        public string TimeArrival { get; set; }

        public List<Choice> Choices
        {
            get { return choices; }
            set
            {
                choices = value;
                Amount = choices.Sum(x => x.Amount);
            }
        }
    }

    internal class Choice
    {
        public int Amount { get; set; }
        public PastaChoice Type { get; set; }
        public bool Kids { get; set; }
    }

    internal enum PastaChoice
    {
        Bolognese,
        Veggie,
        Vegan
    }
}
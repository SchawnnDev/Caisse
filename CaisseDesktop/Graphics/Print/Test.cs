﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Print
{
	public class Test
	{

		private List<string> Invoices = new List<string>();
		public Dictionary<SaveableArticle, int> Amounts = new Dictionary<SaveableArticle, int>();
		private SaveableCheckout Checkout { get; }

		public Test(SaveableCheckout checkout, string path)
		{
			Path = path;
			Checkout = checkout;
		}

		public void Generate()
		{
			using (var db = new CaisseServerContext())
			{
				var invoices = db.Invoices.Where(t => t.Cashier.Checkout.Id == Checkout.Id).Include(t=>t.Cashier).Include(t => t.PaymentMethod).ToList();
				var cashiers = db.Cashiers.Where(t => t.Checkout.Id == Checkout.Id).ToList();

				Invoices.Add($"Compte rendu au {DateTime.Now.ToString("dd/MM/yy HH:mm")}");

				Invoices.Add("");

				Invoices.Add($"Nom de la caisse : {Checkout.Name}");
				Invoices.Add($"Type de caisse : {Checkout.CheckoutType.Name}");
				Invoices.Add("");

				Invoices.Add("Caissiers :");
				Invoices.Add("Format : [id caissier]: [prénom] [nom]");

				foreach (var cashier in cashiers)
					Invoices.Add($"- {cashier.Id}: {cashier.GetFullName()}");

				Invoices.Add("");

				Invoices.Add("Opérations :");
				Invoices.Add("Format : ([id commande] - [id caissier]) [date]: [prixTotal] ([methodeDePaiement])");

				foreach (var invoice in invoices)
				{
					var stringBuilder = new StringBuilder();
					var operations = db.Operations.Include(t => t.Item).Where(t => t.Invoice.Id == invoice.Id).ToList();
					stringBuilder.Append($"- ({invoice.Id} - {invoice.Cashier.Id}) " + invoice.Date.ToString("dd/MM/yy HH:mm") + ": ");
					var finalPrice = 0m;
					foreach (var operation in operations)
					{
						if (Amounts.Any(t => t.Key.Id == operation.Item.Id))
						{
							Amounts[operation.Item] += operation.Amount;
						}
						else
						{
							Amounts.Add(operation.Item, operation.Amount);
						}

						finalPrice += operation.FinalPrice();

					}

					if (db.Consigns.Any(t => t.Invoice.Id == invoice.Id))
						finalPrice += db.Consigns.Where(t => t.Invoice.Id == invoice.Id).Select(t => t.Amount).Single();

					stringBuilder.Append($"{finalPrice:F} € ");

					stringBuilder.Append($"({invoice.PaymentMethod.Name})");

					Invoices.Add(stringBuilder.ToString());

				}

				Invoices.Add("");
				Invoices.Add("Totaux :");

				foreach (var amount in Amounts)
					Invoices.Add($"- {amount.Key.Name} : {amount.Value} x {amount.Key.Price:F}€ = {amount.Value * amount.Key.Price:F} €");

				Invoices.Add("");

				Invoices.Add($"Nombre total d'articles vendus: {Amounts.Sum(t => t.Value)}");
				Invoices.Add($"Montant total des ventes: {Amounts.Sum(t => t.Value * t.Key.Price):F} €");

				System.IO.File.WriteAllLines(Path, Invoices, Encoding.Unicode);


			}
		}

		public string Path { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Windows;
using CaisseDesktop.Graphics;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Common;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseServer;
using CaisseServer.Items;
using CaisseLibrary.test;
using CaisseLibrary.Print;
namespace CaisseDesktop
{
    /// <summary>
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        public SelectionWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var browser = new EvenementBrowser();
            browser.Show();
            Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        { }


            /*
              private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
              {

                  var list = new List<Question>();

                  list.Add(new Question("Einfuhrung in die BWL", " Erklaren Sie den Unterschied zwischen der Allgemeinen und der Speziellen BWL", @"

          Allgemeinen BWL => Probleme in allen Unternehmen


          Spezielle BWL => Probleme in unternehmen eines bestimmten Wirtschaftszweiges
      "));


                  list.Add(new Question("Einfuhrung in die BWL", "In welche 4 Teilfunktionen wird die BWL in der Regel unterteilt?", @"
          Beschaffung

          Produktion

          Absatz

          Finanzierung
      "));
                  list.Add(new Question("Einfuhrung in die BWL", "Was versteht man unter einer Betriebstypologie?", "Unternehmen lassen sich nach verschiedenen Merkmale gruppieren."));
                  list.Add(new Question("Einfuhrung in die BWL", "Anhand welcher Kriterien wird eine Betriebstypologie haeufig vorgenommen?", "Mitarbeiterzahl, Rechtsform, Betriebszweck, Wirtschaftszweig"));

                  2

                  list.Add(new Question("Grundbegriffe der BWL", " Was versteht man in der BWL unter den Begriffen 'Beduerfnissen' und 'Bedarf'?", @"Beduerfnisse => Mangelempfindungen nach Sachguetern / Dienstleistungen

      Bedarf => Kaufkraft ausgestattetes Beduerfnisse
      "));
                 list.Add(new Question("Grundbegriffe der BWL", "Was versteht man generell unter Produktionsfaktoren?", @"materielle oder immaterielle Mittel oder Leistungen, zur Bereitstellung von Gütern.
      "));
                 list.Add(new Question("Grundbegriffe der BWL", "Welches sind die wichtigsten Produktionsfaktoren in der VWL und BWL?", @"
      In der VWL: Boden, Kapital, Wissen, Arbeit

      In der BWL: Werkstoffe, Betriebsmittel, Ausführung
      "));
                 list.Add(new Question("Grundbegriffe der BWL", "Erläutern Sie, was man in der BWL unter sog. Wirtschaftsgütern versteht und wie diese unterschieden werden.", @"Wirtschaftsgüter => Gegenstände, Tätigkeiten, Rechte zur Bedürfnisbefriedigung

      Werden in 2 Kategorien gegliedert:

      - freie Güter: unbegrenzt, Handeln nicht notwendig
      - knappe Güter: sind begrenzt, zwingen zum Handel
      "));
                 list.Add(new Question("Grundbegriffe der BWL", "Erläutern Sie die Begriffe “Wirtschaften” und “Wirtschaftlichkeit”", @"Wirtschaften => Ausgleich von Bedürfnissen, erfordert den planmäßigen Einsatz von knapper Wirtschaftsgütern

      Wirtschaftlichkeit => allgemeiner Maß für die Effizienz im Sinne der Kosten-Nutzen-Relation
      "));
                 list.Add(new Question("Grundbegriffe der BWL", "Was versteht man unter dem sog. “Ökonomischen Prinzip”?", @"Menschen versuchen Nutzen/Gewinn zu Maximieren

      Es geben mehrere Prinzipien, das Minimumprinzip, Maximumprinzip und Optimumprinzip.
      "));
                 list.Add(new Question("Grundbegriffe der BWL", "Erläutern Sie die BWL-Kennzeichen “Produktivität”, “Liquidität” und “Rentabilität”", @"Produktivität = Output / Input

      Rentabilität = Verhältnis einer Erfolgsgröße zum eingesetzten Kapital einer Rechnungsperiode

      Rentabilität = Fähigkeit seine fälligen Verbindlichkeiten jederzeit und eingeschränkt begleichen zu können
      "));
                  list.Add(new Question("Das Unternehmen und sein Umfeld", "Erläutern Sie die Begriffe “Shareholder” und “Stakeholder”.", @"Stakeholder ist eine Person oder eine Gruppe die interessen am Verlauf/Ergebnis eines Projekt/Prozess hat. (zB: Staat, Arbeitnehmer, Kunden, Anteilseigner)

      Shareholder ist ein Anteilseigner, ein Aktionär.
      "));
                 list.Add(new Question("Das Unternehmen und sein Umfeld", "Worin unterscheiden sich der “Shareholder- und der Stakeholderansatz”?", @"Der Shareholder Ansatz ist die orientierung des Managements an den wirtschaftlichen Zielen der Anteilseigner.

      Der Stakeholder Ansatz ist die Gleichmäßige Berücksichtigung aller Anspruchsgruppen und derer Interessen.
      "));


                 list.Add(new Question("Unternehmensziele", "Erläutern Sie das Prinzip der SWOT-Analyse.", @"strengths, weaknesses, opportunities, threads

      stärken, schwächen, chance, risiko

      SWOT stellt eine Positionierungsanalyse der eigenen Aktivitäten gegenüber dem wettbewerb dar.

      "));
                 list.Add(new Question("Unternehmensziele", "Was versteht man unter dem Zielsystem (der Zielhierarchie) eines Unternehmens?", @"Ein gegliedertes System von unterzielen einer Zielkonzeption, wo die Oberziele aufgespaltet werden, um es zu konkretisieren.
      "));
                 list.Add(new Question("Unternehmensziele", "Erklären Sie das “SMART-Prinzip” bei der Zielformulierung anhand eines Beispiels.", @"SMART heißt :
      Spezifisch : Ziele müssen konkret formuliert werden
      Messbar : (Steigerung des Umsatzes um 5%)
      Anspruchsvoll : Ziele sollen eine Herausforderung darstellen
      Realisierbar : keine unrealistische Ziele
      Terminiert : einen definierten Zeitbezug
      "));
                 list.Add(new Question("Unternehmensziele", "Nennen Sie jeweils zwei Beispiele für ökonomische, soziale und ökologische Ziele Ihres Unternehmens.", @"ökonomische Ziele: 
      - gewinnmaximierung
      - rentabilität
      - liquidität

      soziale Ziele: Arbeitsplatzsicherheit , Mitbestimmung , Gerechte Entlohnung

      ökologische Ziele: Ressourcenschonung , Abfallvermeidung , Abfallrecycling
      "));
                 list.Add(new Question("Unternehmensführung", "Welche Aspekte sollten bei der Unternehmensführung beachtet werden?", @"Folgende Aspekte werden berücksichtigt :
      Marktorientierung
      Produktions- und Kostenorientierung
      Finanzorientierung
      Mitarbeiterorientierung
      Technologie- und Innovationsorientierung
      Umwelt- und Gesellschaftsorientierung
      "));
                 list.Add(new Question("Unternehmensführung", "Nennen Sie die wichtigsten Strategiefelder eines (großen) Unternehmens", @"Absatz
      Forschung und Entwicklung , Leistungserstellung , Beschaffung
      Personal , Informations- und Wissensmanagement
      Kosten , Investitionen , Finanzierung
      "));
                 list.Add(new Question("Unternehmensführung", "Welche Organisationsmöglichkeiten der Fertigung werden im Wesentlichen  unterschieden?", @"
      Werkstattprinzip
      Werkbankfertigung
      Fließbandfertigung
      Straßenfertigung
      Gruppenfertigung
      Baustellenfertigung
      "));
                 list.Add(new Question("Unternehmensführung", "Nennen Sie 5 wichtige Instrumente bzw. Techniken der Personalführung.", @"
      das Mitarbeitergespräch
      die Teambesprechungen
      die Leistungsmotivation
      Information und Kommunikation
      Personalentlohnung
      "));

                  var printer = new Printer("TicketsPrinter");
                  printer.SetUp();

                  var tickets = new List<ITicket>();
                  var ticket = new QuestionTicket();


                  foreach (var q in list)
                      tickets.Add(new QuestionTicket { Question = q });

                  printer.Print(tickets);


                    var ticket = new SalesReceipt(new Invoice(Main.ActualCashier)
                    {
                        SaveableInvoice = new SaveableInvoice
                        {
                            Cashier = new SaveableCashier
                            {
                                Id = 12
                            },

                            Id = 14808,
                            PaymentMethod = new SaveablePaymentMethod
                            {
                                Name = "ESPECE"
                            },

                            Date = DateTime.Now
                        },

                        GivenMoney = 50m,

                        Operations = new List<SaveableOperation>
                        {
                            new SaveableOperation
                            {
                                Amount = 5,
                                Item = new SaveableArticle
                                {
                                    Name = "CREPE",
                                    Price = 2m
                                }
                            },
                            new SaveableOperation
                            {
                                Amount = 2,
                                Item = new SaveableArticle
                                {
                                    Name = "PIZZA",
                                    Price = 6.5m
                                }
                            },
                            new SaveableOperation
                            {
                                Amount = 4,
                                Item = new SaveableArticle
                                {
                                    Name = "BOISSON",
                                    Price = 2.5m
                                }
                            },
                            new SaveableOperation
                            {
                                Amount = 1,
                                Item = new SaveableArticle
                                {
                                    Name = "BIERE",
                                    Price = 3.5m
                                }
                            },
                        }
                    });
                    ticket.Generate();
                    ticket.Print(); 
              }*/

            private void ButtonConnection_OnClick(object sender, RoutedEventArgs e)
        {
            new Connection().Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Checkout(null).Show();
            Close();
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            Close();
        }
    }
}

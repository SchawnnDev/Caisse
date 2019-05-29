using System;
using System.Threading;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Exceptions;
using Microsoft.PointOfService;
using CaisseLibrary.test;
using System.Collections.Generic;
namespace CaisseLibrary.Print
{
    public class QuestionTicket : ITicket
    {

        public Question Question;

        public override void SetImage(PosPrinter printer, int id)
        {
        }

        public ITicket PrintWith(Question question)
        {
            Question = question;
            return this;
        }

        public override void Print(PosPrinter printer)
        {

                printer.PrintNormal(PrinterStation.Receipt, CENTER + Question.Chap + NEW_LINE);

                printer.PrintNormal(PrinterStation.Receipt, "\u001b|150uF");
                printer.PrintNormal(PrinterStation.Receipt, CENTER + Question.Content + NEW_LINE);

                printer.PrintNormal(PrinterStation.Receipt, "\u001b|450uF");

                PrintMinimized(printer, CENTER + Question.Answer + NEW_LINE);

        }
    }
}

using ClosedXML.Excel;
using ProjektInzynierski.Domain.Models;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Infrastructure.Services
{
    internal class XlsxService : IXlsxService
    {
        public byte[] GenerateWalletExcel(WalletSnapshot wallet)
        {
            using var workbook = new XLWorkbook();

            var walletSheet = workbook.Worksheets.Add("Wallet");

            walletSheet.Cell(1, 1).Value = "Wallet ID";
            walletSheet.Cell(1, 2).Value = wallet.WalletId;

            walletSheet.Cell(2, 1).Value = "Account Currency";
            walletSheet.Cell(2, 2).Value = wallet.AccountCurrency;

            walletSheet.Cell(3, 1).Value = "Cash Balance";
            walletSheet.Cell(3, 2).Value = wallet.CashAmount;

            walletSheet.Cell(4, 1).Value = "Total Portfolio Value";
            walletSheet.Cell(4, 2).Value = wallet.TotalValueInAccountCurrency;

            walletSheet.Range("A1:A4").Style.Font.Bold = true;
            walletSheet.Columns().AdjustToContents();

            var sheet = workbook.Worksheets.Add("Instruments");

            sheet.Cell(1, 1).Value = "Instrument";
            sheet.Cell(1, 2).Value = "Quantity";
            sheet.Cell(1, 3).Value = "Unit Price";
            sheet.Cell(1, 4).Value = "Instrument Currency";
            sheet.Cell(1, 5).Value = "Position Value";
            sheet.Cell(1, 6).Value = $"Value in {wallet.AccountCurrency}";

            sheet.Range(1, 1, 1, 7).Style.Font.Bold = true;

            int row = 2;
            foreach (var item in wallet.Instruments)
            {
                sheet.Cell(row, 1).Value = item.InstrumentName;
                sheet.Cell(row, 2).Value = item.Quantity;
                sheet.Cell(row, 3).Value = item.UnitPrice;
                sheet.Cell(row, 4).Value = item.InstrumentCurrency;
                sheet.Cell(row, 5).Value = item.PositionValue;
                sheet.Cell(row, 6).Value = item.PositionValueInAccountCurrency;

                row++;
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

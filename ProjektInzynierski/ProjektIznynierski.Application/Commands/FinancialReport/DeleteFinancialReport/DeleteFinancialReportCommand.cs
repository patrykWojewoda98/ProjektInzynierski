using MediatR;

namespace ProjektIznynierski.Application.Commands.FinancialReport.DeleteFinancialReport
{
    public class DeleteFinancialReportCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}

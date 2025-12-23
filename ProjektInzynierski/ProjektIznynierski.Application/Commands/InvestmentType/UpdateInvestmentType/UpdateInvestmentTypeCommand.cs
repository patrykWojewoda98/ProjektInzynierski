using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Country.UpdateCountry
{
    public class UpdateInvestmentTypeCommand : IRequest<InvestmentTypeDto>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

    }
}

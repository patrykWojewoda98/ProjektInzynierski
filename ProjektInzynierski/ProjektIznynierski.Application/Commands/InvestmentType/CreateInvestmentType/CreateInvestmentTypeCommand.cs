using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Country.CreateCountry
{
    public class CreateInvestmentTypeCommand : IRequest<InvestmentTypeDto>
    {
        public string TypeName { get; set; }

    }
}

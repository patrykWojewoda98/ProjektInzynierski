using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WatchList.CreateWatchList
{
    internal class CreateWatchListCommandHandler : IRequestHandler<CreateWatchListCommand, WatchListDto>
    {
        private readonly IWatchListRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateWatchListCommandHandler(IWatchListRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WatchListDto> Handle(CreateWatchListCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.WatchList
            {
                ClientId = request.ClientId,
                CreatedAt = request.CreatedAt
            };
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return new WatchListDto
            {
                Id = entity.Id,
                ClientId = entity.ClientId,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}

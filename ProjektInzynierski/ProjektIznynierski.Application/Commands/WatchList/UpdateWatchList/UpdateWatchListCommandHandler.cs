using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.WatchList.UpdateWatchList
{
    internal class UpdateWatchListCommandHandler : IRequestHandler<UpdateWatchListCommand, WatchListDto>
    {
        private readonly IWatchListRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateWatchListCommandHandler(IWatchListRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WatchListDto> Handle(UpdateWatchListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new Exception($"WatchList with id {request.Id} not found.");
            }

            entity.ClientId = request.ClientId;
            entity.CreatedAt = request.CreatedAt;

            _repository.Update(entity);
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

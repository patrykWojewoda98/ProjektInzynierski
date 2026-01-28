using MediatR;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Comment.DeleteComment
{
    internal class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (comment is null)
                throw new Exception($"Comment with id {request.Id} not found.");

            _commentRepository.Delete(comment);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

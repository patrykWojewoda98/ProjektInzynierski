using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Comment.UpdateComment
{
    internal class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCommentCommand> _validator;

        public UpdateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IValidator<UpdateCommentCommand> validator)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            ValidationResult result = _validator.Validate(request);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(string.Join(", ", errors));
            }

            var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (comment is null)
                throw new Exception($"Comment with id {request.Id} not found.");

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            _commentRepository.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ClientId = comment.ClientID,
                InvestInstrumentID = comment.InvestInstrumentID,
                dateTime = comment.UpdatedAt ?? comment.CreatedAt
            };
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Commands.Comment.AddComment
{
    internal class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCommentCommand> _validator;

        public AddCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IValidator<AddCommentCommand> validator)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            ValidationResult result = _validator.Validate(request);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException(string.Join(", ", errors));
            }

            var comment = new Domain.Entities.Comment
            {
                ClientID = request.ClientId,
                InvestInstrumentID = request.InvestInstrumentId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            _commentRepository.Add(comment);
            await _unitOfWork.SaveChangesAsync();

            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ClientId = comment.ClientID,
                InvestInstrumentID = comment.InvestInstrumentID,
                dateTime = comment.CreatedAt
            };
        }
    }
}

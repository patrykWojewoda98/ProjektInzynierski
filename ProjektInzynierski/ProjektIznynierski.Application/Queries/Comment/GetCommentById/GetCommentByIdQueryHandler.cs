using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Comment.GetCommentById
{
    internal class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentByIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.id, cancellationToken);

            if (comment is null)
                throw new Exception($"Comment with id {request.id} not found.");

            var dto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ClientId = comment.ClientID,
                ClientName = comment.Client?.Name ?? "",
                InvestInstrumentID = comment.InvestInstrumentID,
                dateTime = comment.UpdatedAt ?? comment.CreatedAt
            };

            return dto;
        }
    }
}
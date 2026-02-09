using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Comment.GetCommentsByClientId
{
    internal class GetCommentsByClientIdQueryHandler : IRequestHandler<GetCommentsByClientIdQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsByClientIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentDto>> Handle(GetCommentsByClientIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByClientIdAsync(request.clientId);

            var dtos = comments.Select(comment => new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ClientId = comment.ClientID,
                ClientName = comment.Client?.Name ?? "",
                InvestInstrumentID = comment.InvestInstrumentID,
                dateTime = comment.UpdatedAt ?? comment.CreatedAt
            }).ToList();

            return dtos;
        }
    }
}
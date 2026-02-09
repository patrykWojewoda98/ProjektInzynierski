using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;

namespace ProjektIznynierski.Application.Queries.Comment.GetCommentsByInvestInstrumentId
{
    internal class GetCommentsByInvestInstrumentIdQueryHandler : IRequestHandler<GetCommentsByInvestInstrumentIdQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsByInvestInstrumentIdQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentDto>> Handle(GetCommentsByInvestInstrumentIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByInvestInstrumentIdAsync(request.investInstrumentId);

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
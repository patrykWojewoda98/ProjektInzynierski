using MediatR;
using ProjektIznynierski.Application.Dtos;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using System.Xml.Linq;

namespace ProjektIznynierski.Application.Queries.Comment.GetAllComments
{
    public class GetAllCommentsQueryHandler: IRequestHandler<GetAllCommentsQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetAllCommentsQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentDto>> Handle(GetAllCommentsQuery request,CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetAllAsync(cancellationToken);

            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                ClientId = c.ClientID,
                ClientName = c.Client.Name,
                InvestInstrumentID = c.InvestInstrumentID,
                dateTime = c.UpdatedAt ?? c.CreatedAt
            }).ToList();
        }
    }
}

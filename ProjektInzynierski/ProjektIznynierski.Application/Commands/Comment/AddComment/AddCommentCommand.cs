using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Comment.AddComment
{
    public class AddCommentCommand : IRequest<CommentDto>
    {
        public int ClientId { get; set; }
        public int InvestInstrumentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}

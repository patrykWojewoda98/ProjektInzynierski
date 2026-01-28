using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Commands.Comment.UpdateComment
{
    public class UpdateCommentCommand : IRequest<CommentDto>
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}

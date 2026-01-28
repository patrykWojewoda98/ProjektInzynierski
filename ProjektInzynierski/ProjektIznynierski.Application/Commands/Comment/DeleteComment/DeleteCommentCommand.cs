using MediatR;

namespace ProjektIznynierski.Application.Commands.Comment.DeleteComment
{
    public class DeleteCommentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}

using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Comment.GetAllComments
{
    public class GetAllCommentsQuery : IRequest<List<CommentDto>>
    {
    }
}

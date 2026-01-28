using MediatR;
using ProjektIznynierski.Application.Dtos;

namespace ProjektIznynierski.Application.Queries.Comment.GetCommentById
{
    public record GetCommentByIdQuery(int id) : IRequest<CommentDto>
    {
    }
}
using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.Comment.GetCommentsByClientId
{
    public record GetCommentsByClientIdQuery(int clientId) : IRequest<List<CommentDto>>
    {
    }
}
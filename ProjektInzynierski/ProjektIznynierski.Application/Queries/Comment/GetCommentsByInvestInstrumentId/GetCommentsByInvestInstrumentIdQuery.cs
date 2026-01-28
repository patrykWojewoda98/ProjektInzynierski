using MediatR;
using ProjektIznynierski.Application.Dtos;
using System.Collections.Generic;

namespace ProjektIznynierski.Application.Queries.Comment.GetCommentsByInvestInstrumentId
{
    public record GetCommentsByInvestInstrumentIdQuery(int investInstrumentId) : IRequest<List<CommentDto>>
    {
    }
}
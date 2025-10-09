using ProjektIznynierski.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAnalizeRepository
    {
        Task<Analize> GetAnalizeById(int id, CancellationToken cancellationToken = default);
    }
}

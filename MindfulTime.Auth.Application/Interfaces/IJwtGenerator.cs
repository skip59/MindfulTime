using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulTime.Auth.Domain.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(MindfulTime.Auth.Infrastructure.Entities.User user);
    }
}

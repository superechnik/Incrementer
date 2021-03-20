using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incrementer.Services
{
    public interface IRepo
    {
         public Task Upsert(Lib.KeyValuePair kvp);

    }
}

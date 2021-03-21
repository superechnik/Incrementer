using System.Threading.Tasks;

namespace Incrementer.Services
{
    public interface IRepo
    {
        public Task Upsert(Lib.KeyValuePair kvp);

        public Task<Models.KeyValue> Get(Lib.KeyValuePair kvp);

    }
}

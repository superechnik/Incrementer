using MassTransit;
using System.Threading.Tasks;
using Incrementer.Services;

namespace Incrementer
{
    public class KvpConsumer : IConsumer<Lib.KeyValuePair>
    {
        private readonly IRepo _repo;

        public KvpConsumer (IRepo repo)
        {
            _repo = repo;
        }

        public async Task Consume(ConsumeContext<Lib.KeyValuePair> ctx) =>
            await _repo.Upsert(ctx.Message);


    }
}

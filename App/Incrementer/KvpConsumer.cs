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

        /// <summary>
        /// dequeues the value from rabbitMq and upserts into the db
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<Lib.KeyValuePair> ctx) =>
            await _repo.Upsert(ctx.Message);


    }
}

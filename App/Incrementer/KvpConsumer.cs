using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Incrementer.Context;

namespace Incrementer
{
    public class KvpConsumer : IConsumer<Lib.KeyValuePair>
    {
        private readonly IncrementContext _ctx;
        public KvpConsumer (IncrementContext ctx)
        {
            _ctx = ctx;

        }

        public async Task Consume(ConsumeContext<Lib.KeyValuePair> ctx)
        {
            var message = ctx.Message;

            var data = new Models.KeyValue()
            {
                Key = message.Key,
                Value = message.Value
            };

            await Task.Run(() =>
            {
                _ctx.KeyValues.Add(data);
                _ctx.SaveChanges();

            });

        }


    }
}

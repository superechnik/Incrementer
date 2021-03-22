using Incrementer.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Incrementer.Services
{
    public class Repo : IRepo 
    {
        private readonly IncrementContext _ctx;
        
        public Repo(IncrementContext ctx)
        {
            _ctx = ctx;
        }

        private async Task<Models.KeyValue> GetValue (Lib.KeyValuePair rec)
        {
            var data = await _ctx.KeyValues
                .Where(x => x.Key == rec.Key)
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<Models.KeyValue> Get (Lib.KeyValuePair data)
        {
            //get current value
            var val = await GetValue(data);

            return val;

        }

        public async Task Upsert(Lib.KeyValuePair data)
        {
            //get current val 
            var val = await GetValue(data);

            if (val != null)
            {
                //update 
                val.Value += data.Value;

            }
            else
            {
                //insert 
                var row = new Models.KeyValue()
                {
                    Key = data.Key,
                    Value = data.Value
                };

                await _ctx.AddAsync(row);
            }

            await _ctx.SaveChangesAsync();

        }

    }
}

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

        public async Task<Models.KeyValue> Get(string key) =>
            await _ctx.KeyValues
                .Where(x => x.Key == key)
                .FirstOrDefaultAsync();                

        public async Task Upsert(Lib.KeyValuePair data)
        {

            //get current val 
            var val = await Get(data.Key);

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

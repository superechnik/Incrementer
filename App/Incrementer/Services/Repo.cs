using Incrementer.Context;
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

        public async Task Upsert(Lib.KeyValuePair data)
        {

            //get current val 
            var val = _ctx.KeyValues
                .Where(x => x.Key == data.Key)
                .FirstOrDefault();

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

                _ctx.Add(row);
            }

            await _ctx.SaveChangesAsync();


        }

    }
}

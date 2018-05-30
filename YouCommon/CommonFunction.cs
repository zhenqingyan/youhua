using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouCommon
{
    public class CommonFunction
    {
        public static TResponse ConvertModel<TRequest, TResponse>(TRequest model)
        {
            return JsonConvert.DeserializeObject<TResponse>(JsonConvert.SerializeObject(model));
        }
    }
}

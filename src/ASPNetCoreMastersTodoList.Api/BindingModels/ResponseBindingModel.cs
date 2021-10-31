using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.BindingModels
{
    [Serializable]
    public class ResponseBindingModel
    {
        public int StatusCode { get; set; }
        public string ResultMessage { get; set; }
        public object ResultObject { get; set; }
    }
}

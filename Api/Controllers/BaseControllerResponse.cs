using System.Collections.Generic;

namespace Easy.Transfers.Admin.Controllers
{
    public class BaseControllerResponse<T>
    {
        public T Dados { get; set; }
        public List<string> Mensagens { get; set; }
    }
}
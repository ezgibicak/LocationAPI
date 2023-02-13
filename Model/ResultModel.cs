using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ResultModel<T>
    {
        public string Mesaj => Data.Count() == 0 ? "İşlem Başarısız" : "İşlem Başarılı";
        public List<T>? Data { get; set; }
    }
}

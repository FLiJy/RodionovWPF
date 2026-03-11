using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR15
{
    public partial class basepart_
    {
        public string DisplayName => $"{name} ({manufacturer_?.name ?? "Неизвестно"})";

        public string PriceFormatted => $"{price:N0} ₽";

        public string TypeName => parttype_?.name ?? "Другое";
    }
}
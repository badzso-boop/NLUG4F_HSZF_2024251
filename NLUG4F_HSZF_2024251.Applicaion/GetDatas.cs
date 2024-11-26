using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class GetDatas
    {
        public DataProvider DataProvider { get; private set; }
        public GetDatas(DataProvider dataProvider)
        {
            this.DataProvider = dataProvider;
        }
    }
}

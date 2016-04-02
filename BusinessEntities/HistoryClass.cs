using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaizos.Entities.Business
{
    public class MonthlyFeeHistory
    {
        public string AccountNo { get; set; }
        public float FeeRate { get; set; }
        public string ShipmentType { get; set; }
        public string AdminAccountNo { get; set; }
        public DateTime InsertDate { get; set; }
    }

}

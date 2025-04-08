using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contarcts.Common
{
    public class CommonContarct
    {
        public enum GenderD
        {
            Male,
            Female,
            Other
        }

        public enum GenderP
        {
            Male,
            Female,
            Other
        }

        public enum AppointmentStatus
        {
            Pending,
            Confirmed,
            Success,
            Cancelled
        }

        public enum Status
        {
            Pending,
            Success,
            Failed
        }

        public enum PaymentWay
        {
            Credit_Card,
            UPI,
            Bank_Transfer
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessorOrder(Cart cart, ShippingDetails shipping);
    }
}

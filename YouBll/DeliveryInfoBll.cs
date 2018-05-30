using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouDal;
using YouModel.DataModel;

namespace YouBll
{
    public class DeliveryInfoBll
    {
        private DeliveryInfoDal _deliveryDal;
        public DeliveryInfoBll()
        {
            _deliveryDal = new DeliveryInfoDal();
        }


        public bool Insert()
        {
            var model = new DeliveryInfo
            {
                CreateTime = DateTime.Now,
                DeliveryCompany="test",
                DeliveryMan="Yan",
                DeliveryNumber=1,
                FileGuid=string.Empty,
                RowGuid=Guid.NewGuid().ToString()
            };
            _deliveryDal.Insert(model);
            return false;
        }

        public IEnumerable<DeliveryInfo> QueryList()
        {
            return _deliveryDal.GetData();
        }
    }
}

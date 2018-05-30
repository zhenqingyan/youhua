﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel
{
    public class ProductInfoVm
    {
        public ProductInfoVm()
        {
            CreateTime = DateTime.Now.ToString();
            Remark = string.Empty;
        }
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string Material { get; set; }
        public string NameAndType { get; set; }
        public string Weight { get; set; }
        public string TotalWeight { get; set; }
        public string FurnaceNumber { get; set; }
        public string Remark { get; set; }
        public string CreateTime { get; set; }
        public int IsDel { get; set; }
    }
}

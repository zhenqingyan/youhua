Vue.component('purchaseproinfo', function (h, params) {
    let randomId = parseInt((Math.random() * 10000));
    axios.get('../Content/components/purchaseplug/purchaseproplug.html?randomId=' + randomId).then(function (response) {
        h({
            template: response.data,
            props: ['data', 'status'],
            data: function () {
                let _self = this;
                return {
                    Columns: [
                        {
                            title: '材料名称',
                            key: 'Material'
                        },
                        {
                            title: '数量',
                            key: 'Number'
                        },
                        {
                            title: '收货数量',
                            key: 'ReceiveNumber'
                        },
                        {
                            title: '检验数量',
                            key: 'AuditNumber'
                        },
                        {
                            title: '单重',
                            key: 'Weight'
                        },
                        {
                            title: '总重',
                            key: 'TotalWeight'
                        },
                        {
                            title: '操作',
                            render: (h, params) => {
                                if (_self.status == 1 && params.row.ReceiveNumber < params.row.Number) {
                                    return h('Button', {
                                        on: {
                                            'click': function () {
                                                _self.receiveModel.data = $.extend(true, {}, params.row);
                                                _self.receiveModel.isShow = true;
                                            }
                                        }
                                    }, '收货')
                                }
                                else if (_self.status == 1 && params.row.Status == 1 && params.row.AuditNumber < params.row.Number) {
                                    return h('Button', {
                                        on: {
                                            'click': function () {
                                                _self.auditModel.data = $.extend(true, {}, params.row);
                                                _self.auditModel.isShow = true;
                                            }
                                        }
                                    }, '检验')
                                }
                                else {
                                    return h('span', {}, '无')
                                }
                            }
                        }
                    ],
                    receiveModel: {
                        isShow: false,
                        loading: false,
                        data: {}
                    },
                    auditModel: {
                        isShow: false,
                        loading: false,
                        data: {}
                    },
                };
            },
            methods: {
                receiveProduct: function (rowGuid) {
                    let _self = this;
                    _self.receiveModel.loading = true;

                    axios.post('ReveiveProduct', _self.receiveModel.data).then(function (response) {
                        _self.$Message.success(response.data);
                        if (response.data.indexOf('成功') != -1) {
                            var item = _self.data.find(function (item) {
                                return item.RowGuid == rowGuid;
                            });
                            item.ReceiveNumber = item.ReceiveNumber + _self.receiveModel.data.ReceiveNumber;
                            if (item.ReceiveNumber == item.Number) {
                                item.Status = 1;
                            }
                        }
                        _self.receiveModel.loading = false;
                        _self.receiveModel.isShow = false;
                    }).catch(function (error) {
                        _self.$Message.error(error);
                        _self.receiveModel.loading = false;
                    })
                },
                auditProduct: function (rowGuid) {
                    let _self = this;
                    _self.auditModel.loading = true;

                    axios.post('AuditProduct', _self.auditModel.data).then(function (response) {
                        _self.$Message.success(response.data);
                        if (response.data.indexOf('成功') != -1) {
                            var item = _self.data.find(function (item) {
                                return item.RowGuid == rowGuid;
                            });
                            item.AuditNumber = item.AuditNumber + _self.auditModel.data.AuditNumber;
                        }
                        _self.auditModel.loading = false;
                        _self.auditModel.isShow = false;
                    }).catch(function (error) {
                        _self.$Message.error(error);
                        _self.auditModel.loading = false;
                    })
                },
            }
        });
    })
});





Vue.component('purchaseinfo', function (h, params) {
    let randomId = parseInt((Math.random() * 10000));
    axios.get('../Content/components/purchaseplug/purchaseplug.html?randomId=' + randomId).then(function (response) {
        h({
            template: response.data,
            props: ['orderproductguid'],
            watch: {
                orderproductguid: function () {
                    let _self = this;
                    _self.searchInfo();
                }
            },
            mounted() {
                let _self = this;
                _self.searchInfo();
            },
            data: function () {
                let _self = this;
                return {
                    purchaseTableInfo: {
                        Columns: [
                            {
                                type: 'expand',
                                width: 50,
                                render: (h, params) => {
                                    return h('purchaseproinfo', {
                                        props: {
                                            data: params.row.Items,
                                            status: params.row.Status == 1 ? 1 : 0
                                        }
                                    });
                                }
                            },
                            {
                                title: '采购单号',
                                key: 'PurchaseNo'
                            },
                            {
                                title: '创建人',
                                key: 'Creator'
                            },
                            {
                                title: '创建时间',
                                key: 'CreateTime'
                            },
                            {
                                title: '操作',
                                render: (h, params) => {
                                    if (params.row.Status == 0) {
                                        return h('div', [
                                            h('Button', {
                                                props: {
                                                    type: 'info'
                                                },
                                                on: {
                                                    'click': function () {
                                                        _self.insertProductModal.productInfo = {};
                                                        _self.insertProductModal.data = {
                                                            Number: 0,
                                                            ProductInfoGuid: '',
                                                            PurchaseGuid: params.row.RowGuid
                                                        };
                                                        _self.insertProductModal.isShow = true;

                                                    }
                                                }
                                            }, '添加材料'),
                                            h('Button', {
                                                props: {
                                                    type: 'info'
                                                },
                                                on: {
                                                    'click': function () {
                                                        axios.post('AuditPurchase', params.row).then(function (response) {
                                                            _self.searchInfo();
                                                            _self.$Message.success(response.data);
                                                        }).catch(function (error) {
                                                            _self.$Message.error(error);
                                                        })
                                                    }
                                                }
                                            }, '审核')
                                        ]);
                                    } else {
                                        return h('span', {}, '已审核');
                                    }
                                }
                            }
                        ],
                        Data: [
                            {
                                PurchaseNo: '123',
                                Creator: '123',
                                CreateTime: '123',
                                Items: [
                                    {
                                        NameAndType: '123',
                                        Material: '13',
                                        Number: 1,
                                        Weight: 1,
                                        TotalWeight: 1,
                                        FurnaceNumber: 1,
                                        Remark: 1
                                    }]
                            }
                        ],
                        loading: false
                    },
                    insertModal: {
                        isShow: false,
                        data: {
                            PurchaseNo: '',
                            SerialNo: _self.orderproductguid
                        },
                        loading: false
                    },
                    insertProductModal: {
                        isShow: false,
                        data: {
                            Number: 0,
                            ProductInfoGuid: '',
                            PurchaseGuid: ''
                        },
                        productInfo: {
                        },
                        loading: false
                    }

                };
            },
            methods: {
                addNew: function () {
                    let _self = this;
                    _self.insertModal.data = {
                        PurchaseNo: '',
                        OrderProductGuid: _self.orderproductguid,
                        items: []
                    }
                    _self.insertModal.isShow = true;
                    _self.insertModal.loading = false;
                },
                insertPurchase: function () {
                    let _self = this;
                    _self.$refs['formItem'].validate((valid) => {
                        if (valid) {
                            _self.insertModal.loading = true;
                            axios.post('/WorkSpace/InsertPurchase', _self.insertModal.data).then(function (response) {
                                _self.insertModal.isShow = false;
                                _self.$Message.success('Success!');
                                _self.searchInfo();
                            }).catch(function (error) {
                                _self.$Message.error(error);
                            });
                        } else {
                            _self.$Message.error('Fail!');
                        }
                    });
                },
                searchInfo: function () {
                    let _self = this;
                    _self.purchaseTableInfo.loading = true;
                    axios.get('/WorkSpace/QueryPurchase?proGuid=' + _self.orderproductguid).then(function (response) {
                        _self.purchaseTableInfo.Data = response.data;
                        _self.purchaseTableInfo.loading = false;
                    }).catch(function (error) {
                        _self.$Message.error(error);
                        _self.purchaseTableInfo.loading = false;
                    });
                },
                insertPurchaseProduct: function () {
                    let _self = this;
                    _self.insertProductModal.loading = true;
                    axios.post('/WorkSpace/InsertPurchaseProduct', _self.insertProductModal.data).then(function (response) {
                        _self.insertProductModal.isShow = false;
                        _self.$Message.success('Success!');
                        _self.searchInfo();
                    }).catch(function (error) {
                        _self.$Message.error(error);
                    })
                },
                selectorCallBack: function (productInfo) {
                    let _self = this;
                    _self.insertProductModal.productInfo = productInfo;
                    if (JSON.stringify(productInfo) != '{}') {
                        _self.insertProductModal.data.ProductInfoGuid = productInfo.RowGuid;
                    }
                }
            }
        });
    })
});

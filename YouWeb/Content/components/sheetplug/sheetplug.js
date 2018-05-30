Vue.component('sheetproinfo', function (h, params) {
    let randomId = parseInt((Math.random() * 10000));
    axios.get('../Content/components/sheetplug/sheetproplug.html?randomId=' + randomId).then(function (response) {
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
                            title: '提货数量',
                            key: 'GetNumber'
                        },
                        {
                            title: '库存数量',
                            key: 'StockNumber'
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
                                if (_self.status == 1 && params.row.GetNumber < params.row.Number) {
                                    return h('Button', {
                                        on: {
                                            'click': function () {
                                                _self.receiveModel.data = $.extend(true, {}, params.row);
                                                _self.receiveModel.isShow = true;
                                            }
                                        }
                                    }, '提货')
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
                };
            },
            methods: {
                receiveProduct: function (rowGuid) {
                    let _self = this;
                    _self.receiveModel.loading = true;
                    axios.post('/WorkSpace/GetProduct', _self.receiveModel.data).then(function (response) {
                        _self.$Message.success(response.data);
                        if (response.data.indexOf('成功') != -1) {
                            var item = _self.data.find(function (item) {
                                return item.RowGuid == rowGuid;
                            });
                            item.GetNumber = item.GetNumber + _self.receiveModel.data.GetNumber;
                            item.StockNumber = item.StockNumber - _self.receiveModel.data.GetNumber;
                            if (item.GetNumber == item.Number) {
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
            }
        });
    })
});





Vue.component('sheetplug', function (h, params) {
    let randomId = parseInt((Math.random() * 10000));
    axios.get('../Content/components/sheetplug/sheetplug.html?randomId=' + randomId).then(function (response) {
        h({
            template: response.data,
            props: ['orderproductguid'],
            data: function () {
                let _self = this;
                return {
                    sheetTableInfo: {
                        Columns: [
                            {
                                type: 'expand',
                                width: 50,
                                render: (h, params) => {
                                    return h('sheetproinfo', {
                                        props: {
                                            data: params.row.Items,
                                            status: params.row.Status == 1 ? 1 : 0
                                        }
                                    });
                                }
                            },
                            {
                                title: '工艺流转单号',
                                key: 'SheetNo'
                            },
                             {
                                 title: '数量',
                                 key: 'Number'
                             },
                              {
                                  title: '完成数量',
                                  key: 'MadeNumber'
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
                                                            SheetGuid: params.row.RowGuid
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
                                                        axios.post('/WorkSpace/AuditSheet', params.row).then(function (response) {
                                                            _self.searchInfo();
                                                            _self.$Message.success(response.data);
                                                        }).catch(function (error) {
                                                            _self.$Message.error(error);
                                                        })
                                                    }
                                                }
                                            }, '审核')
                                        ]);
                                    } else if (params.row.Status == 1) {
                                        return h('Button', {
                                            on: {
                                                'click': function () {
                                                    _self.receiveModel.data = $.extend(true, {}, params.row);
                                                    _self.receiveModel.isShow = true;
                                                }
                                            }
                                        }, '入库');
                                    } else {
                                        return h('span', {}, '完成');
                                    }
                                }
                            }
                        ],
                        Data: [
                            {
                                SheetNo: '123',
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
                            SheetNo: '',
                            OrderProductGuid: _self.orderproductguid,
                            Number:0
                        },
                        loading: false
                    },
                    insertProductModal: {
                        isShow: false,
                        data: {
                            Number: 0,
                            ProductInfoGuid: '',
                            SheetGuid: ''
                        },
                        productInfo: {
                        },
                        loading: false
                    },
                    receiveModel: {
                        isShow: false,
                        loading: false,
                        data: {}
                    },
                };
            },
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
            methods: {
                addNew: function () {
                    let _self = this;
                    _self.insertModal.data = {
                        SheetNo:'',
                        OrderProductGuid: _self.orderproductguid,
                        Number:0,
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
                            axios.post('/WorkSpace/InsertSheet', _self.insertModal.data).then(function (response) {
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
                    _self.sheetTableInfo.loading = true;
                    axios.get('/WorkSpace/QuerySheet?proGuid=' + _self.orderproductguid).then(function (response) {
                        _self.sheetTableInfo.Data = response.data;
                        _self.sheetTableInfo.loading = false;
                    }).catch(function (error) {
                        _self.$Message.error(error);
                        _self.sheetTableInfo.loading = false;
                    });
                },
                insertPurchaseProduct: function () {
                    let _self = this;
                    _self.insertProductModal.loading = true;
                    axios.post('/WorkSpace/InsertSheetProduct', _self.insertProductModal.data).then(function (response) {
                        _self.insertProductModal.isShow = false;
                        _self.$Message.success('Success!');
                        _self.searchInfo();
                    }).catch(function (error) {
                        _self.$Message.error(error);
                    })
                },
                selectorCallBack: function (productInfo) {
                    let _self = this;
                    debugger
                    _self.insertProductModal.productInfo = productInfo;
                    if (JSON.stringify(productInfo) != '{}') {
                        _self.insertProductModal.data.ProductInfoGuid = productInfo.RowGuid;
                    }
                },
                madeProduct: function () {
                    let _self = this;
                    _self.receiveModel.loading = true;
                    axios.post('/WorkSpace/MadeProduct', _self.receiveModel.data).then(function (response) {
                        _self.$Message.success(response.data);
                        debugger
                        if (response.data.indexOf('成功') != -1) {
                            var item = _self.sheetTableInfo.Data.find(function (item) {
                                return item.RowGuid == _self.receiveModel.data.RowGuid;
                            });
                            item.MadeNumber = item.MadeNumber + _self.receiveModel.data.MadeNumber;
                            if (item.MadeNumber == item.Number) {
                                item.Status = 2;
                            }
                            _self.$emit('refresh');
                        }
                        _self.receiveModel.loading = false;
                        _self.receiveModel.isShow = false;
                    }).catch(function (error) {
                        _self.$Message.error(error);
                        _self.receiveModel.loading = false;
                    })
                }
            }
        });
    })
});

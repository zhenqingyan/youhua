//document.write("<script src='/Content/components/purchaseplug/purchaseplug.js'></script>")
Vue.component('orderproplug', function (h, params) {
    let randomId = parseInt((Math.random() * 10000));
    axios.get('../Content/components/orderproplug/orderproplug.html?randomId=' + randomId).then(function (response) {
        h({
            template: response.data,
            props: ['serialid'],
            mounted() {
                let _self = this;
                _self.Query();
            },
            watch:{
                serialid: function (curVal, oldVal) {
                    let _self = this;
                    _self.Query();
                }
            },
            data: function () {
                let _self = this;
                return {
                    Table: {
                        Columns: [
                        {
                            title: '项目编号',
                            key: 'ProjectSerialId'
                        },
                        {
                            title: '材料名称',
                            key: 'Material'
                        },
                        {
                            title: '数量',
                            key: 'Number'
                        },
                        {
                            title: '交货日期',
                            key: 'DeliveryDate'
                        },
                        {
                            title: '备注',
                            key: 'Remark'
                        },
                        {
                            title: '状态',
                            render: (h, params) => {
                                let objTemp={
                                    color:'red',
                                    data:'未完成'
                                };
                                if (params.row.Status == 1)
                                {
                                    objTemp.color = 'green';
                                    objTemp.data = '完成';
                                }
                                return h('Tag', {
                                    props: {
                                        color: objTemp.color
                                    }
                                }, objTemp.data)
                            }
                        },
                        {
                            title: '操作',
                            render: (h, params) => {
                                return h('Button', {
                                    on: {
                                        'click': function () {
                                            _self.currentMeteriaInfo = params.row;
                                        }
                                    }
                                }, '查看')
                            }
                        }
                        ],
                        Data: [],
                        count: 0,
                        search: {
                            from: 1,
                            size: 10
                        },
                        loading: false
                    },
                    currentMeteriaInfo: {
                        RowGuid: '',
                        Material: '',
                        Number: '',
                        Remark: ''
                    },
                }
            },
            methods: {
                Query: function () {
                    let _self = this;
                    _self.Table.loading = true;
                    axios.post('/OrderInfo/GetProductData', $.extend(true, _self.Table.search, {
                        SerialId: _self.serialid
                    })).then(function (response) {
                        _self.Table.Data = response.data.data;
                        _self.Table.count = response.data.count;
                        _self.Table.loading = false;
                        _self.currentGuid='';
                        if (response.data.data.length > 0)
                        {
                            if (_self.currentMeteriaInfo.RowGuid != '') {
                                var item = response.data.data.find(function (item) {
                                    return item.RowGuid == _self.currentMeteriaInfo.RowGuid;
                                });
                                if (item == undefined)
                                {
                                    _self.currentMeteriaInfo = response.data.data[0];
                                } 
                            } else {
                                _self.currentMeteriaInfo = response.data.data[0];
                            }
                        }
                    }).catch(function (error) {
                        _self.$Message.error(error);
                    });
                },
                ChangePage: function (currentPage) {
                    let _self = this;
                    _self.Table.search.from = currentPage;
                    _self.Query();
                },
            }
        });
    })
});

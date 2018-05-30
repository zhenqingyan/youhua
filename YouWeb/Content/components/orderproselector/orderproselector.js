Vue.component('orderproselector', function (h, params) {
    axios.get('../Content/components/orderproselector/orderproselector.html').then(function (response) {
        h({
            template: response.data,
            props: [],
            data: function () {
                let _self = this;
                return {
                    isShow: false,
                    Table: {
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
                                title: '备注',
                                key: 'Remark'
                            }
                        ],
                        Data: [],
                        count: 0,
                        search: {
                            from: 1,
                            size:10
                        },
                        selectorData: {},
                        loading:false
                    },
                    showLoading: false
                }
            },
            methods: {
                selectorCallBack: function () {
                    let _self = this;
                    _self.isShow = false;
                    _self.$emit('callback', _self.Table.selectorData);
                },
                selectRow: function (currentRow, oldCurrentRow) {
                    let _self = this;
                    _self.Table.selectorData = currentRow;
                },
                showModal: function () {
                    let _self = this;
                    _self.Table.search.size = 10;
                    _self.Table.search.from = 1;
                    _self.showLoading = true;
                    _self.$refs.mytable.clearCurrentRow();
                    _self.Table.selectorData = {};
                    _self.isShow = true;
                    _self.Query();
                    _self.showLoading = false;
                },
                Query: function () {
                    let _self = this;
                    _self.Table.loading = true;
                    axios.post('/OrderProduct/GetData', _self.Table.search).then(function (response) {
                        _self.Table.Data = response.data.data;
                        _self.Table.count = response.data.count;
                        _self.Table.loading = false;
                    }).catch(function (error) {
                        _self.$Message.error(error);
                    });
                },
                ChangePage: function (currentPage) {
                    let _self = this;
                    _self.Table.search.from = currentPage;
                    _self.Query();
                }
            }
        });
    })
});

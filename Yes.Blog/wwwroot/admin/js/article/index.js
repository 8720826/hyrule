document.addEventListener('alpine:init', function () {
	Alpine.data('data', function () {
		return {
			pageinfo: {
				items: []
			},
			loading: false,
			loadItems(pageIndex = 1) {
				if (this.loading) {
					return;
				}
				this.loading = true;
				axios.get('articles?pageIndex=' + pageIndex)
					.then(data => {
						this.pageinfo = data;
					})
					.finally(() => {
						this.loading = false;
					});
			},
			getVisiblePages() {
				const visiblePages = [];
				const startPage = Math.max(1, this.pageinfo.pageIndex - 3);
				const endPage = Math.min(this.pageinfo.totalPage, this.pageinfo.pageIndex + 3);

				for (let i = startPage; i <= endPage; i++) {
					visiblePages.push(i);
				}

				return visiblePages;
			},
			deleteId: 0,
			showModal: false,
			confirmAction() {
				axios.delete('articles?id=' + this.deleteId)
					.then(data => {
						this.loadItems(this.pageinfo.pageIndex);
					});
			},
			init() {
				this.loadItems(1);
				window.addEventListener('load', () => {
					document.title = "文章列表 - " + window.config.name + "";
				});
				
			}
		};
	})
});
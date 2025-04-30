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
				axios.get('comments?pageIndex=' + pageIndex)
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
			commentId: 0,
			actionName: "",
			actionType:"",
			confirmAction() {
				switch (this.actionType) {
					case "delete":
						axios.delete('comments?id=' + this.commentId)
							.then(data => {
								this.loadItems(this.pageinfo.pageIndex);
							});
						break;
					case "display":
						axios.put('comments?id=' + this.commentId)
							.then(data => {
								this.loadItems(this.pageinfo.pageIndex);
							});
						break;
					case "hide":
						axios.put('comments?id=' + this.commentId)
							.then(data => {
								this.loadItems(this.pageinfo.pageIndex);
							});

						break;
				}
				this.cancelModal();
			},
			showModal(id, type) {
				console.log("id=" + id);
				console.log("type=" + type);
				this.commentId = id;
				this.actionType = type;
				switch (type) {
					case "delete": this.actionName = "删除"; break;
					case "display": this.actionName = "显示"; break;
					case "hide": this.actionName = "隐藏"; break;
				}
			},
			cancelModal() {
				this.commentId = 0;
				this.actionName = "";
				this.actionType = "";
			},
			init() {
				this.loadItems(1);
				window.addEventListener('load', () => {
					document.title = "评论列表 - " + window.config.name + "";
				});
				
			}
		};
	})
});
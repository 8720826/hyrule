document.addEventListener('alpine:init', function () {
	Alpine.data('data', function () {
		return {
			items: [],
			loading: false,
			loadItems() {
				if (this.loading) {
					return;
				}
				this.loading = true;
				axios.get('users')
					.then(data => {
						this.items = data;
					})
					.finally(() => {
						this.loading = false;
					});
			},
			deleteId: 0,
			showModal: false,
			confirmAction() {
				this.isSubmitting = true;
				axios.delete('users?id=' + this.deleteId)
					.then(data => {
						this.loadItems();
					})
					.finally(() => {
						this.isSubmitting = false;
					});
			},
			init() {
				this.loadItems();

				window.addEventListener('load', () => {
					document.title = "管理员列表 - " + window.config.name + "";
				});
			}
		};
	})
});
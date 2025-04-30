document.addEventListener('alpine:init', function () {
	Alpine.data('data', function () {
		return {
			items: [],
			loading: false,
			theme:"",
			loadItems() {
				if (this.loading) {
					return;
				}
				this.loading = true;
				axios.get('themes')
					.then(data => {
						this.items = data;
					})
					.finally(() => {
						this.loading = false;
					});
			},
			showModal: false,
			confirmAction() {
				this.isSubmitting = true;
				axios.put('themes', { theme: theme })
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
					document.title = "模板设置 - " + window.config.name + "";
				});
			}
		};
	})
});
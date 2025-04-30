document.addEventListener('alpine:init', function () {
	Alpine.data('data', function () {
		return {
			articles: [],
			comments: [],
			stats: {},
			loadArticles() {
				axios.get('articles/latest')
					.then(data => {
						this.articles = data;
					});
			},
			loadStats() {
				axios.get('Stats')
					.then(data => {
						this.stats = data;
					});
			},
			init() {
				this.loadStats();
				this.loadArticles();

				window.addEventListener('load', () => {
					document.title = "控制台 - " + window.config.name + "";
				});
			}
		};
	})
});
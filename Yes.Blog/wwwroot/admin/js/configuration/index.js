document.addEventListener('alpine:init', function () {
	Alpine.data('data', function () {
		return {
            tab: "site",
            config: {
                storage: {
                    storageType: 0,
                    qiniu: {}
                }
            },
            tempfile:"",
            defaultArticleRoute:"",
            loadConfiguration() {
                axios.get('configuration')
                    .then(data => {
                        this.config = data;
                        this.defaultArticleRoute = data.articleRoute;

                        const config = {
                            data: data,
                            timestamp: new Date().getTime()
                        };
                        localStorage.setItem("config", JSON.stringify(config));
                    });
                return this;
            },
            save() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.put('configuration', this.config)
                    .then(data => {
                        location.href = "/admin/configuration";
                    })
                    .finally(() => {
                        this.isSubmitting = false;
                    });
            },
            addIcon() {
                const fileinput = document.getElementById('img-file-input');
                if (fileinput) {
                    fileinput.click();
                }
            },
			init() {
                this.loadConfiguration();

                window.addEventListener('load', () => {
                    document.title = "基本设置 - " + window.config.name + "";
                });
			}
		};
	})
});
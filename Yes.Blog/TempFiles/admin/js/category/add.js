document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {

        return {
            category: {
                name: "",
                slug: "",
                sort: 0,
                coverUrl: "",
                description: ""
            },
            isSubmitting: false,
            addCategory() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.post('categories', this.category)
                    .then(data => {
                        location.href = "/admin/category";
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
                this.loadCategory();

                window.addEventListener('load', () => {
                    document.title = "添加分类 - " + window.config.name + "";
                });
            }
        };
    })
});
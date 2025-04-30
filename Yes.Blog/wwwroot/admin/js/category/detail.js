document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {

        const id = getUrlParameter("id");
        return {
            category: {
                name: "",
                slug: "",
                sort: 0,
                coverUrl: "",
                description: "",
                id: id
            },
            isSubmitting: false,
            loadCategory() {
                axios.get('categories/' + id)
                    .then(data => {
                        this.category = data;
                    });
                return this;
            },
            editCategory() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.put('categories', this.category)
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
                    document.title = "编辑分类 - " + window.config.name + "";
                });
            }
        };
    })
});
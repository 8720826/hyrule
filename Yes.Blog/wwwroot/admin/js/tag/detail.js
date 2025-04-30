document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {

        const id = getUrlParameter("id");
        return {
            tag: {
                name: "",
                slug: "",
                id: id
            },
            isSubmitting: false,
            loadTag() {
                axios.get('tags/' + id)
                    .then(data => {
                        this.tag = data;
                    });
                return this;
            },
            editTag() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.put('tags', this.tag)
                    .then(data => {
                        location.href = "/admin/tag";
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
                this.loadTag();

                window.addEventListener('load', () => {
                    document.title = "编辑标签 - " + window.config.name + "";
                });
            }
        };
    })
});
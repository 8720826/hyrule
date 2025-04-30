document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {
        const editor = new toastui.Editor({
            el: document.querySelector('#editor'),
            language: 'zh-CN',
            previewStyle: 'vertical',
            height: '660px',
            initialEditType: 'wysiwyg',
            initialValue: ""
        });


        return {
            article: {
                title: "",
                slug: "",
                content: "",
                coverUrl: "",
                tags: [],
                categoryId: 0,
                summary: ""
            },
            tempfile: null,
            coverUrl:"",
            categories: [],
            isSubmitting: false,
            loadCategories() {
                if (this.loading) {
                    return;
                }
                this.loading = true;
                axios.get('categories')
                    .then(data => {
                        this.categories = data;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            createArticle() {
                if(this.isSubmitting){
                    return;
                }
                this.isSubmitting = true;
                axios.post('pages', this.article)
                    .then(data => {
                        location.href = "/admin/page";
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
                editor.on('change', () => {
                    this.article.content = editor.getMarkdown();
                });

                this.loadCategories();

                window.addEventListener('load', () => {
                    document.title = "添加单页 - " + window.config.name + "";
                });
            }
        };
    })
});
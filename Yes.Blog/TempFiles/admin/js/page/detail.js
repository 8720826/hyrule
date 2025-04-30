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

        const id = getUrlParameter("id");


        return {
            article: {
                title: "",
                slug: "",
                content: "",
                coverUrl: "",
                tags: [],
                categoryId: 0,
                summary: "",
                id: id
            },
            tagInput:"",
            tempfile: null,
            coverUrl:"",
            categories: [],
            isSubmitting: false,
            loadCategories() {

                axios.get('categories')
                    .then(data => {
                        this.categories = data;
                    });
                return this;
            },
            loadPage() {
                axios.get('pages/' + id)
                    .then(data => {
                        this.article = data;
                        editor.setMarkdown(this.article.content);
                    });
                return this;
            },
            editArticle() {
                if(this.isSubmitting){
                    return;
                }
                this.isSubmitting = true;
                axios.put('pages/', this.article)
                    .then(data => {
                        console.log('data:', data);
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

                this.loadCategories().loadPage();

                window.addEventListener('load', () => {
                    document.title = "编辑单页 - " + window.config.name + "";
                });
            }
        };
    })
});
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
            tempfile :null,
            tagInput:"",
            categories: [],
            isSubmitting: false,
            loadCategories() {
                axios.get('categories')
                    .then(data => {
                        this.categories = data;
                    });
                return this;
            },
            loadArticle() {
                axios.get('articles/' + id)
                    .then(data => {
                        this.article = data;
                        editor.setMarkdown(this.article.content);
                    });
                return this;
            },
            saveTags() {
                this.tagInputText = document.getElementById('tag-input').innerText;
                if (this.tagInputText) {
                    this.tagInputText.replace(/，/g, ",").replace(/ /g, ",").split(',').forEach(x => this.article.tags.push(x));
                    document.getElementById('tag-input').innerText = '';
                } 
            },
            editArticle() {
                this.saveTags();
                if(this.isSubmitting){
                    return;
                }
                this.isSubmitting = true;
                axios.put('articles/', this.article)
                    .then(data => {
                        location.href = "/admin/article";
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

                this.loadCategories().loadArticle();

                window.addEventListener('load', () => {
                    document.title = "编辑文章 - " + window.config.name + "";
                });
            }
        };
    })
});
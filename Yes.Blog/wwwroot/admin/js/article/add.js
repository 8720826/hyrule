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
            pageTitle: "撰写新文章 - " + window.config.name + "",
            article: {
                title: "",
                slug: "",
                content: "",
                coverUrl: "",
                tags: [],
                categoryId: 0,
                summary: ""
            },
            tempfile :null,
            categories: [],
            isSubmitting: false,
            tagInputText:"",
            loadCategories() {
                if (this.loading) {
                    return;
                }
                this.loading = true;
                axios.get('categories')
                    .then(data => {
                        this.categories = data;
                    });
            },
            saveTags() {
                this.tagInputText = document.getElementById('tag-input').innerText;
                if (this.tagInputText) {
                    this.tagInputText.replace(/，/g, ",").replace(/ /g, ",").split(',').forEach(x => this.article.tags.push(x));
                    document.getElementById('tag-input').innerText = '';
                }
            },
            createArticle() {
                this.saveTags(); 
                if(this.isSubmitting){
                    return;
                }
                this.isSubmitting = true;
                axios.post('articles', this.article)
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

                this.loadCategories();

                window.addEventListener('load', () => {
                    document.title = this.pageTitle;
                });
            }
        };
    })
});
document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {
        const themeName = getUrlParameter("themeName")||"default";
        const fileName = getUrlParameter("fileName") || "Index.liquid";

        return {
            file: {

            },
            tagInput: "",
            tempfile: null,
            coverUrl: "",
            files: [],
            isSubmitting: false,
            themeName: "",
            fileName: "",
            loadFiles() {

                axios.get('/themes/' + themeName +'/files')
                    .then(data => {
                        this.files = data;
                    });
                return this;
            },
            loadFile() {
                axios.get('/themes/' + themeName + '/files/' + fileName)
                    .then(data => {
                        this.file = data;
                    });
                return this;
            },
            editFile() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.put('/themes/' + themeName + '/files/' + fileName, { content: this.file.content })
                    .then(data => {
                        console.log('data:', data);
                        location.href = "/admin/theme/detail?themeName=" + themeName + "&fileName=" + fileName + "";
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
                this.loadFiles().loadFile();
                this.themeName = themeName;
                this.fileName = fileName;
                window.addEventListener('load', () => {
                    document.title = "自定义模板内容 - " + window.config.name + "";
                });
            }
        };
    })
});
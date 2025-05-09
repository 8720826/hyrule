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
            uploadFile:"",
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
            isZipFile(filename) {
                if (typeof filename !== 'string') return false;
                return /\.zip$/i.test(filename);
            },
            handleThemeUpload(file) {
                if (!this.isZipFile(file.name)) {
                    window.pushError("请上传zip文件");
                    return;
                }
                this.uploadFile = file;
            },
            save() {
                const formData = new FormData();
                formData.append('file', this.uploadFile);

                axios.post('/themes/upload', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                .then(data => {
                    location.href = "/admin/theme";
                })
            },
            addFile() {
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
document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {
        // 初始化 CodeMirror 编辑器
        const editor = CodeMirror(document.getElementById("code-editor"), {
            mode: "text/html", // 设置语法模式
            lineNumbers: true,  // 显示行号
            theme: "dracula",   // 可选主题
            value: "",
            onChange: function (cm) {
                // 实时同步内容到隐藏的 textarea
                document.getElementById("code-content").value = cm.getValue();
            }
        });

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
                axios.get('/themes/' + themeName + '/file/?fileName=' + fileName)
                    .then(data => {
                        this.file = data;
                        editor.setValue(this.file.content);
                    });
                return this;
            },
            editFile() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.put('/themes/' + themeName + '/file/?fileName=' + fileName, { content: this.file.content })
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
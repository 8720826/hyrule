document.addEventListener('alpine:init', () => {
    Alpine.store('errorMessage', '');

    axios.defaults.baseURL = "/admin/api/"
    axios.defaults.timeout = 3000;
    axios.defaults.headers.common['Authorization'] = "";
    axios.defaults.headers.post['Content-Type'] = 'application/json';
    axios.interceptors.request.use(
        (config) => {
            return config;
        },
        (error) => {
            return Promise.reject(error);
        }
    );

    const baseErrorMessage = "您的请求未能成功处理，可能是服务器开小差了！";

    axios.interceptors.response.use(
        (response) => {
            if (!response.data.success) {
                pushError(response.data.message || baseErrorMessage);
            } else {
                return Promise.resolve(response.data.data);
            }
        },
        (error) => {
            if (error.response) {
                if (error.response.status == 422) { 
                    const errors = Object.values(error.response.data.errors);
                    const values = Array.from(new Set(errors.flat()));
                    const errorMessage = values.join('    ');
                    pushError(errorMessage);
                } else if (error.response.status == 500) {
                    const errorMessage = error.response.data.detail;
                    pushError(errorMessage);
                } else {
                    pushError(baseErrorMessage);
                }
            } else if (error.request) {
                pushError(baseErrorMessage);
            } else {
                pushError(baseErrorMessage);
            }
            return Promise.reject(error);
        }
    );

    
    var hasLoadConfig = false;
    const config = localStorage.getItem("config");
    if (config) {
        const json = JSON.parse(config);
        window.config = json.data;
        const timestamp = new Date().getTime();
        if (json.timestamp && timestamp - json.timestamp < 60 * 1000 * 60) {
            hasLoadConfig = true;
        }
    }
    if (!hasLoadConfig) {
        axios.get('configuration')
            .then(data => {
                window.config = data;
                const config = {
                    data: data,
                    timestamp: new Date().getTime()
                };
                localStorage.setItem("config", JSON.stringify(config));
            });
    }


    window.username = localStorage.getItem("username");
    if (!window.username && window.location.pathname.split('/').pop() != "login") {
        location.href = "/admin/login";
    }
});



window.logout = function () {
    axios.post('logout', this.user)
        .then(data => {
            location.href = "/admin";
        })
}

window.handleFileChange = function (file, scuess) {
    const that = this;
    axios.post('Storages/token')
        .then(data => {

            switch (data.storageType) {
                case "Qiniu":
                    that.uploadQiniu(file, data.token, null, function (res) {
                        const url = data.prefix + res.key;
                        if (scuess) {
                            scuess(url);
                        };
                    });
                    break;
                case "Local":
                    uploadLocal(file, function (res) {
                        const url = data.prefix + res.key;
                        if (scuess) {
                            scuess(url);
                        };
                    });
                    break;

                default:
                    pushError("文件存储配置错误，请检查！");
                    break;
            }

        })
        .catch(error => {
            pushError("上传过程中出现错误！");
        });
}

window.uploadLocal = function (file, scuess) {
    const formData = new FormData();
    formData.append('file', file); 

    axios.post('Storages/upload', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        .then(data => {
            if (scuess) {
                scuess(data);
            }
        })
}

window.uploadQiniu = function (file, token, key, scuess) {

    const config = {
        useCdnDomain: true
    };

    const putExtra = {
        fname: "",
        params: {},
        mimeType: ["image/png", "image/jpeg", "image/gif"]
    };

    const observable = qiniu.upload(file, key, token, putExtra, config);

    const observer = {
        next(res) {
            //console.log("next:", res)
        },
        error(err) {
            //console.log("error:", res)
        },
        complete(res) {
            if (scuess) {
                scuess(res);
            }
        }
    };
    const subscription = observable.subscribe(observer);
}


var timeout;
window.pushError = function(errorMessage) {
    clearTimeout(timeout);
    Alpine.store('errorMessage', errorMessage);
    timeout = setTimeout(function () {
        Alpine.store('errorMessage', '');
    }, 5000);
}

window.friendlyTime = function(time) {
    const now = new Date();
    const past = new Date(time);
    const diffInSeconds = Math.floor((now - past) / 1000);

    if (diffInSeconds < 60) {
        return '刚刚';
    } else if (diffInSeconds < 3600) {
        const minutes = Math.floor(diffInSeconds / 60);
        return `${minutes}分钟前`;
    } else if (diffInSeconds < 86400) {
        const hours = Math.floor(diffInSeconds / 3600);
        return `${hours}小时前`;
    } else if (diffInSeconds < 604800) {
        const days = Math.floor(diffInSeconds / 86400);
        return `${days}天前`;
    } else {
        const weeks = Math.floor(diffInSeconds / 604800);
        return `${weeks}周前`;
    }
}

window.getUrlParameter = function (name) {
    name = name.replace(/[\[\]]/g, '\\$&'); 
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(window.location.search);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

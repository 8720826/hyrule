document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {
        return {
            user: {
                name: "",
                password: ""
            },
            login() {
                axios.post('login', this.user)
                    .then(data => {
                        localStorage.setItem("username", this.user.name);
                        localStorage.setItem("token", data.token);
                        location.href = "/admin";
                    })
            },
            init() {
                window.addEventListener('load', () => {
                    document.title = "登录到 - " + window.config.name + "";
                });
            }
        };
    })
});



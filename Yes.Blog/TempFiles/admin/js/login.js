document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {
        return {
            user: {
            },
            login() {
                axios.post('login', this.user)
                    .then(data => {
                        localStorage.setItem("username", this.user.name);
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



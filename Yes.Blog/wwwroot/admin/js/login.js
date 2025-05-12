document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {
        return {
            user: {
                name: "",
                password: ""
            },
            validate() {
                axios.post('validate')
                    .then(data => {
                        console.log('data:', data);
                        if (data.isValid) {
                            location.href = "/admin";
                        }
                    })
            },
            login() {
                axios.post('login', this.user)
                    .then(data => {
                        console.log('data:', data);
                        localStorage.setItem("username", this.user.name);
                        localStorage.setItem("token", data.token);
                        location.href = "/admin";
                    })
            },
            init() {
                this.validate();

                window.addEventListener('load', () => {
                    document.title = "登录到 - " + window.config.name + "";
                });
            }
        };
    })
});



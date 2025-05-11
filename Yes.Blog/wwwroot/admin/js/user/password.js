document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {

        const id = getUrlParameter("id");
        return {
            user: {
            },
            isSubmitting: false,
            loadUser() {
                axios.get('users/profile')
                    .then(data => {
                        this.user = data;
                    });
                return this;
            },
            editUser() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.put('users/password', this.user)
                    .then(data => {
                        location.href = "/admin/user/password";
                    })
                    .finally(() => {
                        this.isSubmitting = false;
                    });
            },
            init() {
                this.loadUser();

                window.addEventListener('load', () => {
                    document.title = "修改密码 - " + window.config.name + "";
                });
            }
        };
    })
});
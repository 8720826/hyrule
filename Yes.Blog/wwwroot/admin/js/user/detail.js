document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {

        const id = getUrlParameter("id");
        return {
            user: {
            },
            isSubmitting: false,
            loadUser() {
                axios.get('users/' + id)
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
                axios.put('users', this.user)
                    .then(data => {
                        location.href = "/admin/user";
                    })
                    .finally(() => {
                        this.isSubmitting = false;
                    });
            },
            init() {
                this.loadUser();

                window.addEventListener('load', () => {
                    document.title = "编辑管理员 - " + window.config.name + "";
                });
            }
        };
    })
});
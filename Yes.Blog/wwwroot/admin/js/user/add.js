document.addEventListener('alpine:init', function () {
    Alpine.data('data', function () {

        return {
            user: {
            },
            isSubmitting: false,
            addUser() {
                if (this.isSubmitting) {
                    return;
                }
                this.isSubmitting = true;
                axios.post('users', this.user)
                    .then(data => {
                        location.href = "/admin/user";
                    })
                    .finally(() => {
                        this.isSubmitting = false;
                    });
            },
            init() {
                window.addEventListener('load', () => {
                    document.title = "添加管理员 - " + window.config.name + "";
                });
            }
        };
    })
});
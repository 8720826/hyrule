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
                axios.put('users/profile', this.user)
                    .then(data => {
                        location.href = "/admin/user/profile";
                    })
                    .finally(() => {
                        this.isSubmitting = false;
                    });
            },
            init() {
                this.loadUser();

                window.addEventListener('load', () => {
                    document.title = "个人资料 - " + window.config.name + "";
                });
            }
        };
    })
});
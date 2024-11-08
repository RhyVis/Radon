import "@/assets/style/mixin.less";
import "@/assets/style/style.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "tdesign-vue-next/es/style/index.css";

import { createApp } from "vue";

import App from "@/App.vue";
import i18n from "@/locale";
import router from "@/router";
import store from "@/store";

const app = createApp(App);

app.use(router);
app.use(store);
app.use(i18n);

app.mount("#app");

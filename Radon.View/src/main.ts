import "@/assets/style/mixin.less";
import "@/assets/style/style.css";
import "tailwindcss/tailwind.css";
import "tdesign-vue-next/es/style/index.css";

import { createApp } from "vue";

import App from "@/App.vue";
import { printBanner } from "@/lib/util/banner.ts";
import { setupWindowListener } from "@/lib/util/windowListener";
import i18n from "@/locale";
import router from "@/router";
import store from "@/store";
import { registerSW } from "virtual:pwa-register";

console.log("RN: main");
printBanner();

registerSW({ immediate: true });

const app = createApp(App);

app.use(router);
app.use(store);
app.use(i18n);

app.mount("#app");

setupWindowListener();

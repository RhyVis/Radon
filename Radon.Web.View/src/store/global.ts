import { defineStore } from "pinia";

export const useGlobalStore = defineStore("global", {
  state: () => ({
    asideVisible: false,
    authShow: false,
    fontLoaded: false,
    locale: "zh-CN",
  }),
  persist: {
    pick: ["authPassed", "locale"],
  },
});

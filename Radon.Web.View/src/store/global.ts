import { defineStore } from "pinia";

export const useGlobalStore = defineStore("global", {
  state: () => ({
    asideVisible: false,
    authShow: false,
    fontLoaded: false,
  }),
  persist: {
    pick: ["authPassed"],
  },
});

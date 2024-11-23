import { defineStore } from "pinia";

export type GlobalStore = {
  asideVisible: boolean;
  authShow: boolean;
  authPassed: boolean;
  fontLoaded: boolean;
  locale: string;
};

export const useGlobalStore = defineStore("global", {
  state: (): GlobalStore => ({
    asideVisible: false,
    authShow: false,
    authPassed: false,
    fontLoaded: false,
    locale: "zh-CN",
  }),
  persist: {
    pick: ["authPassed", "locale"],
  },
});

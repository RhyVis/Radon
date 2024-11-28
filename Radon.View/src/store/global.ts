import { defineStore } from "pinia";

export type GlobalStore = {
  asideVisible: boolean;
  authShow: boolean;
  authPassed: boolean;
  fontLoaded: boolean;
  darkTheme: boolean;
  locale: string;
};

export const useGlobalStore = defineStore("global", {
  state: (): GlobalStore => ({
    asideVisible: false,
    authShow: false,
    authPassed: false,
    fontLoaded: false,
    darkTheme: false,
    locale: "zh-CN",
  }),
  getters: {
    localeStandard: state => (state.locale === "zh-CN" ? "zh-CN" : "en-US"),
  },
  persist: {
    pick: ["authPassed", "locale"],
  },
});

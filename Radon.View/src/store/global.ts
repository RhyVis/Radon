import { radixValExtended } from "@/pages/math/radix/scripts/radix";
import { defineStore } from "pinia";

export type GlobalStore = {
  asideVisible: boolean;
  authShow: boolean;
  authPassed: boolean;
  fontLoaded: boolean;
  vRemote: number;
  vState: number;
  vServer: string;
  vServerState: 0 | -1 | 1 | -999;
  locale: string;
};

export const useGlobalStore = defineStore("global", {
  state: (): GlobalStore => ({
    asideVisible: false,
    authShow: false,
    authPassed: false,
    fontLoaded: false,
    vRemote: -1,
    vState: -2,
    vServer: "",
    vServerState: 0,
    locale: "zh-CN",
  }),
  getters: {
    vRemoteShort: state => (state.vRemote === -1 ? "?" : radixValExtended(state.vRemote)),
    needUpdate: state => state.vState > 0,
  },
  persist: {
    pick: ["authPassed", "locale"],
  },
});

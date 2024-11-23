import LocalVersion from "@/assets/local/version.json";
import { getClientCompileTimeRemote, getClientVersionRemote, getServerVersion } from "@/lib/common/apiMethods.ts";
import { formatFromTimestamp } from "@/lib/util/dateFormatter.ts";
import { radixValExtended } from "@/pages/math/radix/scripts/radix.ts";
import { get, useOnline } from "@vueuse/core";
import { defineStore } from "pinia";

type VersionState = {
  cCompileTimeL: number;
  cCompileTimeR: number;
  cVersionL: string;
  cVersionR: string;
  sVersion: string;
  fetchState: FetchState;
  initialized: boolean;
};

export const enum FetchState {
  INIT = 0,
  SUCCESS = 1,
  NEED_UPDATE = 2,
  ERROR = -1,
  NOT_ONLINE = -2,
}

export const useVersionStore = defineStore("version", {
  state: (): VersionState => ({
    cCompileTimeL: LocalVersion.compileTime as number,
    cCompileTimeR: 0,
    cVersionL: LocalVersion.clientVersion as string,
    cVersionR: "0.0.0",
    sVersion: "0.0.0",
    fetchState: FetchState.INIT,
    initialized: false,
  }),
  getters: {
    cAssembleTimeL(state) {
      return `${state.cVersionL}.${radixValExtended(state.cCompileTimeL)}`;
    },
    cAssembleTimeR(state) {
      return `${state.cVersionR}.${radixValExtended(state.cCompileTimeR)}`;
    },
    cCompileTimeRFormatted(state) {
      return formatFromTimestamp(state.cCompileTimeR);
    },
    needUpdate(state) {
      return state.fetchState === FetchState.NEED_UPDATE;
    },
    badState(state) {
      return state.fetchState === FetchState.ERROR || state.fetchState === FetchState.NOT_ONLINE;
    },
  },
  actions: {
    init() {
      if (!get(useOnline())) {
        this.fetchState = FetchState.NOT_ONLINE;
        return;
      }

      try {
        getClientCompileTimeRemote()
          .then(time => (this.cCompileTimeR = time))
          .then(() => {
            if (this.cCompileTimeL != this.cCompileTimeR) {
              this.fetchState = FetchState.NEED_UPDATE;
            } else {
              this.fetchState = FetchState.SUCCESS;
            }
          });
        getClientVersionRemote().then(version => (this.cVersionR = version));
        getServerVersion().then(version => (this.sVersion = version));
      } catch (e) {
        console.error(e);
        this.fetchState = FetchState.ERROR;
      } finally {
        this.initialized = true;
      }
    },
  },
});

import LocalVersion from "@/assets/local/version.json";
import { getClientCompileTimeRemote, getClientVersionRemote, getServerVersion } from "@/lib/common/apiMethods.ts";
import { radixValExtended } from "@/pages/math/radix/scripts/radix.ts";
import { get, useOnline } from "@vueuse/core";
import { defineStore } from "pinia";

type VersionFile = {
  compileTime: number;
  clientVersion: string;
};

type VersionState = {
  cCompileTimeL: number;
  cCompileTimeR: number;
  cVersionL: string;
  cVersionR: string;
  sVersion: string;
  fetchState: FetchState;
  initialized: boolean;
};

const localVersion: VersionFile = LocalVersion;

export const enum FetchState {
  INIT = 0,
  SUCCESS = 1,
  NEED_UPDATE = 2,
  ERROR = -1,
  NOT_ONLINE = -2,
}

export const useVersionStore = defineStore("version", {
  state: (): VersionState => ({
    cCompileTimeL: localVersion.compileTime,
    cCompileTimeR: 0,
    cVersionL: localVersion.clientVersion,
    cVersionR: "0.0.0",
    sVersion: "0.0.0",
    fetchState: 0,
    initialized: false,
  }),
  getters: {
    cAssembleTimeL: state => `${state.cVersionL}.${radixValExtended(state.cCompileTimeL)}`,
    cAssembleTimeR: state => `${state.cVersionR}.${radixValExtended(state.cCompileTimeR)}`,
    cCompileTimeRFormatted: state => new Date(state.cCompileTimeR).toLocaleString(),
    needUpdate: state => state.fetchState == FetchState.NEED_UPDATE,
    badState: state => state.fetchState < 0,
  },
  actions: {
    async init() {
      if (!get(useOnline())) {
        this.fetchState = FetchState.NOT_ONLINE;
        return;
      }

      await getClientCompileTimeRemote()
        .then(time => (this.cCompileTimeR = time))
        .then(() => {
          if (this.cCompileTimeL != this.cCompileTimeR) {
            this.fetchState = FetchState.NEED_UPDATE;
          } else {
            this.fetchState = FetchState.SUCCESS;
          }
        })
        .catch(e => {
          console.error(e);
          this.fetchState = FetchState.ERROR;
        });
      await getClientVersionRemote()
        .then(version => (this.cVersionR = version))
        .catch(e => {
          console.error(e);
          this.fetchState = FetchState.ERROR;
        });
      await getServerVersion()
        .then(version => (this.sVersion = version))
        .catch(e => {
          console.error(e);
          this.fetchState = FetchState.ERROR;
        });

      this.initialized = true;
    },
  },
});

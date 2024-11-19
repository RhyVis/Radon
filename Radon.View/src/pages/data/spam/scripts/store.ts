import { defineStore } from "pinia";

type SpamStore = {
  qType: string;
  qDict: string;
  qSize: number;
  qIds: string[];
  aType: string;
  aText: string;
  activeTab: string;
};

type Query = {
  type: string;
  dict: string;
  size: number;
};
type QueryPrecise = {
  type: string;
  dict: string;
  ids: number[];
};
type QueryAppend = {
  type: string;
  text: string;
};

export const useSpamStore = defineStore("spam", {
  state: (): SpamStore => ({
    qType: "sn",
    qDict: "none",
    qSize: 1,
    qIds: [],
    aType: "sn",
    aText: "",
    activeTab: "spam",
  }),
  getters: {
    query(state): Query {
      return {
        type: state.qType,
        dict: state.qDict,
        size: state.qSize,
      };
    },
    queryPrecise(state): QueryPrecise {
      return {
        type: state.qType,
        dict: state.qDict,
        ids: state.qIds.map(id => Number(id)),
      };
    },
    queryAppend(state): QueryAppend {
      return {
        type: state.aType,
        text: state.aText,
      };
    },
  },
  actions: {
    clearAppendQuery() {
      this.aText = "";
    },
  },
  persist: {
    pick: ["qType", "qDict", "qSize", "qIds", "aType", "activeTab"],
  },
});

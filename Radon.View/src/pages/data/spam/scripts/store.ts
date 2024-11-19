import { SpamType } from "@/pages/data/spam/scripts/define.ts";
import { defineStore } from "pinia";

type SpamStore = {
  qType: SpamType;
  qDict: string;
  qSize: number;
  qIds: string[];
  aType: SpamType;
  aText: string;
  activeTab: string;
};

type Query = {
  type: SpamType;
  dict: string;
  size: number;
};
type QueryPrecise = {
  type: SpamType;
  dict: string;
  ids: number[];
};
type QueryAppend = {
  type: SpamType;
  text: string;
};

export const useSpamStore = defineStore("spam", {
  state: (): SpamStore => ({
    qType: SpamType.SpamMin,
    qDict: "none",
    qSize: 1,
    qIds: [],
    aType: SpamType.SpamMin,
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

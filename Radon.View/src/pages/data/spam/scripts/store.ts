type Query = {
  type: string;
  dict: string;
  size: number;
};
type QueryAppend = {
  type: string;
  text: string;
};

export const useSpamStore = defineStore("spam", {
  state: () => ({
    qType: "sn",
    qDict: "none",
    qSize: 1,
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
    pick: ["qType", "qDict", "qSize", "aType", "activeTab"],
  },
});

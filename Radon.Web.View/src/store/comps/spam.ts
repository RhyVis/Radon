import { defineStore } from "pinia";

type Query = {
  type: string;
  dict: string;
  size: number;
};
type Conf = {
  activeTab: string;
};

export const useSpamStore = defineStore("spam", {
  state: () => ({
    type: "spam_min",
    dict: "none",
    size: 1,
    activeTab: "spam",
  }),
  actions: {
    update(query: Query, conf: Conf) {
      this.type = query.type;
      this.dict = query.dict;
      this.size = query.size;
      this.activeTab = conf.activeTab;
    },
  },
  persist: true,
});

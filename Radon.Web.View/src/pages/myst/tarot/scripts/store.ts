import { defineStore } from "pinia";

type Query = {
  deck: string;
  full: boolean;
  size: number;
};
type Conf = {
  activeTab: string;
  deckShowDesc: boolean;
};

export const useTarotStore = defineStore("tarot", {
  state: () => ({
    deck: "waite",
    full: true,
    size: 1,
    activeTab: "simple",
    deckShowDesc: true,
  }),
  actions: {
    update(query: Query, conf: Conf) {
      this.deck = query.deck;
      this.full = query.full;
      this.size = query.size;
      this.activeTab = conf.activeTab;
      this.deckShowDesc = conf.deckShowDesc;
    },
  },
});

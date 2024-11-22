import { apiGet } from "@/lib/common/apiMethods";
import type { CardDisplay, DeckInfo, DeckInfoInterface, DeckInfoSelect } from "@/pages/myst/tarot/scripts/define";
import { defineStore } from "pinia";

type TarotStore = {
  qDeck: string;
  qFull: boolean;
  qSize: number;
  activeTab: string;
  dShowDesc: boolean;
  dInfoMap: DeckInfoInterface;
  dInfoSelect: DeckInfoSelect[];
  dInfoLoaded: boolean;
  results: CardDisplay[];
};
type Query = {
  deck: string;
  full: boolean;
  size: number;
};

const mapOptions = (info: DeckInfo): DeckInfoSelect => ({
  value: info.name,
  label: info.loc,
});

export const useTarotStore = defineStore("tarot", {
  state: (): TarotStore => ({
    qDeck: "waite",
    qFull: true,
    qSize: 1,
    activeTab: "simple",
    dShowDesc: true,
    dInfoMap: {},
    dInfoSelect: [],
    dInfoLoaded: false,
    results: [],
  }),
  getters: {
    query(state): Query {
      return {
        deck: state.qDeck,
        full: state.qFull,
        size: state.qSize,
      };
    },
  },
  actions: {
    async init() {
      try {
        const fetch = (await apiGet<DeckInfoInterface>("/api/tarot/info")).data;
        this.dInfoMap = fetch;
        this.dInfoSelect = Object.values<DeckInfo>(fetch).map(mapOptions);
      } catch (e) {
        console.error(e);
      } finally {
        this.dInfoLoaded = true;
      }
    },
  },
  persist: {
    pick: ["qDeck", "qFull", "qSize", "activeTab", "dShowDesc"],
  },
});

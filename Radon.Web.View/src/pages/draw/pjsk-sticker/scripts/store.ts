import { defineStore } from "pinia";

export const usePjskStore = defineStore("pjsk-sticker", {
  state: () => ({
    charaId: 0,
    useCommercialFonts: false,
  }),
  persist: {
    pick: ["charaId", "useCommercialFonts"],
  },
});

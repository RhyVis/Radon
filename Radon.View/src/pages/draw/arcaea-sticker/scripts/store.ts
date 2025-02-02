﻿import { defineStore } from "pinia";

export type ArcaeaStickerStore = {
  charaId: number;
  fontSize: number;
  spaceSize: number;
  rotate: number;
  x: number;
  y: number;
  text: string;
  curve: boolean;
  useCommercialFonts: boolean;
};

export const useArcaeaStickerStore = defineStore("arcaea-sticker", {
  state: (): ArcaeaStickerStore => ({
    charaId: 0,
    fontSize: 1,
    spaceSize: 1,
    rotate: 0,
    x: 0,
    y: 0,
    text: "",
    curve: false,
    useCommercialFonts: false,
  }),
  persist: {
    pick: ["charaId", "useCommercialFonts"],
  },
});

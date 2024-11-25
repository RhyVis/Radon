import type { MdIndexBrief } from "@/pages/with/markdown/define.ts";
import { defineStore } from "pinia";

export type MdState = {
  indexList: MdIndexBrief[];
};

export const useMdStore = defineStore("markdown", {
  state: (): MdState => ({
    indexList: [],
  }),
  actions: {},
});

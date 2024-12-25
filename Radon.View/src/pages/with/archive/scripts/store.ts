import type { MdIndexDto } from "@/pages/with/archive/scripts/define.ts";
import { defineStore } from "pinia";

export type MdState = {
  indexList: MdIndexDto[];
};

export const useMdStore = defineStore("markdown", {
  state: (): MdState => ({
    indexList: [],
  }),
  actions: {},
});

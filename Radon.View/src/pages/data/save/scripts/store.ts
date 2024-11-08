import { defineStore } from "pinia";

export const useSaveStore = defineStore("save", {
  state: () => ({
    id: 0,
  }),
  actions: {
    update(id: number) {
      this.id = id;
    },
  },
});

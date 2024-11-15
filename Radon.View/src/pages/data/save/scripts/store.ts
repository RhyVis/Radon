import { defineStore } from "pinia";

type SaveStore = {
  id: number;
  qText: string;
  qNote: string;
  rText: string;
  rNote: string;
  loading: boolean;
};

export const useSaveStore = defineStore("save", {
  state: (): SaveStore => ({
    id: 0,
    qText: "",
    qNote: "",
    rText: "",
    rNote: "",
    loading: false,
  }),
  getters: {
    query(state) {
      return {
        id: state.id,
        text: state.qText,
        note: state.qNote,
      };
    },
  },
});

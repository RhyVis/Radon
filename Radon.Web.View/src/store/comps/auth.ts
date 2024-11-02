import { validateToken } from "@/lib/util/authFunction";
import { defineStore } from "pinia";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: "",
    valid: false,
  }),
  getters: {
    async validate(state) {
      try {
        state.valid = await validateToken(state.token);
        return state.valid;
      } catch (e) {
        console.error(e);
        state.valid = false;
        return state.valid;
      }
    },
  },
  actions: {
    updateToken(token: string) {
      this.token = token;
    },
  },
  persist: true,
});

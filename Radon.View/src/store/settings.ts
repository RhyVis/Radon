import { defineStore } from "pinia";

type SettingsState = {
  showLoginMessage: boolean;
};

export const useSettingStore = defineStore("settings", {
  state: (): SettingsState => ({
    showLoginMessage: true,
  }),
  persist: true,
});

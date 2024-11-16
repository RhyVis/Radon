import { apiGet } from "@/lib/common/apiMethods";
import type { NavData } from "@/pages/data/nav/scripts/define";
import { defineStore } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import { useI18n } from "vue-i18n";

interface NavState {
  navDataList: NavData[];
  navLoaded: boolean;
}

export const useNavStore = defineStore("navigation", {
  state: (): NavState => ({
    navDataList: [],
    navLoaded: false,
  }),
  actions: {
    async fetch() {
      try {
        this.navDataList = (await apiGet<NavData[]>("/api/nav")).data;
      } catch (e) {
        console.error(e);
        await MessagePlugin.error(useI18n().t("common.fetchError"));
      } finally {
        this.navLoaded = true;
      }
    },
  },
});

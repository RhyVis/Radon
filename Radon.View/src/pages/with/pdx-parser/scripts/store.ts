import { apiGetStr, apiPutState } from "@/lib/common/apiMethods.ts";
import i18n from "@/locale";
import type { PdxReplacerEntry } from "@/pages/with/pdx-parser/scripts/define.ts";
import { defineStore } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";

const { t } = i18n.global;

interface PdxStore {
  replacer: PdxReplacerEntry;
  addReplacerKey: string;
  addReplacerValue: string;
  requestTextStorageId: number;
  eventSelectId: number;
}

export const usePdxStore = defineStore("pdx-parser", {
  state: (): PdxStore => ({
    replacer: {
      DEFAULT: "默认",
    },
    addReplacerKey: "",
    addReplacerValue: "",
    requestTextStorageId: 0,
    eventSelectId: 0,
  }),
  actions: {
    updateReplacer() {
      if (this.addReplacerKey.length === 0 || this.addReplacerValue.length === 0)
        return MessagePlugin.warning(t("common.noEmpty"));
      this.replacer[this.addReplacerKey] = this.addReplacerValue;
      this.addReplacerKey = "";
      this.addReplacerValue = "";
    },
    removeReplacer(key: string) {
      const copy = JSON.parse(JSON.stringify(this.replacer));
      delete copy[key];
      this.replacer = copy;
    },
    getSyncReplacer() {
      try {
        apiGetStr("api/pdx/parse/lang/replacer")
          .then(res => {
            this.replacer = JSON.parse(res.data);
          })
          .then(() => {
            void MessagePlugin.success(t("common.syncSuccess"));
          });
      } catch (e) {
        console.error(e);
      }
    },
    setSyncReplacer() {
      const json = JSON.stringify(this.replacer);
      apiPutState("api/pdx/parse/lang/replacer", json).then(() => {
        void MessagePlugin.success(t("common.syncSuccess"));
      });
    },
  },
  persist: {
    pick: ["replacer", "requestTextStorageId"],
  },
});

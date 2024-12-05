import { apiGetStr, apiPutState } from "@/lib/common/apiMethods";
import i18n from "@/locale";
import type { PdxReplacerEntry } from "@/pages/util/pdx-parser/scripts/define";
import { defineStore } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";

const { t } = i18n.global;

interface PdxStore {
  initialized: boolean;
  replacer: PdxReplacerEntry;
  addReplacerKey: string;
  addReplacerValue: string;
  requestTextStorageId: number;
}

export const usePdxStore = defineStore("pdx-parser", {
  state: (): PdxStore => ({
    initialized: false,
    replacer: {
      DEFAULT: "默认",
    },
    addReplacerKey: "",
    addReplacerValue: "",
    requestTextStorageId: 0,
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

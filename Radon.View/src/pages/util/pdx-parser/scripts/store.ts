import { apiGet, apiGetStr, apiPutState } from "@/lib/common/apiMethods";
import i18n from "@/locale";
import type { PdxParsedLangItem } from "@/pages/util/pdx-parser/scripts/type";
import { defineStore } from "pinia";
import type { TreeProps } from "tdesign-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const { t } = i18n.global;

interface ReplaceEntry {
  [key: string]: string;
}
interface PdxStore {
  initialized: boolean;
  parseLangResult: TreeProps["data"];
  replacer: ReplaceEntry;
  addReplacerKey: string;
  addReplacerValue: string;
}

const buildTree = (data: PdxParsedLangItem[]) => {
  const root: TreeProps["data"] = [];

  data.forEach(item => {
    let currentLevel = root;
    item.namespace.forEach((name, index) => {
      let existingNode = currentLevel.find(node => node.label === name);
      if (!existingNode) {
        existingNode = { label: name, children: [] };
        currentLevel.push(existingNode);
      }
      if (index === item.namespace.length - 1) {
        existingNode.label = name;
        existingNode.value = item.value;
      }
      currentLevel = (existingNode.children as TreeProps["data"])!;
    });
  });

  return root;
};

export const usePdxStore = defineStore("pdx-parser", {
  state: (): PdxStore => ({
    initialized: false,
    parseLangResult: [],
    replacer: {
      DEFAULT: "默认",
    },
    addReplacerKey: "",
    addReplacerValue: "",
  }),
  actions: {
    init() {
      apiGet<PdxParsedLangItem[]>("api/pdx/test").then(res => {
        this.updateParsedLang(res.data);
      });
      this.initialized = true;
    },
    updateParsedLang(data: PdxParsedLangItem[]) {
      this.parseLangResult = buildTree(data);
    },
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
    pick: ["replacer"],
  },
});

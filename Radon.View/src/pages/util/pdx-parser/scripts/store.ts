import type { PdxParsedLangItem } from "@/pages/util/pdx-parser/scripts/type";
import type { TreeProps } from "tdesign-vue-next";

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
      this.replacer[this.addReplacerKey] = this.addReplacerValue;
      this.addReplacerKey = "";
      this.addReplacerValue = "";
    },
  },
  persist: {
    pick: ["replacer"],
  },
});

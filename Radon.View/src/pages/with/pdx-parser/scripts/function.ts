import { apiGet } from "@/lib/common/apiMethods.ts";
import type { PdxLangEventItem, PdxLangItem, PdxReplacerEntry } from "@/pages/with/pdx-parser/scripts/define.ts";
import type { TreeProps } from "tdesign-vue-next";

export const getPdxLangParsedItemById = async (id: number) => {
  const { data } = await apiGet<PdxLangItem[]>(`/api/pdx/parse/lang/${id}`, { timeout: 120000 });
  return data;
};

export const getPdxLangParsedEventItemById = async (id: number) => {
  const { data } = await apiGet<PdxLangEventItem[]>(`/api/pdx/parse/event/${id}`, { timeout: 120000 });
  return data;
};

export const buildPdxLangTree = (data: PdxLangItem[]) => {
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

export const sepTextContent = (raw: string) => raw.replace(/(\\n)+|(\n)+/gi, "%!%").split("%!%");

export const renderPdxColor = (raw: string, replacer: PdxReplacerEntry, dark: boolean) => {
  const darkStr = dark ? "-dark" : "";
  return raw
    .replace(/\\"/g, '"')
    .replace(/§([beghlmprstwy])/gi, (_, p1) => `<span class='r-pdx-c${darkStr}-${p1.toLowerCase()}'>`)
    .replace(/§!/g, "</span>")
    .replace(/\$(\w+)\$/gi, (_, key) => `<span style="font-weight: bold">${replacer[key] ?? "(" + key + ")"}</span>`)
    .replace(/\[(.+?)]/gi, (_, key) => `<span>${replacer[key] ?? "[" + key + "]"}</span>`);
};

export const replacePdxAlias = (raw: string, replacer: PdxReplacerEntry) => {
  return raw
    .replace(/\\"/g, '"')
    .replace(/§([beghlmprstwy])/gi, "")
    .replace(/§!/g, "")
    .replace(/\$(\w+)\$/gi, (_, key) => `${replacer[key] ?? "(" + key + ")"}`)
    .replace(/\[(.+?)]/gi, (_, key) => `${replacer[key] ?? "[" + key + "]"}`);
};

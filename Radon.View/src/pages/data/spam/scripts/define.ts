import { EscapeType } from "@/pages/util/codex/scripts/define.ts";
import { Space, type TableProps, Tag, Text } from "tdesign-vue-next";

export type TextEntry = {
  id: number;
  text: string;
};

export enum SpamType {
  N = "n",
  Arknights = "ak",
  Genshin = "gs",
  ML = "ml",
  SpamMin = "sn",
  SpamMax = "sx",
  ACGN = "ac",
  Dinner = "dn",
}

export const spamTypes = Object.entries(SpamType).map(([label, value]) => ({ value, label }));

export const codeTypes = [
  { value: EscapeType.NONE, label: "直白对决😅" },
  { value: EscapeType.NMSL, label: "抽象加密🤗" },
  { value: EscapeType.TRAD, label: "繁体传统🤔" },
  { value: EscapeType.SPRK, label: "火星密文😘" },
  { value: EscapeType.DIFF, label: "形近转换🧐" },
];

export const spamColumns: (copyFn: (s: string) => void) => TableProps["columns"] = (copyFn: (s: string) => void) => {
  return [
    {
      colKey: "id",
      title: "ID",
      width: 60,
      cell: (h, { row }) => {
        return h(
          "div",
          {
            class: "r-sp-column-tag",
            onClick: () => copyFn(row.text),
          },
          [h(Tag, { shape: "round", variant: "outline" }, { default: () => row.id })],
        );
      },
    },
    {
      colKey: "text",
      title: "内容",
      cell: (h, { row }) => {
        return h(
          Space,
          { direction: "vertical", size: 2 },
          (row.text as string).split(/[\r\n]|\r\n|\\r\\n/).map((t, i) => h(Text, { key: i }, { default: () => t })),
        );
      },
    },
  ];
};

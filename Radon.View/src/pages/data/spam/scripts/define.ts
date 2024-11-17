import { Space, type TableProps, Tag, Text } from "tdesign-vue-next";

export type TextEntry = {
  id: number;
  text: string;
};

export const spamTypes = [
  { value: "ak", label: "Arknights" },
  { value: "gs", label: "Genshin" },
  { value: "ml", label: "ML" },
  { value: "sn", label: "Spam Min" },
  { value: "sx", label: "Spam Max" },
  { value: "ac", label: "ACGN" },
  { value: "dn", label: "Dinner" },
];

export const codeTypes = [
  { value: "none", label: "直白对决😅" },
  { value: "nmsl", label: "抽象加密🤗" },
  { value: "trad", label: "繁体传统🤔" },
  { value: "sprk", label: "火星密文😘" },
  { value: "diff", label: "形近转换🧐" },
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

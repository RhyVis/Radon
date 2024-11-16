import type { TableProps } from "tdesign-vue-next";
import { Link, Tag } from "tdesign-vue-next";
import { ref } from "vue";

type NavData = {
  id: number;
  data: string;
  label: string;
  note: string;
};

const navColumns = ref<TableProps["columns"]>([
  { colKey: "id", title: "ID", width: 40, align: "center" },
  {
    colKey: "label",
    title: "Label",
    width: 80,
    align: "center",
    cell: (h, { row }) => {
      return h(Tag, { shape: "round", variant: "outline", content: row.label });
    },
  },
  {
    colKey: "data",
    title: "Data",
    cell: (h, { row }) => {
      return h(Link, { size: "small", href: row.data }, { default: () => row.data });
    },
  },
  { colKey: "note", title: "Note", width: 100, ellipsis: true },
]);

export { navColumns };
export type { NavData };

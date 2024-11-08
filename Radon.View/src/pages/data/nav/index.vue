<script lang="tsx" setup>
import { useNavStore } from "@/pages/data/nav/scripts/store";
import type { TableProps } from "tdesign-vue-next";

const columns = ref<TableProps["columns"]>([
  { colKey: "id", title: "ID", width: 40, align: "center" },
  {
    colKey: "label",
    title: "Label",
    width: 80,
    align: "center",
    cell: (_, { row }) => {
      return <t-tag shape="round" variant="outline" content={row.label} />;
    },
  },
  {
    colKey: "data",
    title: "Data",
    cell: (_, { row }) => {
      return (
        <t-link size="small" href={row.data}>
          {row.data}
        </t-link>
      );
    },
  },
  { colKey: "note", title: "Note", width: 100, ellipsis: true },
]);
const store = useNavStore();
const { navDataList, navLoaded } = storeToRefs(store);
const { t } = useI18n();

onMounted(() => {
  if (store.navDataList.length === 0) {
    store.fetch();
  }
});
</script>

<template>
  <content-layout :title="t('tt')" :subtitle="t('st')">
    <t-table
      :bordered="true"
      :columns="columns"
      :data="navDataList"
      :loading="!navLoaded"
      :hover="true"
      :stripe="true"
      row-key="id"
      size="small"
    />
  </content-layout>
</template>

<i18n locale="en">
tt: Navigation
st: Memo
</i18n>

<i18n locale="zh-CN">
tt: 导航
st: 备忘录
</i18n>

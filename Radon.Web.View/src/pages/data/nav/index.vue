<script lang="tsx" setup>
import ContentLayout from "@/layout/frame/ContentLayout.vue";
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

onMounted(() => {
  if (store.navDataList.length === 0) {
    store.fetch();
  }
});
</script>

<template>
  <ContentLayout title="导航" subtitle="备忘录">
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
  </ContentLayout>
</template>

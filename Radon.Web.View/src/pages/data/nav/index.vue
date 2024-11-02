<script lang="tsx" setup>
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { ref } from "vue";
import { MessagePlugin, type TableProps } from "tdesign-vue-next";
import { apiGet } from "@/lib/util/apiMethods";
import type { NavData } from "@/pages/data/nav/scripts/navType";

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

const loading = ref(true);
const result = ref<NavData[]>([]);

try {
  apiGet("/api/nav")
    .then(res => {
      result.value = res.data as NavData[];
    })
    .then(() => {
      loading.value = false;
    });
} catch (e) {
  loading.value = false;
  console.error(e);
  MessagePlugin.error("获取导航信息失败");
}
</script>

<template>
  <ContentLayout title="导航" subtitle="备忘录">
    <t-table
      :bordered="true"
      :columns="columns"
      :data="result"
      :loading="loading"
      :hover="true"
      :stripe="true"
      row-key="id"
      size="small"
    />
  </ContentLayout>
</template>

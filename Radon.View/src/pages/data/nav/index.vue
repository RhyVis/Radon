<script lang="tsx" setup>
import { navColumns } from "@/pages/data/nav/scripts/define";
import { useNavStore } from "@/pages/data/nav/scripts/store";
import { storeToRefs } from "pinia";
import { onMounted } from "vue";
import { useI18n } from "vue-i18n";

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
      :columns="navColumns"
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

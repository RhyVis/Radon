<script setup lang="ts">
import { FetchState, useVersionStore } from "@/store/version.ts";
import { get } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const version = useVersionStore();
const { t } = useI18n();
const { fetchState, sVersion } = storeToRefs(version);

const tagTheme = computed(() => {
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return "success";
    case FetchState.NOT_ONLINE:
      return "warning";
    case FetchState.ERROR:
      return "danger";
    default:
      return "default";
  }
});
const tagIcon = computed(() => {
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return "check-circle";
    case FetchState.NOT_ONLINE:
      return "info-circle";
    case FetchState.ERROR:
      return "error-circle";
    default:
      return "help-circle";
  }
});
const tagValue = computed(() => {
  const prefix = t("serverVersion");
  switch (fetchState.value) {
    case FetchState.INIT:
      return prefix + t("await");
    case FetchState.SUCCESS:
      return prefix + get(sVersion);
    case FetchState.ERROR:
      return prefix + t("error");
    case FetchState.NOT_ONLINE:
      return prefix + t("local");
    default:
      return prefix + t("await");
  }
});
</script>

<template>
  <t-tag :theme="tagTheme" size="small" variant="outline">
    <template #icon>
      <t-icon :name="tagIcon" />
    </template>
    <span class="r-no-select">{{ tagValue }}</span>
  </t-tag>
</template>

<i18n locale="en">
serverVersion: "Server Version: "
await: Await
error: Error
local: Client Offline
</i18n>

<i18n locale="zh-CN">
serverVersion: "服务端版本: "
await: 等待中
error: 错误
local: 客户端离线
</i18n>

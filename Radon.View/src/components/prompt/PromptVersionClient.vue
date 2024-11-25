<script setup lang="ts">
import { FetchState, useVersionStore } from "@/store/version.ts";
import { get } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const version = useVersionStore();
const { t } = useI18n();
const { fetchState, cAssembleTimeL, cAssembleTimeR } = storeToRefs(version);

const tagTheme = computed(() => {
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return "success";
    case FetchState.NEED_UPDATE | FetchState.NOT_ONLINE:
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
    case FetchState.NEED_UPDATE | FetchState.NOT_ONLINE:
      return "info-circle";
    case FetchState.ERROR:
      return "error-circle";
    default:
      return "help-circle";
  }
});
const tagValue = computed(() => {
  const prefix = t("clientVersion");
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return prefix + get(cAssembleTimeL);
    case FetchState.NEED_UPDATE:
      return prefix + get(cAssembleTimeL) + " -> " + get(cAssembleTimeR);
    case FetchState.ERROR:
      return prefix + t("display.error");
    case FetchState.NOT_ONLINE:
      return prefix + t("display.local");
    case FetchState.INIT:
      return prefix + get(cAssembleTimeL) + t("display.wait");
    default:
      return prefix + t("display.wait");
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

<i18n lang="yaml" locale="en">
clientVersion: "Client Version: "
display:
  error: "Error"
  wait: "Waiting"
  local: "(Local)"
msg:
  update: "Need to update"
  error: "Version fetch failed"
  comm-error: "Communication with server failed"
</i18n>

<i18n lang="yaml" locale="zh-CN">
clientVersion: "客户端版本: "
display:
  error: "错误"
  wait: "等待中"
  local: "(本地)"
msg:
  update: "需要更新"
  error: "版本获取失败"
  comm-error: "与服务器通信失败"
</i18n>

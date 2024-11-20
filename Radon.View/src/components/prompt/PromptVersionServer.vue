<script setup lang="ts">
import { getServerVersion } from "@/lib/common/apiMethods.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useOnline } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const { vServer, vServerState } = storeToRefs(useGlobalStore());
const online = useOnline();

const tagTheme = computed(() => {
  switch (get(vServerState)) {
    case 0:
      return "default";
    case 1:
      return "success";
    case -1:
      return "danger";
    default:
      return "default";
  }
});
const tagIcon = computed(() => {
  switch (get(vServerState)) {
    case 0:
      return "help-circle";
    case 1:
      return "check-circle";
    case -1:
      return "error-circle";
    default:
      return "help-circle";
  }
});
const tagValue = computed(() => {
  switch (get(vServerState)) {
    case 0:
      return t("serverVersion") + t("await");
    case 1:
      return t("serverVersion") + get(vServer);
    case -1:
      return t("serverVersion") + t("error");
    case -999:
      return t("serverVersion") + t("local");
    default:
      return t("serverVersion") + t("await");
  }
});

const fetchVersion = async () => {
  try {
    set(vServer, await getServerVersion());
    set(vServerState, 1);
  } catch (e) {
    console.error(e);
    set(vServerState, -1);
  }
};

onMounted(() => {
  if (!get(online)) {
    set(vServerState, -999);
    return;
  }
  if (get(vServerState) <= 0) {
    fetchVersion();
  }
});
</script>

<template>
  <t-tag :theme="tagTheme" size="small">
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
serverVersion: "服务器版本："
await: 等待中
error: 错误
local: 客户端离线
</i18n>

﻿<script setup lang="ts">
import { getServerVersion } from "@/lib/common/apiMethods.ts";
import { get, useOnline, useToggle } from "@vueuse/core";
import { WifiIcon, WifiOffIcon } from "tdesign-icons-vue-next";
import { computed, onMounted } from "vue";
import { useI18n } from "vue-i18n";

const online = useOnline();
const { t } = useI18n();
const [available, setAvailable] = useToggle(false);
const status = computed(() => get(online) && get(available));
const tagTheme = computed(() => {
  if (get(status)) {
    return "success";
  } else {
    return "default";
  }
});
const tagValue = computed(() => {
  if (get(status)) {
    return t("serverStatus") + t("online");
  } else {
    return t("serverStatus") + t("offline");
  }
});

const checkServer = async () => {
  try {
    await getServerVersion();
    setAvailable(true);
  } catch (e) {
    console.error(e);
    setAvailable(false);
  }
};

onMounted(() => {
  checkServer();
});
</script>

<template>
  <t-tag :theme="tagTheme" size="small" variant="outline">
    <template #icon>
      <WifiIcon v-if="online" />
      <WifiOffIcon v-else />
    </template>
    <span class="r-no-select">{{ tagValue }}</span>
  </t-tag>
</template>

<i18n locale="en">
serverStatus: "Server Status: "
online: "Online"
offline: "Offline"
</i18n>

<i18n locale="zh-CN">
serverStatus: "服务器: "
online: "在线"
offline: "离线"
</i18n>

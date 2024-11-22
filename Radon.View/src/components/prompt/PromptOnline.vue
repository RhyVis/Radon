<script setup lang="ts">
import { get, useOnline } from "@vueuse/core";
import { WifiIcon, WifiOffIcon } from "tdesign-icons-vue-next";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

const online = useOnline();
const onlineText = computed(() => (get(online) ? t("online") : t("offline")));
const tagTheme = computed(() => (get(online) ? "success" : "default"));
</script>

<template>
  <t-tag :theme="tagTheme" size="small" variant="outline">
    <template #icon>
      <WifiIcon v-if="online" />
      <WifiOffIcon v-else />
    </template>
    <span class="r-no-select">{{ t("networkStatus") + onlineText }}</span>
  </t-tag>
</template>

<i18n locale="en">
networkStatus: "Network: "
online: Online
offline: Offline
</i18n>

<i18n locale="zh-CN">
networkStatus: "网络："
online: 在线
offline: 离线
</i18n>

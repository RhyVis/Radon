<script setup lang="ts">
import VersionView from "@/assets/local/version.json";
import { getVersion } from "@/lib/common/apiMethods";
import { radixValExtended } from "@/pages/math/radix/scripts/radix";
import { useGlobalStore } from "@/store/global";
import { get, set, useOnline } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, onMounted } from "vue";
import { useI18n } from "vue-i18n";

const global = useGlobalStore();
const online = useOnline();
const { t } = useI18n();
const { vRemote, vState } = storeToRefs(global);

const vPackage = computed(() => import.meta.env.PACKAGE_VERSION);
const vLocal = VersionView.compileTime;

const vLocalShort = computed(() => radixValExtended(vLocal));
const vRemoteShort = computed(() => radixValExtended(get(vRemote)));

const tagTheme = computed(() => {
  switch (get(vState)) {
    case 0:
      return "success";
    case 1:
      return "warning";
    case -1:
      return "danger";
    default:
      return "default";
  }
});
const tagIcon = computed(() => {
  switch (get(vState)) {
    case 0:
      return "check-circle";
    case 1:
      return "info-circle";
    case -1:
      return "error-circle";
    default:
      return "help-circle";
  }
});
const tagValue = computed(() => {
  switch (get(vState)) {
    case 0:
      return `${t("clientVersion")}${get(vPackage)}.${get(vLocalShort)}`;
    case 1:
      return `${t("clientVersion")}${get(vPackage)}.${get(vLocalShort)} -> ${get(vPackage)}.${get(vRemoteShort)}`;
    case -1:
      return t("clientVersion") + t("display.error");
    case -999:
      return `${t("clientVersion")}${get(vPackage)}.${get(vLocalShort)} ${t("display.local")}`;
    default:
      return t("clientVersion") + t("display.wait");
  }
});

onMounted(async () => {
  if (!get(online)) {
    set(vState, -999);
    return;
  }
  if (get(vState) < 0) {
    try {
      const v = await getVersion();
      if (v != 0) {
        console.log(`vR: ${v} | vL: ${vLocal}`);
        set(vRemote, v);
        if (v != vLocal) {
          set(vState, 1);
          await MessagePlugin.warning(t("msg.update"));
        } else {
          set(vState, 0);
        }
      } else {
        set(vState, -1);
        await MessagePlugin.warning(t("msg.error"));
      }
    } catch (e) {
      console.error(e);
      set(vState, -1);
      await MessagePlugin.error(t("msg.comm-error"));
    } finally {
    }
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

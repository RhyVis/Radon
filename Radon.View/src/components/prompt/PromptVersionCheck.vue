<script setup lang="ts">
import VersionView from "@/assets/local/version.json";
import { getVersion } from "@/lib/common/apiMethods";
import { radixValExtended } from "@/pages/math/radix/scripts/radix";
import { useGlobalStore } from "@/store/global";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, onMounted } from "vue";
import { useI18n } from "vue-i18n";

const global = useGlobalStore();
const { t } = useI18n();
const { vRemote, vState } = storeToRefs(global);

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
      return `${get(vLocalShort)} ${t("display.latest")}`;
    case 1:
      return `${get(vLocalShort)} -> ${get(vRemoteShort)} ${t("display.update")}`;
    case -1:
      return t("display.error");
    default:
      return t("display.wait");
  }
});

onMounted(async () => {
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
  <t-tag :theme="tagTheme" size="small">
    <template #icon>
      <t-icon :name="tagIcon" />
    </template>
    <span class="r-no-select">{{ tagValue }}</span>
  </t-tag>
</template>

<i18n lang="yaml" locale="en">
display:
  latest: "Latest Version"
  update: "Update Required"
  error: "Version Fetch Failed"
  wait: "Waiting for Version Fetch"
msg:
  update: "Non-latest version"
  error: "Version fetch failed"
  comm-error: "Communication with server failed"
</i18n>

<i18n lang="yaml" locale="zh-CN">
display:
  latest: "最新版本"
  update: "需要更新"
  error: "版本获取失败"
  wait: "等待版本获取"
msg:
  update: "非最新版本"
  error: "版本获取失败"
  comm-error: "与服务器通信失败"
</i18n>
